using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Planes.dto;
using AutoMapper;

namespace AirlineCompanyAPI.Services.Mapper
{
    public class MapperService : Profile
    {
        public MapperService() 
        {
            CreateMap<PlaneDto, Plane>();
        }
    }
}
