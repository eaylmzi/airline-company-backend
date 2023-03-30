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
using airlinecompany.Data.Models.dto;
using Microsoft.AspNetCore.Authorization;
using System.Data;

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
        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public ActionResult<Response<int>> Add([FromBody] FlightAttendantDto flightAttendantDto)
        {
            try
            {
                Company? isCompanyFound = _companyLogic.GetSingle(flightAttendantDto.CompanyId);
                if (isCompanyFound != null)
                {
                    FlightAttendant flightAttendant = _mapper.Map<FlightAttendant>(flightAttendantDto);
                    int flightAttendantId = _flightAttendantLogic.Add(flightAttendant);
                    if(flightAttendantId != -1)
                    {
                        return Ok(new Response<int> { Message = Success.SuccesfullyAddedFlightAttendant, Data = flightAttendantId });
                    }
                    return Ok(new Response<int> { Message = Error.NotAddedFlightAttendant, Data = flightAttendantId });

                }
                return BadRequest(Error.NotFoundCompany);
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
                bool isBusy = _flightAttendantLogic.CheckAvailabality(idDto.Id);
                if (!isBusy)
                {
                    bool isDeleted = _flightAttendantLogic.Delete(idDto.Id);
                    if (isDeleted)
                    {
                        return Ok(new Response<bool> { Message = Success.SuccesfullyDeletedFlightAttendant, Data = isDeleted });
                    }
                    return Ok(new Response<bool> { Message = Error.NotDeletedFlightAttendant, Data = isDeleted });
                }
               return Ok(new Response<bool> { Message = Error.NotAvailableFlightAttendant, Data = !isBusy });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public ActionResult<Response<FlightAttendant>> Get([FromBody] IdDto idDto)
        {
            try
            {
                FlightAttendant? flightAttendant = _flightAttendantLogic.GetSingle(idDto.Id);
                if (flightAttendant != null)
                {
                    return Ok(new Response<FlightAttendant> { Message = Success.SuccesfullyReceivedFlightAttendant, Data = flightAttendant });
                }
                return Ok(new Response<FlightAttendant> { Message = Error.NotDeletedFlightAttendant, Data = new FlightAttendant() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public async Task<ActionResult<Response<FlightAttendant>>> Update([FromBody] FlightAttendant flightAttendant)
        {
            try
            {
                Company? isCompanyFound = _companyLogic.GetSingle(flightAttendant.CompanyId);
                if (isCompanyFound != null)
                {
                    FlightAttendant? updatedFlightAttendant = await _flightAttendantLogic.UpdateAsync(flightAttendant.Id, flightAttendant);
                    if (updatedFlightAttendant != null)
                    {
                        return Ok(new Response<FlightAttendant> { Message = Success.SuccesfullyUpdatedFlightAttendant, Data = updatedFlightAttendant });
                    }

                    return Ok(new Response<FlightAttendant> { Message = Error.NotUpdatedFlightAttendant, Data = new FlightAttendant() });
                }
                return Ok(new Response<FlightAttendant> { Message = Error.NotFoundCompany, Data = new FlightAttendant() });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
