using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Nebula.Caching.Redis.Attributes
{
    public class RedisCacheController : ActionFilterAttribute
    {
        public RedisCacheController()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionExecutedContext = await next();
            var controllerFullPathName = $"{actionExecutedContext.Controller.ToString()}.{(string)context.RouteData.Values["action"]}";
            // var cacheInvalidator = actionExecutedContext.HttpContext.RequestServices.GetService<ICacheInvalidator>();
            // await cacheInvalidator.InvalidateCacheAsync(_keysToInvalidate);
        }
    }
}