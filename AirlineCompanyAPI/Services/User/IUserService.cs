using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Flights.dto;
using airlinecompany.Data.Models.dto;
using airlinecompany.Data.Models.dto.Passengers.dto;

namespace AirlineCompanyAPI.Services.User
{
    public interface IUserService
    {
        public Task<Response<Passenger>> SignUp(PassengerDto passengerDto);
        public Task<Response<PurchasedFlight>> BuyTicket(FlightDetails flightDetails, IHeaderDictionary headers);
        public bool SignIn(string password, Passenger passenger);
        public bool Verify(IHeaderDictionary headers);
    }
}
