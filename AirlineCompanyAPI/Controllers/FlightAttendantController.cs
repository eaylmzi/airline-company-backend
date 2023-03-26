using airlinecompany.Data.Models.dto.Companies.dto;
using airlinecompany.Data.Models.dto.Credentials.dto;
using airlinecompany.Data.Models;
using airlinecompany.Logic.Logics.Companies;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using airlinecompany.Logic.Logics.FlightAttendants;
using airlinecompany.Data.Models.dto.FlightAttendant.dto;
using airlinecompany.Data.Resources.String;

namespace AirlineCompanyAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FlightAttendantController : Controller
    {
        FlightAttendant emptyObject = new FlightAttendant();
        private readonly IMapper _mapper;
        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;
        private readonly ICompanyLogic _companyLogic;
        private readonly IFlightAttendantLogic _flightAttendantLogic;


        public FlightAttendantController(IMapper mapper, IPassengerLogic passengerLogic, IPlaneLogic planeLogic, ICompanyLogic companyLogic,
           IFlightAttendantLogic flightAttendantLogic)
        {
            _mapper = mapper;
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic;
            _companyLogic = companyLogic;
            _flightAttendantLogic = flightAttendantLogic;


        }
        [HttpPost]
        public ActionResult<IdDto> Add([FromBody] FlightAttendantDto flightAttendantDto)
        {
            try
            {
                Company? isCompanyFound = _companyLogic.GetSingle(flightAttendantDto.CompanyId);
                if (isCompanyFound != null)
                {
                    FlightAttendant flightAttendant = _mapper.Map<FlightAttendant>(flightAttendantDto);
                    return new IdDto { Id = _flightAttendantLogic.Add(flightAttendant) };
                }
                return BadRequest(Error.NotFoundCompany);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<BooleanDto> Delete([FromBody] IdDto idDto)
        {
            try
            {
                return new BooleanDto { isHappened = _flightAttendantLogic.Delete(idDto.Id) };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<FlightAttendant> Get([FromBody] IdDto idDto)
        {
            try
            {
                FlightAttendant? flightAttendant = _flightAttendantLogic.GetSingle(idDto.Id);
                if (flightAttendant != null)
                {
                    return Ok(flightAttendant);
                }
                return Ok(emptyObject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<FlightAttendant>> Update([FromBody] FlightAttendant flightAttendant)
        {
            try
            {
                Company? isCompanyFound = _companyLogic.GetSingle(flightAttendant.CompanyId);
                if (isCompanyFound != null)
                {
                    FlightAttendant? updatedFlightAttendant = await _flightAttendantLogic.UpdateAsync(flightAttendant.Id, flightAttendant);
                    if (updatedFlightAttendant != null)
                    {
                        return Ok(updatedFlightAttendant);
                    }

                    return Ok(emptyObject);
                }
                return BadRequest(Error.NotFoundCompany);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
