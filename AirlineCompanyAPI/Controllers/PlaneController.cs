using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Credentials.dto;
using airlinecompany.Data.Models.dto.Planes.dto;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PlaneController : ControllerBase
    {
        Plane emptyObject = new Plane();

        private readonly IMapper _mapper;
        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;


        public PlaneController(IMapper mapper, IPassengerLogic passengerLogic, IPlaneLogic planeLogic)
        {
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic;
            _mapper = mapper;
        }
        //  [HttpPost, Authorize(Roles = $"{Roles.Driver},{Roles.Admin},{Roles.SuperAdmin}")]

        [HttpPost]
        public ActionResult<IdDto> Add([FromBody]PlaneDto planeDto)
        {
            try
            {
                Plane plane = _mapper.Map<Plane>(planeDto);
                return new IdDto { Id = _planeLogic.Add(plane) };              
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
                return new BooleanDto { isHappened = _planeLogic.Delete(idDto.Id) };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<Plane> Get([FromBody] IdDto idDto)
        {
            try
            {
                Plane? plane = _planeLogic.GetSingle(idDto.Id);
                if(plane != null)
                {
                    return Ok(plane);
                }
                return Ok(emptyObject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Plane>> Update([FromBody] Plane updatedEntity)
        {
            try
            {
                Plane? updatedPlane = await _planeLogic.UpdateAsync(updatedEntity.Id, updatedEntity);
                if (updatedPlane != null)
                {
                    return Ok(updatedPlane);
                }
                return Ok(emptyObject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}