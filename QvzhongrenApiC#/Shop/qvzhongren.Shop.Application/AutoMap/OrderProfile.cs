using AutoMapper;
using qvzhongren.Shop.Application.Dtos;
using qvzhongren.Shop.Model;

namespace qvzhongren.Shop.Application.AutoMap
{
    /// <summary>
    /// 订单映射配置
    /// </summary>
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<ShopOrder, OrderResponseDto>().ReverseMap();
            CreateMap<ShopOrderItem, OrderItemDto>().ReverseMap();
            CreateMap<OrderCreateDto, ShopOrder>();
            CreateMap<OrderCreateItemDto, ShopOrderItem>();
        }
    }
}
