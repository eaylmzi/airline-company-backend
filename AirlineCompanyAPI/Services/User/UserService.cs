using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Passengers.dto;
using airlinecompany.Data.Resources.String;
using airlinecompany.Logic.Logics.Companies;
using airlinecompany.Logic.Logics.FlightAttendants;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using AirlineCompanyAPI.Services.Cipher;
using AirlineCompanyAPI.Services.Jwt;
using AutoMapper;
using Org.BouncyCastle.Crypto;

namespace AirlineCompanyAPI.Services.User
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly ICipherService _cipherService;
        private readonly IConfiguration _configuration;

        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;
        private readonly ICompanyLogic _companyLogic;
        private readonly IFlightAttendantLogic _flightAttendantLogic;


        public UserService(IMapper mapper, IJwtService jwtService, IConfiguration configuration, IPassengerLogic passengerLogic,
            IPlaneLogic planeLogic, ICompanyLogic companyLogic, IFlightAttendantLogic flightAttendantLogic, ICipherService cipherService)

        {
            _mapper = mapper;
            _jwtService = jwtService;
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic;
            _companyLogic = companyLogic;
            _flightAttendantLogic = flightAttendantLogic;
            _configuration = configuration;
            _cipherService = cipherService;
        }

        private SignUpResult CreateSignUpResult(string message,int passengerId,Passenger passenger)
        {
            return new SignUpResult
            {
                ResultMessage = message,
                isPassengerAdded = passengerId,
                passenger = passenger
            };
        }
        private async Task<Passenger> AssingToken(int passengerId)
        {
            Passenger? addedPassenger = _passengerLogic.GetSingle(passengerId);
            Passenger? updatedPassenger = addedPassenger;
            if (addedPassenger != null)
            {
                updatedPassenger.Token = _jwtService.CreateToken(updatedPassenger, Role.Passenger, _configuration);
                updatedPassenger = await _passengerLogic.UpdateAsync(passengerId, updatedPassenger);
                return updatedPassenger;               
            }

            _passengerLogic.Delete(passengerId);
            return updatedPassenger;
            


        }
       
        public async Task<SignUpResult> SignUp(PassengerDto passengerDto)
        {
            Passenger? isAlreadyAdded = _passengerLogic.GetSingleByUsername(passengerDto.UserName);
            if (isAlreadyAdded != null)
            {
                return CreateSignUpResult(Error.AlreadyAddedPassenger, -1, new Passenger());
            }

            passengerDto.Money = 0;
            
            Passenger newPassenger = _mapper.Map<Passenger>(passengerDto);
            int passengerId = _passengerLogic.Add(newPassenger);
            if (passengerId == -1)
            {
                return CreateSignUpResult(Error.NotAddedPassenger, passengerId,new Passenger());
            }

            Passenger isUpdatedPassenger = await AssingToken(passengerId);
            if (isUpdatedPassenger != null)
            {
                return CreateSignUpResult(Success.SuccesfullySignUp, passengerId, isUpdatedPassenger);
            }
            return CreateSignUpResult(Error.NotAssignedToken, -1, new Passenger());
        }

        public bool SignIn(string password,Passenger passenger)
        {
            bool isVerified = _cipherService.VerifyPasswordHash(passenger.PasswordHash, passenger.PasswordSalt, password);
            return isVerified;
        }
        public bool Verify(IHeaderDictionary headers,string role)
        {
            if(role == Role.Passenger)
            {
                Passenger? passenger = _passengerLogic.GetSingle(_jwtService.GetUserIdFromToken(headers));
                if (passenger != null)
                {
                    if (passenger.Token == _jwtService.GetUserTokenFromToken(headers))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
