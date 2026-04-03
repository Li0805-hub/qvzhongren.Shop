using AutoMapper;
using qvzhongren.Shop.Application.Dtos;
using qvzhongren.Shop.Model;

namespace qvzhongren.Shop.Application.AutoMap
{
    /// <summary>
    /// 分类映射配置
    /// </summary>
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<ShopCategory, CategoryResponseDto>().ReverseMap();
            CreateMap<CategoryCreateDto, ShopCategory>();
        }
    }
}
