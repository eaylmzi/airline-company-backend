using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Companies.dto;
using airlinecompany.Data.Models.dto.Credentials.dto;
using airlinecompany.Data.Models.dto.Planes.dto;
using airlinecompany.Logic.Logics.Companies;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AirlineCompanyAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompanyController : Controller
    {
        Company emptyObject = new Company();
        private readonly IMapper _mapper;
        private readonly IPassengerLogic _passengerLogic;
        private readonly IPlaneLogic _planeLogic;
        private readonly ICompanyLogic _companyLogic;


        public CompanyController(IMapper mapper, IPassengerLogic passengerLogic, IPlaneLogic planeLogic, ICompanyLogic companyLogic)
        {
            _mapper = mapper;
            _passengerLogic = passengerLogic;
            _planeLogic = planeLogic;
            _companyLogic = companyLogic;


        }
        [HttpPost]
        public ActionResult<int> Add([FromBody] CompanyDto companyDto)
        {
            try
            {
                Company company = _mapper.Map<Company>(companyDto);
                company.TotalMoney = 0;
                return _companyLogic.Add(company);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<bool> Delete([FromBody] IdDto idDto)
        {
            try
            {
                return _companyLogic.Delete(idDto.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<Company> Get([FromBody] IdDto idDto)
        {
            try
            {
                Company? company = _companyLogic.GetSingle(idDto.Id);
                if (company != null)
                {
                    return Ok(company);
                }
                return Ok(emptyObject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Company>> Update([FromBody] IdNameDto updatedEntity)
        {
            try
            {
                Company? company = _companyLogic.GetSingle(updatedEntity.Id);
                if(company != null)
                {
                    Company newCompany = _mapper.Map<Company>(updatedEntity);
                    Company? updatedCompany = await _companyLogic.UpdateAsync(updatedEntity.Id, newCompany);
                    if (updatedCompany != null)
                    {
                        return Ok(updatedCompany);
                    }
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
