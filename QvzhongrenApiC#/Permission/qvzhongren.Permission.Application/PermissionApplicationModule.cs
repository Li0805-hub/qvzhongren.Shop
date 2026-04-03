using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace qvzhongren.Permission.Application;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpEmailingModule),
    typeof(AbpAutoMapperModule)
)]
public class PermissionApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<PermissionApplicationModule>();
        });

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers
                .Create(typeof(PermissionApplicationModule).Assembly, opts =>
                {
                    opts.RootPath = "Permission";
                    opts.RemoteServiceName = "His";
                });
        });
    }
}
