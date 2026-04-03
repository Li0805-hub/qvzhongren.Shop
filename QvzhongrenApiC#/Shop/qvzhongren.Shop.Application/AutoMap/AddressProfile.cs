using AutoMapper;
using qvzhongren.Shop.Application.Dtos;
using qvzhongren.Shop.Model;

namespace qvzhongren.Shop.Application.AutoMap
{
    /// <summary>
    /// 收货地址映射配置
    /// </summary>
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<ShopAddress, AddressResponseDto>().ReverseMap();
            CreateMap<AddressCreateDto, ShopAddress>();
        }
    }
}
