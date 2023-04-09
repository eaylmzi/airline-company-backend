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
using AirlineCompanyAPI.Services.Cipher;
using airlinecompany.Data.Models.dto;

namespace AirlineCompanyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1")]
    public class PassengerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly ICipherService _cipherService;

        private readonly IConfiguration _configuration;

        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;
        private readonly ICompanyLogic _companyLogic;
        private readonly IFlightAttendantLogic _flightAttendantLogic;


        public PassengerController(IMapper mapper, IJwtService jwtService, IUserService userService, ICipherService cipherService, IConfiguration configuration, IPassengerLogic passengerLogic, 
            IPlaneLogic planeLogic, ICompanyLogic companyLogic, IFlightAttendantLogic flightAttendantLogic)

        {
            _mapper = mapper;
            _jwtService = jwtService;
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic;
            _cipherService = cipherService;
            _companyLogic = companyLogic;
            _flightAttendantLogic = flightAttendantLogic;
            _configuration = configuration;
            _userService = userService;
         }
        [HttpPost]
        public async Task<ActionResult<Response<Passenger>>> SignUp([FromBody] PassengerSignUpDto passengerSignUpDto)
        {
            try
            {
                PassengerDto newEntity = new PassengerDto();
                newEntity = _mapper.Map<PassengerDto>(passengerSignUpDto);

                _cipherService.CreatePasswordHash(passengerSignUpDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                newEntity.PasswordHash = passwordHash;
                newEntity.PasswordSalt = passwordSalt;
                Response<Passenger> passenger = await _userService.SignUp(newEntity);
                return passenger;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<Response<PassengerSignInDto>> SignIn([FromBody] SignInDto signInDto)
        {
            try
            {
                Passenger? passenger = _passengerLogic.GetSingleByUsername(signInDto.UserName);
                if (passenger != null)
                {
                    bool isLogin = _userService.SignIn(signInDto.Password, passenger);
                    if (isLogin)
                    {
                        PassengerSignInDto passengerSignInDto = _mapper.Map<PassengerSignInDto>(passenger);
                        return Ok(new Response<PassengerSignInDto> { Message = Success.SuccesfullySignIn, Data = passengerSignInDto, Progress = true });
                    }                 
                }
                return BadRequest(new Response<PassengerSignInDto> { Message = Error.NotFoundPassengerCredential, Data = new PassengerSignInDto(), Progress = false });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.Passenger}")]
        public ActionResult<Response<bool>> Delete([FromBody] IdDto idDto)
        {
            try
            {
                if (_userService.Verify(Request.Headers))
                {
                    bool isDeleted = _passengerLogic.Delete(idDto.Id);
                    if (isDeleted)
                    {
                        return Ok(new Response<bool> { Message = Success.SuccesfullyDeletedPassenger, Data = isDeleted, Progress = true });
                    }
                    return Ok(new Response<bool> { Message = Error.NotDeletedPassenger, Data = isDeleted, Progress = false });
                }
                return BadRequest(new Response<bool> { Message = Error.NotMatchedUser, Data = false, Progress = false });
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



            
        }
        [HttpPost, Authorize(Roles = $"{Role.Passenger},{Role.SuperAdmin}")]
        public ActionResult<Response<Passenger>> Get([FromBody] IdDto idDto)
        {
            try
            {
                Passenger? passenger = _passengerLogic.GetSingle(idDto.Id);
                if (passenger != null)
                {
                    return Ok(new Response<Passenger> { Message = Success.SuccesfullyAddedPassenger, Data = passenger, Progress = true });
                }
                return Ok(new Response<Passenger> { Message = Success.SuccesfullyAddedPassenger, Data = new Passenger(), Progress = false });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.Passenger}")]
        public async Task<ActionResult<Response<PassengerDto>>> Update([FromBody] PassengerDto passengerDto)
        {
            try
            {
                if (_userService.Verify(Request.Headers)) 
                {
                    Passenger? isPassengerFound = _passengerLogic.GetSingle(_jwtService.GetUserIdFromToken(Request.Headers));
                    if (isPassengerFound != null)
                    {
                        Passenger? isUserNameUnique = _passengerLogic.GetSingleByUsername(passengerDto.UserName);
                        if (isUserNameUnique != null)
                        {
                            return BadRequest(new Response<PassengerDto> { Message = Error.AlreadyAddedUsername, Data = new PassengerDto(), Progress = false });
                        }

                        Passenger newPassenger = _mapper.Map<Passenger>(passengerDto);
                        newPassenger.Id = _jwtService.GetUserIdFromToken(Request.Headers);
                        newPassenger.Money = isPassengerFound.Money;
                        newPassenger.Token = isPassengerFound.Token;

                        Passenger? updatedPassenger = await _passengerLogic.UpdateAsync(isPassengerFound.Id, newPassenger);
                        if (updatedPassenger != null)
                        {
                            PassengerDto updatedPassengerDto = _mapper.Map<PassengerDto>(updatedPassenger);
                            return Ok(new Response<PassengerDto> { Message = Success.SuccesfullyUpdatedPassenger, Data = updatedPassengerDto, Progress = true });
                        }

                        return Ok(new Response<PassengerDto> { Message = Error.NotUpdatedPassenger, Data = new PassengerDto(), Progress = false });
                    }
                    return Ok(new Response<PassengerDto> { Message = Error.NotFoundPassenger, Data = new PassengerDto(), Progress = false });

                }
                return BadRequest(new Response<PassengerDto> { Message = Error.NotMatchedUser, Data = new PassengerDto(), Progress = false });
                

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
