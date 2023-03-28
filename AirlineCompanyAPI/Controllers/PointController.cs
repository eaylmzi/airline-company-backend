using airlinecompany.Data.Models.dto.Credentials.dto;
using airlinecompany.Data.Models.dto.Planes.dto;
using airlinecompany.Data.Models.dto;
using airlinecompany.Data.Resources.String;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using airlinecompany.Data.Models;
using airlinecompany.Logic.Logics.Points;
using airlinecompany.Data.Models.dto.Point.dto;

namespace AirlineCompanyAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PointController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;
        private readonly IPointLogic _pointLogic;


        public PointController(IMapper mapper, IPassengerLogic passengerLogic, IPlaneLogic planeLogic, IPointLogic pointLogic)
        {
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic;
            _mapper = mapper;
            _pointLogic = pointLogic;
        }
        //  [HttpPost, Authorize(Roles = $"{Roles.Driver},{Roles.Admin},{Roles.SuperAdmin}")]

        [HttpPost]
        public ActionResult<Response<int>> Add([FromBody] PointDto pointDto)
        {
            try
            {
                Point isAlreadyAdded = _pointLogic.GetSingleByName(pointDto.Name);
                if (isAlreadyAdded != null)
                {
                    return Ok(new Response<int> { Message = Warning.AlreadyAddedPoint, Data = isAlreadyAdded.Id });
                }
                Point point = _mapper.Map<Point>(pointDto);
                int pointId = _pointLogic.Add(point);
                if (pointId != -1)
                {
                    return Ok(new Response<int> { Message = Success.SuccesfullyAddedPoint, Data = pointId });
                }
                return Ok(new Response<int> { Message = Error.NotAddedPoint, Data = pointId });
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
                bool isDeleted = _pointLogic.Delete(idDto.Id);
                if (isDeleted)
                {
                    return Ok(new Response<bool> { Message = Success.SuccesfullyDeletedPoint, Data = isDeleted });
                }
                return Ok(new Response<bool> { Message = Error.NotDeletedPoint, Data = isDeleted });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<Response<Point>> Get([FromBody] IdDto idDto)
        {
            try
            {
                Point? point = _pointLogic.GetSingle(idDto.Id);
                if (point != null)
                {
                    return Ok(new Response<Point> { Message = Success.SuccesfullyReceivedPoint, Data = point });
                }
                return Ok(new Response<Point> { Message = Error.NotAddedPoint, Data = new Point() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Response<Point>>> Update([FromBody] Point updatedEntity)
        {
            try
            {
                Point? updatedPoint = await _pointLogic.UpdateAsync(updatedEntity.Id, updatedEntity);
                if (updatedPoint != null)
                {
                    return Ok(new Response<Point> { Message = Success.SuccesfullyUpdatedPoint, Data = updatedPoint });
                }
                return Ok(new Response<Point> { Message = Error.NotUpdatedPoint, Data = new Point() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
