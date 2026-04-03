using AutoMapper;
using qvzhongren.Shop.Application.Dtos;
using qvzhongren.Shop.Model;

namespace qvzhongren.Shop.Application.AutoMap
{
    /// <summary>
    /// 商品评价映射配置
    /// </summary>
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<ShopReview, ReviewResponseDto>().ReverseMap();
            CreateMap<ReviewCreateDto, ShopReview>();
        }
    }
}
