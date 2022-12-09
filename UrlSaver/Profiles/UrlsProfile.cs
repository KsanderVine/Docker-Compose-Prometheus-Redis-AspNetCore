using AutoMapper;
using UrlSaver.Models;
using UrlSaver.Dtos;

namespace UrlSaver.Profiles
{
    public class UrlsProfile : Profile
    {
        public UrlsProfile()
        {
            CreateMap<Url, UrlReadDto>();
            CreateMap<UrlCreateDto, Url>();
        }
    }
}
