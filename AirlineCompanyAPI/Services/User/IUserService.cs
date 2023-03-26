using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Passengers.dto;

namespace AirlineCompanyAPI.Services.User
{
    public interface IUserService
    {
        public Task<SignUpResult> SignUp(Passenger passenger);
        public bool VerifyPassenger(IHeaderDictionary headers);
    }
}
