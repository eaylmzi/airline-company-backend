using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto;
using airlinecompany.Data.Models.dto.Credentials.dto;
using airlinecompany.Data.Models.dto.Planes.dto;
using airlinecompany.Data.Resources.String;
using airlinecompany.Logic.Logics.JoinTables;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;
using System.Data;


namespace WebApplication1.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1")]
    public class PlaneController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;
        private readonly IJoinTableLogic _joinTableLogic;



        public PlaneController(IMapper mapper, IPassengerLogic passengerLogic, IPlaneLogic planeLogic, IJoinTableLogic joinTableLogic)
        {
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic;
            _joinTableLogic = joinTableLogic;
            _mapper = mapper;
        }
        //  [HttpPost, Authorize(Roles = $"{Roles.Driver},{Roles.Admin},{Roles.SuperAdmin}")]

        [HttpPost,Authorize(Roles = $"{Role.SuperAdmin}")]
        public ActionResult<Response<int>> Add([FromBody]PlaneDto planeDto)
        {
            try
            {
                Plane plane = _mapper.Map<Plane>(planeDto);
                int planeId = _planeLogic.Add(plane);
                if(planeId != -1)
                {
                    return Ok(new Response<int> { Message = Success.SuccesfullyAddedPlane, Data = planeId, Progress = true });
                }
                return Ok(new Response<int> { Message = Error.NotAddedPlane , Data = planeId, Progress = false });              
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
                bool isBusy = _planeLogic.CheckAvailabality(idDto.Id);
                if(!isBusy)
                {
                    bool isDeleted = _planeLogic.Delete(idDto.Id);
                    if (isDeleted)
                    {
                        return Ok(new Response<bool> { Message = Success.SuccesfullyDeletedPlane, Data = isDeleted, Progress = true });
                    }
                    return Ok(new Response<bool> { Message = Error.NotDeletedPlane, Data = isDeleted, Progress = false });
                }
                return Ok(new Response<bool> { Message = Error.NotAvailablePlane, Data = !isBusy, Progress = false });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public ActionResult<Response<Plane>> Get([FromBody] IdDto idDto)
        {
            try
            {
                Plane? plane = _planeLogic.GetSingle(idDto.Id);
                if(plane != null)
                {
                    return Ok(new Response<Plane> { Message = Success.SuccesfullyReceivedPlane, Data = plane, Progress = true });
                }
                return Ok(new Response<Plane> { Message = Error.NotAddedPlane, Data = new Plane(), Progress = false });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public async Task<ActionResult<Response<Plane>>> Update([FromBody] Plane updatedEntity)
        {
            try
            {
                Plane? updatedPlane = await _planeLogic.UpdateAsync(updatedEntity.Id, updatedEntity);
                if (updatedPlane != null)
                {
                    return Ok(new Response<Plane> { Message = Success.SuccesfullyUpdatedPlane, Data = updatedPlane, Progress = true });
                }
                return Ok(new Response<Plane> { Message = Error.NotUpdatedPlane, Data = new Plane(), Progress = false });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}