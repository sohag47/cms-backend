using AutoMapper;
using cms_backend.DTOs.Posts;
using cms_backend.DTOs.Users;
using cms_backend.Models;

namespace cms_backend.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<User, UserResponseDto>();
            //CreateMap<PostUpdateDto, Post>();
            CreateMap<CategoryCreateDto, Post>();
            CreateMap<Post, CategoryResponseDto>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author));
        }
    }
}
