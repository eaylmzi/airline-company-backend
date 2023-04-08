using airlinecompany.Data.Models.dto.Credentials.dto;
using airlinecompany.Data.Models.dto.Planes.dto;
using airlinecompany.Data.Models.dto;
using airlinecompany.Data.Resources.String;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using airlinecompany.Logic.Logics.SessionPassengers;
using airlinecompany.Data.Models.dto.SessionPassenger.dto;
using airlinecompany.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace AirlineCompanyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1")]
    public class SessionPassengerController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;
        private readonly ISessionPassengerLogic _sessionPassengerLogic;


        public SessionPassengerController(IMapper mapper, IPassengerLogic passengerLogic, IPlaneLogic planeLogic,
            ISessionPassengerLogic sessionPassengerLogic)
        {
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic;
            _mapper = mapper;
            _sessionPassengerLogic = sessionPassengerLogic;
        }
        //  [HttpPost, Authorize(Roles = $"{Roles.Driver},{Roles.Admin},{Roles.SuperAdmin}")]

        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public ActionResult<Response<int>> Add([FromBody] SessionPassengerDto sessionPassengerDto)
        {
            try
            {
                SessionPassenger sessionPassenger = _mapper.Map<SessionPassenger>(sessionPassengerDto);
                int sessionPassengerId = _sessionPassengerLogic.Add(sessionPassenger);
                if (sessionPassengerId != -1)
                {
                    return Ok(new Response<int> { Message = Success.SuccesfullyAddedSessionPassenger, Data = sessionPassengerId });
                }
                return Ok(new Response<int> { Message = Error.NotAddedSessionPassenger, Data = sessionPassengerId });
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
                bool isDeleted = _sessionPassengerLogic.Delete(idDto.Id);
                if (isDeleted)
                {
                    return Ok(new Response<bool> { Message = Success.SuccesfullyDeletedSessionPassenger, Data = isDeleted });
                }
                return Ok(new Response<bool> { Message = Error.NotDeletedSessionPassenger, Data = isDeleted });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.Passenger},{Role.SuperAdmin}")]
        public ActionResult<Response<List<SessionPassenger>>> Get([FromBody] IdDto idDto)
        {
            try
            {
                var sessionPassengerList = _sessionPassengerLogic.GetByPassengerId(idDto.Id);
                if (sessionPassengerList != null)
                {
                    return Ok(new Response<List<SessionPassenger>> { Message = Success.SuccesfullyReceivedSessionPassenger, Data = sessionPassengerList });
                }
                return Ok(new Response<List<SessionPassenger>> { Message = Error.NotAddedSessionPassenger, Data = new List<SessionPassenger>() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public async Task<ActionResult<Response<SessionPassenger>>> Update([FromBody] SessionPassenger updatedEntity)
        {
            try
            {
                SessionPassenger? updatedSessionPassenger = await _sessionPassengerLogic.UpdateAsync(updatedEntity.Id, updatedEntity);
                if (updatedSessionPassenger != null)
                {
                    return Ok(new Response<SessionPassenger> { Message = Success.SuccesfullyUpdatedSessionPassenger, Data = updatedSessionPassenger });
                }
                return Ok(new Response<SessionPassenger> { Message = Error.NotUpdatedSessionPassenger, Data = new SessionPassenger() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
