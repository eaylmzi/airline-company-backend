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

namespace AirlineCompanyAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FlightController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;
        private readonly ICompanyLogic _companyLogic;
        private readonly IFlightLogic _flightLogic;


        public FlightController(IMapper mapper, IPassengerLogic passengerLogic, IPlaneLogic planeLogic, ICompanyLogic companyLogic,
            IFlightLogic flightLogic)
        {
            _mapper = mapper;
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic;
            _companyLogic = companyLogic;
            _flightLogic = flightLogic;


        }
        [HttpPost]
        public ActionResult<Response<int>> Add([FromBody] FlightDto flightDto)
        {
            try
            {
                Flight flight = _mapper.Map<Flight>(flightDto);
                int flightId = _flightLogic.Add(flight);
                if (flightId != -1)
                {
                    return Ok(new Response<int> { Message = Success.SuccesfullyAddedFlight, Data = flightId });
                }
                return Ok(new Response<int> { Message = Error.NotAddedFlight, Data = flightId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<Response<bool>> Delete([FromBody] IdDto idDto)
        {
            try
            {
                bool isDeleted = _flightLogic.Delete(idDto.Id);
                if (isDeleted)
                {
                    return Ok(new Response<bool> { Message = Success.SuccesfullyDeletedFlight, Data = isDeleted });
                }
                return Ok(new Response<bool> { Message = Error.NotDeletedFlight, Data = isDeleted });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
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
        [HttpPost]
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
    }
}
