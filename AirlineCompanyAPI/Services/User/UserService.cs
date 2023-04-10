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

        private async Task<PassengerMoneyTransaction> TransferMoneyToCompany(Passenger passenger, FlightDetails flightDetails)
        {
            PassengerMoneyTransaction isPaid = await PayToCompany(passenger, flightDetails);
            if (!isPaid.IsConfirmed)
            {
                return new PassengerMoneyTransaction { Message = isPaid.Message, IsConfirmed = false };
            }
            PassengerMoneyTransaction isReceived = await ReceiveMoneyByCompany(flightDetails);
            if (!isReceived.IsConfirmed)
            {
                PassengerMoneyTransaction isReceivedByPassenger = await ReceivedMoneyByPassenger(passenger, flightDetails);
                if (!isReceivedByPassenger.IsConfirmed)
                {
                    return new PassengerMoneyTransaction { Message = isReceived.Message + isReceivedByPassenger.Message, IsConfirmed = false };
                }
                return new PassengerMoneyTransaction { Message = isReceived.Message , IsConfirmed = false };            
            }
            return new PassengerMoneyTransaction {IsConfirmed = true };
        }
        private async Task<PassengerMoneyTransaction> TransferMoneyToPassenger(Passenger passenger, FlightDetails flightDetails)
        {
            PassengerMoneyTransaction isPaid = await PayToPassenger(flightDetails);
            if (!isPaid.IsConfirmed)
            {
                return new PassengerMoneyTransaction { Message = isPaid.Message, IsConfirmed = false};
            }
            PassengerMoneyTransaction isReceived = await ReceivedMoneyByPassenger(passenger, flightDetails);
            if (!isReceived.IsConfirmed)
            {
                PassengerMoneyTransaction isReceivedByCompany = await ReceiveMoneyByCompany(flightDetails);
                if (!isReceivedByCompany.IsConfirmed)
                {
                    return new PassengerMoneyTransaction { Message = isReceived.Message + isReceivedByCompany.Message, IsConfirmed = isReceivedByCompany.IsConfirmed };
                }
                return new PassengerMoneyTransaction { Message = isReceivedByCompany.Message, IsConfirmed = isReceivedByCompany.IsConfirmed };
            }
            return new PassengerMoneyTransaction { IsConfirmed = true };
        }
        private async Task<PassengerMoneyTransaction> ReceiveMoneyByCompany(FlightDetails flightDetails)
        {
            Flight? flight = _flightLogic.GetSingle(flightDetails.FlightId);
            if (flight != null)
            {
                Company? company = _companyLogic.GetSingle(flight.CompanyId);
                if (company != null)
                {
                    company.TotalMoney = company.TotalMoney + flightDetails.Price;
                    Company? updatedResult = await _companyLogic.UpdateAsync(company.Id, company);
                    if (updatedResult != null)
                    {
                        return new PassengerMoneyTransaction { IsConfirmed = true };
                    }
                    return new PassengerMoneyTransaction { Message = Error.NotReceivedMoneyCompany, IsConfirmed = false };
                }
                return new PassengerMoneyTransaction { Message = Error.NotFoundCompany, IsConfirmed = false };
            }
            return new PassengerMoneyTransaction { Message = Error.NotFoundFlight, IsConfirmed = false };
        }
        private async Task<PassengerMoneyTransaction> ReceivedMoneyByPassenger(Passenger passenger, FlightDetails flightDetails)
        {
            passenger.Money = passenger.Money + flightDetails.Price;
            Passenger? isPaid = await _passengerLogic.UpdateAsync(passenger.Id, passenger);
            if (isPaid != null)
            {
                return new PassengerMoneyTransaction { IsConfirmed = true };
            }
            return new PassengerMoneyTransaction { Message = Error.NotReceivedMoneyPassenger, IsConfirmed = false };
        }
        private async Task<PassengerMoneyTransaction> PayToCompany(Passenger passenger, FlightDetails flightDetails)
        {
            passenger.Money = passenger.Money - flightDetails.Price;
            Passenger? isPaid = await _passengerLogic.UpdateAsync(passenger.Id, passenger);
            if (isPaid != null)
            {
                return new PassengerMoneyTransaction { IsConfirmed = true };
            }
            return new PassengerMoneyTransaction { Message = Error.NotOccuredTransaction, IsConfirmed = true };
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
            return new PassengerMoneyTransaction { Message = Error.NotFoundFlight, IsConfirmed = false };
        }
        private bool IsFlightEmpty(Flight flight, Plane plane)
        {
            if (flight.PassengerCount < plane.SeatNumber)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<Response<PurchasedFlight>> BuyTicket(FlightDetails flightDetails, IHeaderDictionary headers)
        {
            PurchasedFlight purchasedFlight = _mapper.Map<PurchasedFlight>(flightDetails);
            Flight? flight = _flightLogic.GetSingle(flightDetails.FlightId);
            if (flight == null)
            {
                return new Response<PurchasedFlight> { Message = Error.NotFoundFlight, Data = new PurchasedFlight(), Progress = false };
            }
            Plane? plane = _planeLogic.GetSingle(flight.PlaneNumber);
            if(plane == null)
            {
                return new Response<PurchasedFlight> { Message = Error.NotFoundFlight, Data = new PurchasedFlight(), Progress = false };
            }
            bool isPlaneEmpty = IsFlightEmpty(flight, plane);
            if (!isPlaneEmpty)
            {
                return new Response<PurchasedFlight> { Message = Error.FullPlane, Data = new PurchasedFlight(), Progress = false };
            }


            Passenger? passenger = _passengerLogic.GetSingleByUsername(_jwtService.GetUserNameFromToken(headers));
            if (passenger == null)
            {
                return new Response<PurchasedFlight> { Message = Error.NotFoundPassenger, Data = new PurchasedFlight(), Progress = false };
            }

            if(passenger.Money < flightDetails.Price)
            {
                return new Response<PurchasedFlight> { Message = Error.NotEnoughMoney, Data = new PurchasedFlight(), Progress = false };
            }

            PassengerMoneyTransaction isPaid = await TransferMoneyToCompany(passenger, flightDetails);
            if (!isPaid.IsConfirmed)
            {
                return new Response<PurchasedFlight> { Message = isPaid.Message, Data = new PurchasedFlight(), Progress = false };
            }


            int sessionPassenger = _sessionPassengerLogic.Add(new SessionPassenger
            {
                FlightId = flightDetails.FlightId,
                PassengerId = _jwtService.GetUserIdFromToken(headers)
            });
            if (sessionPassenger < 0)
            {
                PassengerMoneyTransaction isReceived = await TransferMoneyToPassenger(passenger, flightDetails);
                if (!isReceived.IsConfirmed)
                {
                    return new Response<PurchasedFlight> { Message = Error.NotReceivedPaymentForPassenger, Data = new PurchasedFlight(), Progress = false };
                }
                return new Response<PurchasedFlight> { Message = Error.NotRegisteredToFlight, Data = new PurchasedFlight(), Progress = false };             
            }
        
            flight.PassengerCount = flight.PassengerCount + 1;
            Flight? isUpdated = await _flightLogic.UpdateAsync(flight.Id, flight);
            if (isUpdated == null)
            {
                PassengerMoneyTransaction isReceived = await TransferMoneyToPassenger(passenger, flightDetails);
                if (!isReceived.IsConfirmed)
                {
                    return new Response<PurchasedFlight> { Message = Error.NotReceivedPaymentForPassenger, Data = new PurchasedFlight(), Progress = false };
                }
                return new Response<PurchasedFlight> { Message = Error.NotRegisteredToFlight, Data = new PurchasedFlight(), Progress = false };
            }

            else
            {
                return new Response<PurchasedFlight> { Message = Success.SuccesfullyBought, Data = purchasedFlight, Progress = true };
            }

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
