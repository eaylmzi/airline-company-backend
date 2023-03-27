using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Passengers.dto;

namespace AirlineCompanyAPI.Services.User
{
    public interface IUserService
    {
        public Task<SignUpResult> SignUp(PassengerDto passengerDto);
        public bool SignIn(string password, Passenger passenger);
        public bool Verify(IHeaderDictionary headers, string role);
    }
}
