using AspectCore.DynamicProxy;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Redis.Attributes;
using Newtonsoft.Json;

namespace Nebula.Caching.Redis.Interceptors;

public class RedisCacheInterceptorAttribute(ICacheManager cacheManager, IKeyManager keyManager, IContextUtils utils)
    : AbstractInterceptorAttribute
{
    private ICacheManager _cacheManager { get; set; } = cacheManager;
    private IKeyManager _keyManager { get; set; } = keyManager;
    private IContextUtils _utils { get; set; } = utils;
    private AspectContext context { get; set; }
    private AspectDelegate next { get; set; }


    public override Task Invoke(AspectContext context, AspectDelegate next)
    {
        this.context = context;
        this.next = next;
        if (ExecutedMethodHasRedisCacheAttribute()) return ExecuteMethodThatHasRedisCacheAttribute();
        else return ContinueExecutionForNonCacheableMethod();
    }

    private bool ExecutedMethodHasRedisCacheAttribute()
    {
        return _utils.IsAttributeOfType<RedisCacheAttribute>(context);
    }

    private async Task ExecuteMethodThatHasRedisCacheAttribute()
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
        string value = await _cacheManager.GetAsync(GenerateKey()).ConfigureAwait(false);

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
        var expiration = TimeSpan.FromSeconds(_utils.GetCacheDuration<RedisCacheAttribute>(GenerateKey(), context));

        await _cacheManager.SetAsync(key, value, expiration).ConfigureAwait(false);
    }

    private Task<bool> CacheExistsAsync()
    {
        return _cacheManager.CacheExistsAsync(GenerateKey());
    }

    private string GenerateKey()
    {
        return _keyManager.GenerateKey(_utils.GetExecutedMethodInfo(context), _utils.GetServiceMethodInfo(context), _utils.GetMethodParameters(context));
    }
}
