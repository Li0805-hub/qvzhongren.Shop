using AutoMapper;
using qvzhongren.Permission.Application.Dtos;
using qvzhongren.Permission.Model;

namespace qvzhongren.Permission.Application.AutoMap;

public class SysPermissionProfile : Profile
{
    public SysPermissionProfile()
    {
        CreateMap<SysPermission, SysPermissionResponseDto>().ReverseMap();
        CreateMap<SysPermission, SysPermissionCreateDto>().ReverseMap();
    }
}
