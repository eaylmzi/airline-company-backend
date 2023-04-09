using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto;
using airlinecompany.Data.Models.dto.Flights.dto;
using airlinecompany.Data.Models.dto.Passengers.dto;
using airlinecompany.Data.Models.dto.SessionPassenger.dto;
using airlinecompany.Data.Repositories.Flights;
using airlinecompany.Data.Resources.String;
using airlinecompany.Logic.Logics.Companies;
using airlinecompany.Logic.Logics.FlightAttendants;
using airlinecompany.Logic.Logics.Flights;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using airlinecompany.Logic.Logics.SessionPassengers;
using AirlineCompanyAPI.Services.Cipher;
using AirlineCompanyAPI.Services.Jwt;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Org.BouncyCastle.Crypto;
using System.Diagnostics.Metrics;

namespace AirlineCompanyAPI.Services.User
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly ICipherService _cipherService;
        private readonly IConfiguration _configuration;
        private readonly ISessionPassengerLogic _sessionPassengerLogic;
        private readonly IFlightLogic _flightLogic;

        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;
        private readonly ICompanyLogic _companyLogic;
        private readonly IFlightAttendantLogic _flightAttendantLogic;


        public UserService(IMapper mapper, IJwtService jwtService, IConfiguration configuration, IPassengerLogic passengerLogic,
            IPlaneLogic planeLogic, ICompanyLogic companyLogic, IFlightAttendantLogic flightAttendantLogic,
            ICipherService cipherService, ISessionPassengerLogic sessionPassengerLogic, IFlightLogic flightLogic)

        {
            _mapper = mapper;
            _jwtService = jwtService;
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic;
            _companyLogic = companyLogic;
            _flightAttendantLogic = flightAttendantLogic;
            _sessionPassengerLogic = sessionPassengerLogic;
            _configuration = configuration;
            _cipherService = cipherService;
            _flightLogic = flightLogic;
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
            return updatedPassenger;
            
        }
       
        public async Task<Response<Passenger>> SignUp(PassengerDto passengerDto)
        {
            Passenger? isAlreadyAdded = _passengerLogic.GetSingleByUsername(passengerDto.UserName);
            if (isAlreadyAdded != null)
            {
                return new Response<Passenger> { Message = Error.AlreadyAddedPassenger, Data = new Passenger(), Progress = false };
            }

            passengerDto.Money = 0;
            
            Passenger newPassenger = _mapper.Map<Passenger>(passengerDto);
            int passengerId = _passengerLogic.Add(newPassenger);
            if (passengerId == -1)
            {
                return new Response<Passenger> { Message = Error.NotAddedPassenger, Data = new Passenger(), Progress = false };
            }

            Passenger isUpdatedPassenger = await AssingToken(passengerId);
            if (isUpdatedPassenger != null)
            {
                return new Response<Passenger> { Message = Success.SuccesfullySignUp, Data = new Passenger(), Progress = true };
            }
            _passengerLogic.Delete(passengerId);
            return new Response<Passenger> { Message = Error.NotAssignedToken, Data = new Passenger(), Progress = false };
        }
        public async Task<Response<PurchasedFlight>> BuyTicket(FlightDetails flightDetails, IHeaderDictionary headers)
        {
            PurchasedFlight purchasedFlight = _mapper.Map<PurchasedFlight>(flightDetails);
            Passenger? passenger = _passengerLogic.GetSingleByUsername(_jwtService.GetUserNameFromToken(headers));
            if (passenger == null)
            {
                return new Response<PurchasedFlight> { Message = Error.NotFoundPassenger, Data = new PurchasedFlight(), Progress = false };
            }

            if(passenger.Money < flightDetails.Price)
            {
                return new Response<PurchasedFlight> { Message = Error.NotEnoughMoney, Data = new PurchasedFlight(), Progress = false };
            }

            Response<Passenger> isPaid = await TransferMoneyToCompany(passenger, flightDetails);
            if (!isPaid.Progress)
            {
                return new Response<PurchasedFlight> { Message = Error.NotPaidPayment, Data = new PurchasedFlight(), Progress = false };
            }


            int sessionPassenger = _sessionPassengerLogic.Add(new SessionPassenger
            {
                FlightId = flightDetails.FlightId,
                PassengerId = _jwtService.GetUserIdFromToken(headers)
            });
            if (sessionPassenger < 0)
            {
                Response<Passenger> isReceived = await TransferMoneyToPassenger(passenger, flightDetails);
                if (isReceived.Progress)
                {
                    return new Response<PurchasedFlight> { Message = Error.NotRegisteredToFlight, Data = new PurchasedFlight(), Progress = false };
                }
                return new Response<PurchasedFlight> { Message = Error.NotReceivedPaymentForPassenger, Data = new PurchasedFlight(), Progress = false };
            }

            Flight? flight = _flightLogic.GetSingle(flightDetails.FlightId);
            if(flight != null)
            {
                flight.PassengerCount = flight.PassengerCount + 1;
                await _flightLogic.UpdateAsync(flight.Id, flight);
            }
            return new Response<PurchasedFlight> { Message = Success.SuccesfullyBought, Data = purchasedFlight, Progress = true };



        }
     
        private async Task<Response<Passenger>> TransferMoneyToCompany(Passenger passenger,FlightDetails flightDetails)
        {
            PassengerMoneyTransaction isPaid = await PayToCompany(passenger, flightDetails);
            if (isPaid.IsConfirmed)
            {
                PassengerMoneyTransaction isReceived = await ReceiveMoneyByCompany(flightDetails);
                if (isReceived.IsConfirmed)
                {
                    return new Response<Passenger> { Message = isReceived.Message, Data = new Passenger(), Progress = isReceived.IsConfirmed };
                }
                else
                {
                  PassengerMoneyTransaction isReceivedByPassenger = await ReceivedMoneyByPassenger(passenger, flightDetails);
                    if (isReceivedByPassenger.IsConfirmed)
                    {
                        return new Response<Passenger> { Message = isReceivedByPassenger.Message, Data = new Passenger(), Progress = isReceivedByPassenger.IsConfirmed };
                    }
                    return new Response<Passenger> { Message = isReceivedByPassenger.Message, Data = new Passenger(), Progress = isReceivedByPassenger.IsConfirmed };
                }
            }
            return new Response<Passenger> { Message = isPaid.Message, Data = new Passenger(), Progress = false};
        }
        private async Task<Response<Passenger>> TransferMoneyToPassenger(Passenger passenger, FlightDetails flightDetails)
        {
            PassengerMoneyTransaction isPaid = await PayToPassenger(flightDetails);
            if (isPaid.IsConfirmed)
            {
                PassengerMoneyTransaction isReceived = await ReceivedMoneyByPassenger(passenger,flightDetails);
                if (isReceived.IsConfirmed)
                {
                    return new Response<Passenger> { Message = isReceived.Message, Data = new Passenger(), Progress = isReceived.IsConfirmed };
                }
                else
                {
                    PassengerMoneyTransaction isReceivedByCompany =  await ReceiveMoneyByCompany(flightDetails);
                    if (isReceivedByCompany.IsConfirmed)
                    {
                        return new Response<Passenger> { Message = isReceivedByCompany.Message, Data = new Passenger(), Progress = isReceivedByCompany.IsConfirmed };
                    }
                    return new Response<Passenger> { Message = isReceivedByCompany.Message, Data = new Passenger(), Progress = isReceivedByCompany.IsConfirmed };
                }
            }
            return new Response<Passenger> { Message = isPaid.Message, Data = new Passenger(), Progress = false };
        }
        private async Task<PassengerMoneyTransaction> ReceiveMoneyByCompany(FlightDetails flightDetails)
        {
            Flight? flight = _flightLogic.GetSingle(flightDetails.FlightId);
            if (flight != null)
            {
                Company? company = _companyLogic.GetSingle(flight.CompanyId);
                if(company != null)
                {
                    company.TotalMoney = company.TotalMoney + flightDetails.Price;
                    Company? updatedResult = await _companyLogic.UpdateAsync(company.Id, company);
                    if(updatedResult != null)
                    {
                        return new PassengerMoneyTransaction {IsConfirmed = true }; 
                    }
                    return new PassengerMoneyTransaction { Message = Error.NotReceivedMoneyCompany, IsConfirmed = false }; 
                }
                return new PassengerMoneyTransaction { Message = Error.NotFoundCompany, IsConfirmed = false };
            }
            return new PassengerMoneyTransaction { Message = Error.NotFoundFlight , IsConfirmed = false};
        }
        private async Task<PassengerMoneyTransaction> ReceivedMoneyByPassenger(Passenger passenger, FlightDetails flightDetails)
        {
            passenger.Money = passenger.Money + flightDetails.Price;
            Passenger? isPaid = await _passengerLogic.UpdateAsync(passenger.Id, passenger);
            if (isPaid != null)
            {
                return new PassengerMoneyTransaction{IsConfirmed = true };
            }
            return new PassengerMoneyTransaction {Message = Error.NotReceivedMoneyPassenger, IsConfirmed = false };
        }
        private async Task<PassengerMoneyTransaction> PayToCompany(Passenger passenger, FlightDetails flightDetails)
        {
            passenger.Money = passenger.Money - flightDetails.Price;
            Passenger? isPaid = await _passengerLogic.UpdateAsync(passenger.Id, passenger);
            if (isPaid != null)
            {
                return new PassengerMoneyTransaction {IsConfirmed = true};
            }
            return new PassengerMoneyTransaction { Message = Error.NotOccuredTransaction, IsConfirmed = true};
        }
        private async Task<PassengerMoneyTransaction> PayToPassenger(FlightDetails flightDetails)
        {
            Flight? flight = _flightLogic.GetSingle(flightDetails.FlightId);
            if (flight != null)
            {
                Company? company = _companyLogic.GetSingle(flight.CompanyId);
                if (company != null)
                {
                    company.TotalMoney = company.TotalMoney - flightDetails.Price;
                    Company? updatedResult = await _companyLogic.UpdateAsync(company.Id, company);
                    if (updatedResult != null)
                    {
                        return new PassengerMoneyTransaction { IsConfirmed = true };
                    }
                    return new PassengerMoneyTransaction { Message = Error.NotOccuredTransaction, IsConfirmed = false };
                }
                return new PassengerMoneyTransaction { Message = Error.NotFoundCompany, IsConfirmed = false };
            }
            return new PassengerMoneyTransaction {Message = Error.NotFoundFlight, IsConfirmed = false};
        }




        public bool SignIn(string password,Passenger passenger)
        {
            bool isVerified = _cipherService.VerifyPasswordHash(passenger.PasswordHash, passenger.PasswordSalt, password);
            return isVerified;
        }
        public bool Verify(IHeaderDictionary headers)
        {
            string role = _jwtService.GetUserRoleFromToken(headers);
            if (role == Role.Passenger)
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
