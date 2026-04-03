using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace qvzhongren.Shop.Application
{
    /// <summary>
    /// 商城应用层模块
    /// </summary>
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(AbpAutoMapperModule)
    )]
    public class ShopApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 配置AutoMapper
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<ShopApplicationModule>();
            });

            // 配置约定控制器
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(ShopApplicationModule).Assembly, opts =>
                {
                    opts.RootPath = "Shop";
                });
            });
        }
    }
}
