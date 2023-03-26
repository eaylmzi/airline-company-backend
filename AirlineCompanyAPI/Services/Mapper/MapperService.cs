using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Companies.dto;
using airlinecompany.Data.Models.dto.Credentials.dto;
using airlinecompany.Data.Models.dto.FlightAttendant.dto;
using airlinecompany.Data.Models.dto.Passengers.dto;
using airlinecompany.Data.Models.dto.Planes.dto;
using AirlineCompanyAPI.Services.Jwt;
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

            CreateMap<PassengerDto, Passenger>();
            CreateMap<Passenger, PassengerDto>();

        }
    }
}
