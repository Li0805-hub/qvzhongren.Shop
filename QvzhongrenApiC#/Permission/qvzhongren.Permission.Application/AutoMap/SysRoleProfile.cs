using AutoMapper;
using qvzhongren.Permission.Application.Dtos;
using qvzhongren.Permission.Model;

namespace qvzhongren.Permission.Application.AutoMap;

public class SysRoleProfile : Profile
{
    public SysRoleProfile()
    {
        CreateMap<SysRole, SysRoleResponseDto>().ReverseMap();
        CreateMap<SysRole, SysRoleCreateDto>().ReverseMap();
    }
}
