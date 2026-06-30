using AspectCore.DynamicProxy;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.InMemory.Attributes;
using Newtonsoft.Json;

namespace Nebula.Caching.InMemory.Interceptors;

public sealed class InMemoryCacheInterceptorAttribute(IContextUtils utils, ICacheManager cacheManager, IKeyManager keyManager)
    : AbstractInterceptorAttribute
{
    private AspectContext context { get; set; }
    private AspectDelegate next { get; set; }


    public override Task Invoke(AspectContext context, AspectDelegate next)
    {
        this.context = context;
        this.next = next;
        if (ExecutedMethodHasInMemoryCacheAttribute()) return ExecuteMethodThatHasInMemoryCacheAttribute();
        else return ContinueExecutionForNonCacheableMethod();
    }

    private bool ExecutedMethodHasInMemoryCacheAttribute()
    {
        return utils.IsAttributeOfType<InMemoryCacheAttribute>(context);
    }

    private async Task ExecuteMethodThatHasInMemoryCacheAttribute()
    {
        if (await CacheExistsAsync().ConfigureAwait(false))
        {
            await ReturnCachedValueAsync().ConfigureAwait(false);
        }
        else
        {
            await next(context).ConfigureAwait(false);
            await CacheValueAsync().ConfigureAwait(false);
        }
    }

    private Task ContinueExecutionForNonCacheableMethod()
    {
        return next(context);
    }

    private async Task ReturnCachedValueAsync()
    {
        string value = await cacheManager.GetAsync(GenerateKey()).ConfigureAwait(false);

        if (context.IsAsync())
        {
            dynamic objectType = context.ServiceMethod.ReturnType.GetGenericArguments()[0];
            dynamic deserializedObject = JsonConvert.DeserializeObject(value, objectType);
            context.ReturnValue = Task.FromResult(deserializedObject);
        }
        else
        {
            var objectType = context.ServiceMethod.ReturnType;
            context.ReturnValue = JsonConvert.DeserializeObject(value, objectType);
        }
    }

    private async Task CacheValueAsync()
    {
        string value = "";

        if (context.IsAsync())
        {
            value = JsonConvert.SerializeObject(await context.UnwrapAsyncReturnValue().ConfigureAwait(false));
        }
        else
        {
            object? returnValue = context.ReturnValue;
            value = JsonConvert.SerializeObject(returnValue);
        }

        string key = GenerateKey();
        var expiration = TimeSpan.FromSeconds(utils.GetCacheDuration<InMemoryCacheAttribute>(key, context));

        await cacheManager.SetAsync(key, value, expiration).ConfigureAwait(false);
    }

    private Task<bool> CacheExistsAsync()
    {
        return cacheManager.CacheExistsAsync(GenerateKey());
    }

    private string GenerateKey()
    {
        return keyManager.GenerateKey(utils.GetExecutedMethodInfo(context), utils.GetServiceMethodInfo(context), utils.GetMethodParameters(context));
    }
}
