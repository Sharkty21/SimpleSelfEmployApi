using AutoMapper;
using MongoDB.Bson;
using SimpleSelfEmployApi.Dtos;
using SimpleSelfEmployApi.Models;

namespace SimpleSelfEmployApi.Profiles
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<JobDto, Job>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => MapId(src.Id)));
            CreateMap<Job, JobDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<PaymentDto, Payment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => MapId(src.Id)));
        }

        private ObjectId MapId(string id)
        {
            if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                return objectId;
            }
            else
            {
                return ObjectId.Empty;
            }
        }
    }
}
