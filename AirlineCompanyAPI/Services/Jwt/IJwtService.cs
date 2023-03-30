using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Passengers.dto;

namespace AirlineCompanyAPI.Services.Jwt
{
    public interface IJwtService
    {
        public string CreateToken(Passenger passenger, string role, IConfiguration _configuration);
        public bool validateToken(string token, IConfiguration _configuration);
        public int GetUserIdFromToken(IHeaderDictionary headers);
        public string GetUserTokenFromToken(IHeaderDictionary headers);
        public string GetUserNameFromToken(IHeaderDictionary headers);
        public string GetUserRoleFromToken(IHeaderDictionary headers);
        public PassengerVerifying GetUserInformation(IHeaderDictionary headers);
    }
}
