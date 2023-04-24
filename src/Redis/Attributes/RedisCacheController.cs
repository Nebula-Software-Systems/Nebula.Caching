using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Newtonsoft.Json;

namespace Nebula.Caching.Redis.Attributes
{
    public class RedisCacheController : ActionFilterAttribute
    {

        public RedisCacheController()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerFullPathName = $"{context.Controller.ToString()}.{(string)context.RouteData.Values["action"]}";
            var keyManager = context.HttpContext.RequestServices.GetService<IKeyManager>();
            var cacheManager = context.HttpContext.RequestServices.GetService<ICacheManager>();
            var cacheKey = keyManager.ConvertCacheKeyToConfigKey(controllerFullPathName);

            if (cacheManager.CacheExists(controllerFullPathName))
            {
                var value = cacheManager.Get(controllerFullPathName);
                Type controllerType = context.Controller.GetType();
                MethodInfo actionMethodInfo = default(MethodInfo);
                //dynamic objectType = controllerType.GetMethod((string)context.RouteData.Values["action"]).ReturnType;
                // var list = JsonConvert.DeserializeObject<Task<IActionResult>>(value);
                // context.Result = await list;
                var val = JsonConvert.DeserializeObject<string>(value);
                context.Result = new Microsoft.AspNetCore.Mvc.OkObjectResult(val);
            }
            else
            {
                var actionExecutedContext = await next();
                var value = JsonConvert.SerializeObject(actionExecutedContext.Result);
                cacheManager.Set(controllerFullPathName, value, TimeSpan.FromHours(2));
            }
        }
    }
}