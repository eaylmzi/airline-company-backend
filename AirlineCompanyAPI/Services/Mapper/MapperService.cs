using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Companies.dto;
using airlinecompany.Data.Models.dto.Credentials.dto;
using airlinecompany.Data.Models.dto.FlightAttendant.dto;
using airlinecompany.Data.Models.dto.Planes.dto;
using AutoMapper;

namespace AirlineCompanyAPI.Services.Mapper
{
    public class MapperService : Profile
    {
        public MapperService() 
        {
            CreateMap<IdNameDto, Company>();

            CreateMap<PlaneDto, Plane>();

            CreateMap<CompanyDto, Company>();

            CreateMap<FlightAttendantDto, FlightAttendant>();

        }
    }
}
