using AutoMapper;
using qvzhongren.Shop.Application.Dtos;
using qvzhongren.Shop.Model;

namespace qvzhongren.Shop.Application.AutoMap
{
    /// <summary>
    /// 购物车映射配置
    /// </summary>
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<ShopCart, CartResponseDto>().ReverseMap();
            CreateMap<CartAddDto, ShopCart>();
        }
    }
}
