using airlinecompany.Data.Models;
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
        public ActionResult<int> Add([FromBody]PlaneDto planeDto)
        {
            try
            {
                Plane plane = _mapper.Map<Plane>(planeDto);
                return _planeLogic.Add(plane);              
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}