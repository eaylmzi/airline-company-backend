using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto;
using airlinecompany.Data.Models.dto.Companies.dto;
using airlinecompany.Data.Models.dto.Credentials.dto;
using airlinecompany.Data.Models.dto.Planes.dto;
using airlinecompany.Data.Resources.String;
using airlinecompany.Logic.Logics.Companies;
using airlinecompany.Logic.Logics.Passengers;
using airlinecompany.Logic.Logics.Planes;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AirlineCompanyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1")]
    public class CompanyController : Controller
    {
        
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
        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public ActionResult<Response<int>> Add([FromBody] CompanyDto companyDto)
        {
            try
            {
                Company company = _mapper.Map<Company>(companyDto);
                company.TotalMoney = 0;
                int companyId = _companyLogic.Add(company);
                if(companyId != -1)
                {
                    return Ok(new Response<int> { Message = Success.SuccesfullyAddedCompany, Data = companyId });
                }
                return Ok(new Response<int> { Message = Error.NotAddedCompany, Data = companyId });
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
                bool isBusy = _companyLogic.CheckAvailabality(idDto.Id);
                if (!isBusy)
                {
                    bool isDeleted = _companyLogic.Delete(idDto.Id);
                    if (isDeleted)
                    {
                        return Ok(new Response<bool> { Message = Success.SuccesfullyDeletedCompany, Data = isDeleted });
                    }
                    return Ok(new Response<bool> { Message = Error.NotDeletedCompany, Data = isDeleted });
                }
                return Ok(new Response<bool> { Message = Error.NotAvailableCompany, Data = !isBusy });


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public ActionResult<Response<Company>> Get([FromBody] IdDto idDto)
        {
            try
            {
                Company? company = _companyLogic.GetSingle(idDto.Id);
                if (company != null)
                {
                    return Ok(new Response<Company> { Message = Success.SuccesfullyAddedCompany, Data = company});
                }
                return Ok(new Response<Company> { Message = Error.NotFoundCompany, Data = new Company() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Authorize(Roles = $"{Role.SuperAdmin}")]
        public async Task<ActionResult<Response<Company>>> Update([FromBody] IdNameDto updatedEntity)
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
                        return Ok(new Response<Company> { Message = Success.SuccesfullyUpdatedCompany, Data = updatedCompany });
                    }
                    return Ok(new Response<Company> { Message = Error.NotUpdatedCompany, Data = new Company() });
                }
                
                return Ok(new Response<Company> { Message = Error.NotUpdatedCompany, Data = new Company() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
