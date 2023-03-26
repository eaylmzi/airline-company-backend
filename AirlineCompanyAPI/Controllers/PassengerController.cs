using airlinecompany.Data.Models.dto.Credentials.dto;
using airlinecompany.Data.Models.dto.FlightAttendant.dto;
using airlinecompany.Data.Models;
using airlinecompany.Data.Resources.String;
using airlinecompany.Logic.Logics.Companies;
using airlinecompany.Logic.Logics.FlightAttendants;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AirlineCompanyAPI.Services.Jwt;
using System.Data;
using Microsoft.Extensions.Configuration;
using AirlineCompanyAPI.Services.User;
using airlinecompany.Data.Models.dto.Passengers.dto;
using Microsoft.AspNetCore.Authorization;

namespace AirlineCompanyAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PassengerController : Controller
    {
        Passenger emptyObject = new Passenger();
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;
        private readonly ICompanyLogic _companyLogic;
        private readonly IFlightAttendantLogic _flightAttendantLogic;


        public PassengerController(IMapper mapper, IJwtService jwtService, IUserService userService, IConfiguration configuration, IPassengerLogic passengerLogic, 
            IPlaneLogic planeLogic, ICompanyLogic companyLogic, IFlightAttendantLogic flightAttendantLogic)

        {
            _mapper = mapper;
            _jwtService = jwtService;
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic; 
            _companyLogic = companyLogic;
            _flightAttendantLogic = flightAttendantLogic;
            _configuration = configuration;
            _userService = userService;
         }
        [HttpPost]
        public async Task<ActionResult<SignUpResult>> SignUp([FromBody] PassengerDto passengerDto)
        {
            try
            {
                if (_userService.Verify(Request.Headers, _jwtService.GetUserRoleFromToken(Request.Headers)))
                {
                    Passenger newEntity = _mapper.Map<Passenger>(passengerDto);
                    return await _userService.SignUp(newEntity);
                }
                return BadRequest(Error.NotMatchedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Passenger>> SignIn([FromBody] SignInDto signInDto)
        {
            try
            {
                Passenger? passenger = _passengerLogic.GetSingleByUsernameAndPassword(signInDto.UserName, signInDto.Password);
                if (passenger != null)
                {
                    return Ok(passenger);
                }
                return BadRequest(Error.NotFoundUser);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.Passenger}")]
        public ActionResult<BooleanDto> Delete([FromBody] IdDto idDto)
        {
            try
            {
                if (_userService.Verify(Request.Headers, _jwtService.GetUserRoleFromToken(Request.Headers)))
                {
                    return new BooleanDto { isHappened = _passengerLogic.Delete(idDto.Id) };
                }
                return BadRequest(Error.NotMatchedUser);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }




        }
        [HttpPost]
        public ActionResult<Passenger> Get([FromBody] IdDto idDto)
        {
            try
            {
                Passenger? passenger = _passengerLogic.GetSingle(idDto.Id);
                if (passenger != null)
                {
                    return Ok(passenger);
                }
                return Ok(emptyObject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.Passenger}")]
        public async Task<ActionResult<PassengerDto>> Update([FromBody] PassengerDto passengerDto)
        {
            try
            {
                if (_userService.Verify(Request.Headers, _jwtService.GetUserRoleFromToken(Request.Headers))) 
                {
                    Passenger? isPassengerFound = _passengerLogic.GetSingle(_jwtService.GetUserIdFromToken(Request.Headers));
                    if (isPassengerFound != null)
                    {
                        Passenger? isUserNameUnique = _passengerLogic.GetSingleByUsername(passengerDto.UserName);
                        if (isUserNameUnique != null)
                        {
                            return BadRequest(Error.AlreadyAddedUsername);
                        }

                        Passenger newPassenger = _mapper.Map<Passenger>(passengerDto);
                        newPassenger.Id = _jwtService.GetUserIdFromToken(Request.Headers);
                        newPassenger.Money = isPassengerFound.Money;
                        newPassenger.Token = isPassengerFound.Token;

                        Passenger? updatedPassenger = await _passengerLogic.UpdateAsync(isPassengerFound.Id, newPassenger);
                        if (updatedPassenger != null)
                        {
                            PassengerDto updatedPassengerDto = _mapper.Map<PassengerDto>(updatedPassenger);
                            return Ok(updatedPassengerDto);
                        }

                        return Ok(emptyObject);
                    }
                    return BadRequest(Error.NotFoundPassenger);

                }
                return BadRequest(Error.NotMatchedUser);
                

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
