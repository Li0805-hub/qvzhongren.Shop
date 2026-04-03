using AutoMapper;
using qvzhongren.Shop.Application.Dtos;
using qvzhongren.Shop.Model;

namespace qvzhongren.Shop.Application.AutoMap
{
    /// <summary>
    /// 商品映射配置
    /// </summary>
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ShopProduct, ProductResponseDto>().ReverseMap();
            CreateMap<ProductCreateDto, ShopProduct>();
        }
    }
}
