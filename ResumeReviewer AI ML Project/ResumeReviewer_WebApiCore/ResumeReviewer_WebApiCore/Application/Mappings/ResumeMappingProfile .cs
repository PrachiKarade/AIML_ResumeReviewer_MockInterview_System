using ResumeReviewer_WebApiCore.Domain.Entities;
using ResumeReviewer_WebApiCore.Domain.Models;
using AutoMapper;

namespace ResumeReviewer_WebApiCore.Application.Mappings
{
    public class ResumeMappingProfile : Profile
    {
        public ResumeMappingProfile()
        {
            CreateMap<ResumeEntity, ResumeModel>().ReverseMap();
            CreateMap<ResumeChunkEntity, ResumeChunkModel>().ReverseMap();
        }
    }
}
