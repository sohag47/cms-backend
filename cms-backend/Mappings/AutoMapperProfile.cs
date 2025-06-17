using AutoMapper;
using cms_backend.DTOs.Posts;
using cms_backend.Models;

namespace cms_backend.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<PostCreateDto, Post>();
            CreateMap<PostUpdateDto, Post>();
            CreateMap<Post, PostResponseDto>()
                .ForMember(dest => dest.AuthorUsername, opt => opt.MapFrom(src => src.Author.Username));
        }
    }
}
