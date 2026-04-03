using AutoMapper;
using qvzhongren.Permission.Application.Dtos;
using qvzhongren.Permission.Model;

namespace qvzhongren.Permission.Application.AutoMap;

public class SysUserProfile : Profile
{
    public SysUserProfile()
    {
        CreateMap<SysUser, SysUserResponseDto>().ReverseMap();
        CreateMap<SysUser, SysUserCreateDto>().ReverseMap();
    }
}
