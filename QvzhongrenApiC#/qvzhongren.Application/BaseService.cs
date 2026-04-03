using Autofac.Extras.DynamicProxy;
using qvzhongren.Application.Interceptors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace qvzhongren.Application
{
    [ApiController]
    //[Authorization]
    [Intercept(typeof(LoggingInterceptor))]
    [Route("api/[controller]")]
    public class BaseService : ApplicationService
    {

    }
}
