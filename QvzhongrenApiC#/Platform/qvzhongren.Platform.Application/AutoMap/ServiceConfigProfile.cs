using AutoMapper;
using qvzhongren.Platform.Application.Dtos;
using qvzhongren.Platform.Model;

namespace qvzhongren.Platform.Application.AutoMap;

public class ServiceConfigProfile : Profile
{
    public ServiceConfigProfile()
    {
        CreateMap<SysServiceConfig, ServiceConfigResponseDto>();
        CreateMap<ServiceConfigResponseDto, SysServiceConfig>();
        CreateMap<ServiceConfigCreateDto, SysServiceConfig>();
    }
}
