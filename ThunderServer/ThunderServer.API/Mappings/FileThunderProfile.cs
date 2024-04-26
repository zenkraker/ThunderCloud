using AutoMapper;
using ThunderServer.API.DTOs;
using ThunderServer.Models.Domain;

namespace ThunderServer.API.Mappings
{
    public class FileThunderProfile : Profile
    {
        protected FileThunderProfile()
        {
            CreateMap<ThunderFile, ThunderFileDto>().ReverseMap();
        }
    }
}
