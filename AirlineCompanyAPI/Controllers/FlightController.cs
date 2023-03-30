using airlinecompany.Data.Models.dto.Companies.dto;
using airlinecompany.Data.Models.dto.Credentials.dto;
using airlinecompany.Data.Models.dto;
using airlinecompany.Data.Models;
using airlinecompany.Data.Resources.String;
using airlinecompany.Logic.Logics.Companies;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using airlinecompany.Logic.Logics.Flights;
using airlinecompany.Data.Models.dto.Flights.dto;
using airlinecompany.Data.Models.dto.Point.dto;
using airlinecompany.Logic.Logics.JoinTables;
using AirlineCompanyAPI.Services.User;
using System.ComponentModel.Design;
using airlinecompany.Data.Models.dto.Passengers.dto;
using AirlineCompanyAPI.Services.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace AirlineCompanyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1")]
    public class FlightController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;
        private readonly ICompanyLogic _companyLogic;
        private readonly IFlightLogic _flightLogic;
        private readonly IJoinTableLogic _joinTableLogic;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;


        public FlightController(IMapper mapper, IPassengerLogic passengerLogic, IPlaneLogic planeLogic, ICompanyLogic companyLogic,
            IFlightLogic flightLogic, IJoinTableLogic joinTableLogic, IUserService userService, IJwtService jwtService)
        {
            _mapper = mapper;
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic;
            _companyLogic = companyLogic;
            _flightLogic = flightLogic;
            _joinTableLogic = joinTableLogic;
            _userService = userService;
            _jwtService = jwtService;


        }

        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public async Task<ActionResult<Response<int>>> Add([FromBody] FlightDto flightDto)
        {
            try
            {
                bool isExist = await _flightLogic.CheckForeignKey(flightDto);
                if(isExist)
                {
                    Flight flight = _mapper.Map<Flight>(flightDto);
                    flight.PassengerCount = 0;
                    int flightId = _flightLogic.Add(flight);
                    if (flightId != -1)
                    {
                        return Ok(new Response<int> { Message = Success.SuccesfullyAddedFlight, Data = flightId });
                    }
                    return Ok(new Response<int> { Message = Error.NotAddedFlight, Data = flightId });
                }
                return BadRequest(new Response<int> { Message = Error.ForeignKeyConstraints, Data = -1 });
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public ActionResult<Response<bool>> Delete([FromBody] IdDto idDto)
        {
            try
            {
                bool isBusy = _flightLogic.CheckAvailabality(idDto.Id);
                if (!isBusy)
                {
                    bool isDeleted = _flightLogic.Delete(idDto.Id);
                    if (isDeleted)
                    {
                        return Ok(new Response<bool> { Message = Success.SuccesfullyDeletedFlight, Data = isDeleted });
                    }
                    return Ok(new Response<bool> { Message = Error.NotDeletedFlight, Data = isDeleted });
                }
                return Ok(new Response<bool> { Message = Error.NotAvailableFlight, Data = !isBusy });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public ActionResult<Response<Flight>> Get([FromBody] IdDto idDto)
        {
            try
            {
                Flight? flight = _flightLogic.GetSingle(idDto.Id);
                if (flight != null)
                {
                    return Ok(new Response<Flight> { Message = Success.SuccesfullyAddedFlight, Data = flight });
                }
                return Ok(new Response<Flight> { Message = Error.NotFoundFlight, Data = new Flight() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public async Task<ActionResult<Response<Flight>>> Update([FromBody] Flight updatedEntity)
        {
            try
            {
                Flight? updatedFlight = await _flightLogic.UpdateAsync(updatedEntity.Id, updatedEntity);
                if (updatedFlight != null)
                {
                    return Ok(new Response<Flight> { Message = Success.SuccesfullyUpdatedFlight, Data = updatedFlight });
                }
                return Ok(new Response<Flight> { Message = Error.NotUpdatedFlight, Data = new Flight() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<Response<FlightDetails>> FindFlight([FromBody] Journey journey)
        {
            try
            {
                var list = _joinTableLogic.FindFlightsByDestinationJoinTables(journey.From, journey.To, journey.Date);
                if (list != null)
                {
                    return Ok(list);
                }
                return Ok(new Response<FlightDetails> { Message = Error.NotFoundFlight, Data = new FlightDetails() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.Passenger}")]
        public async Task<ActionResult<Response<PurchasedFlight>>> BuyTicket([FromBody] FlightDetails flightDetails)
        {
            try
            {
                if(_userService.Verify(Request.Headers))
                {
                    Response<PurchasedFlight> isBought = await _userService.BuyTicket(flightDetails, Request.Headers);
                    return isBought;

                }
                return Unauthorized(Error.NotMatchedUser);
               



            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
