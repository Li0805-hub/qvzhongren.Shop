using AutoMapper;
using qvzhongren.Permission.Application.Dtos;
using qvzhongren.Permission.Model;

namespace qvzhongren.Permission.Application.AutoMap;

public class SysMenuProfile : Profile
{
    public SysMenuProfile()
    {
        CreateMap<SysMenu, SysMenuResponseDto>().ReverseMap();
        CreateMap<SysMenu, SysMenuCreateDto>().ReverseMap();
    }
}
