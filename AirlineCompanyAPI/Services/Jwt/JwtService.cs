using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Passengers.dto;
using airlinecompany.Logic.Logics.Companies;
using airlinecompany.Logic.Logics.FlightAttendants;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using AirlineCompanyAPI.Services.User;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AirlineCompanyAPI.Services.Jwt
{
    public class JwtService : IJwtService
    {

        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;
        private readonly ICompanyLogic _companyLogic;
        private readonly IFlightAttendantLogic _flightAttendantLogic;


        public JwtService(IMapper mapper, IConfiguration configuration, IPassengerLogic passengerLogic,
            IPlaneLogic planeLogic, ICompanyLogic companyLogic, IFlightAttendantLogic flightAttendantLogic)

        {
            _mapper = mapper;
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic;
            _companyLogic = companyLogic;
            _flightAttendantLogic = flightAttendantLogic;
            _configuration = configuration;
        }
        public string CreateToken(Passenger passenger, string role, IConfiguration _configuration)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role,role),
                new Claim("id",passenger.Id.ToString()),
                new Claim("username",passenger.UserName.ToString()),
                new Claim("role",role.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
              _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMonths(9),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        public bool validateToken(string token, IConfiguration _configuration)
        {
            try
            {
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSettings:Token").Value));
                JwtSecurityTokenHandler handler = new();
                handler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false
                }, out SecurityToken validatedToken);
                //var jwtToken = (JwtSecurityToken)validatedToken;
                //var claims = jwtToken.Claims.ToList();
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        public int GetUserIdFromToken(IHeaderDictionary headers)
        {
            string requestToken = headers[HeaderNames.Authorization].ToString().Replace("bearer ", "");
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(requestToken);
            string user = jwt.Claims.First(c => c.Type == "id").Value;
            int userId = int.Parse(user);
            return userId;
        }
        public string GetUserRoleFromToken(IHeaderDictionary headers)
        {
            string requestToken = headers[HeaderNames.Authorization].ToString().Replace("bearer ", "");
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(requestToken);
            string role = jwt.Claims.First(c => c.Type == "role").Value;
            return role;
        }
        public string GetUserTokenFromToken(IHeaderDictionary headers)
        {
            string requestToken = headers[HeaderNames.Authorization].ToString().Replace("bearer ", "");
            return requestToken;
        }
        
        public PassengerVerifying GetUserInformation(IHeaderDictionary headers)
        {
            PassengerVerifying userVerifyingDto = new PassengerVerifying()
            {
                Id = GetUserIdFromToken(headers),
                Token = GetUserTokenFromToken(headers)
            };
            return userVerifyingDto;
        }

    }
}
