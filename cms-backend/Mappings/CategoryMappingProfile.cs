using AutoMapper;
using cms_backend.DTOs.Categories;
using cms_backend.Models;

namespace cms_backend.Mappings;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<Category, CategoryCreateDto>();

        CreateMap<Category, CategoryResponseDto>();
    }
}

