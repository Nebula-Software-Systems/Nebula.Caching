using AspectCore.DynamicProxy;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Common.Utils;
using Newtonsoft.Json;

namespace Nebula.Caching.Common.Interceptor;

public sealed class NebulaCacheInterceptor(
    ICacheManager cacheManager,
    IKeyManager keyManager,
    IContextUtils utils)
    : AbstractInterceptorAttribute
{
    public override Task Invoke(AspectContext context, AspectDelegate next)
    {
        if (ExecutedMethodHasNebulaCacheAttribute(context)) return ExecuteMethodThatHasNebulaCacheAttribute(context, next);
        return ContinueExecutionForNonCacheableMethod(context, next);
    }

    private bool ExecutedMethodHasNebulaCacheAttribute(AspectContext context)
    {
        return utils.IsAttributeOfType(context);
    }

    private async Task ExecuteMethodThatHasNebulaCacheAttribute(AspectContext context, AspectDelegate next)
    {
        if (await CacheExistsAsync(context).ConfigureAwait(false))
        {
            await ReturnCachedValueAsync(context).ConfigureAwait(false);
        }
        else
        {
            await next(context).ConfigureAwait(false);
            await CacheValueAsync(context).ConfigureAwait(false);
        }
    }

    private static Task ContinueExecutionForNonCacheableMethod(AspectContext context, AspectDelegate next)
    {
        return next(context);
    }

    private async Task ReturnCachedValueAsync(AspectContext context)
    {
        string value = await cacheManager.GetAsync(GenerateKey(context)).ConfigureAwait(false);

        if (context.IsAsync())
        {
            dynamic objectType = context.ServiceMethod.ReturnType.GetGenericArguments()[0];
            dynamic deserializedObject = JsonConvert.DeserializeObject(value, objectType);
            context.ReturnValue = Task.FromResult(deserializedObject);
        }
        else
        {
            Type objectType = context.ServiceMethod.ReturnType;
            context.ReturnValue = JsonConvert.DeserializeObject(value, objectType);
        }
    }

    private async Task CacheValueAsync(AspectContext context)
    {
        string value = string.Empty;

        if (context.IsAsync())
        {
            value = JsonConvert.SerializeObject(await context.UnwrapAsyncReturnValue().ConfigureAwait(false));
        }
        else
        {
            object? returnValue = context.ReturnValue;
            value = JsonConvert.SerializeObject(returnValue);
        }

        string key = GenerateKey(context);

        TimeSpan expiration = TimeSpan.FromSeconds(utils.GetCacheDuration(key, context));

        await cacheManager.SetAsync(key, value, expiration).ConfigureAwait(false);
    }

    private Task<bool> CacheExistsAsync(AspectContext context)
    {
        return cacheManager.CacheExistsAsync(GenerateKey(context));
    }

    private string GenerateKey(AspectContext context)
    {
        return keyManager.GenerateKey(utils.GetExecutedMethodInfo(context), utils.GetServiceMethodInfo(context), utils.GetMethodParameters(context));
    }
}
