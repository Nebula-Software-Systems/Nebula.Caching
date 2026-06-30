using AspectCore.DynamicProxy;
using Nebula.Caching.Common.Attributes;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Common.Utils;
using Newtonsoft.Json;

namespace Nebula.Caching.Common.Interceptor;

public abstract class BaseCacheInterceptorAttribute<CacheAttribute>(
    IContextUtils utils,
    ICacheManager cacheManager,
    IKeyManager keyManager)
    : AbstractInterceptorAttribute
    where CacheAttribute : BaseAttribute
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
        if (ExecutedMethodHasMemCachedAttribute()) return ExecuteMethodThatHasMemCachedAttribute();
        else return ContinueExecutionForNonCacheableMethod();
    }

    private bool ExecutedMethodHasMemCachedAttribute()
    {
        return _utils.IsAttributeOfType<CacheAttribute>(context);
    }

    private async Task ExecuteMethodThatHasMemCachedAttribute()
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

        string key = GenerateKey();
        var expiration = TimeSpan.FromSeconds(_utils.GetCacheDuration<CacheAttribute>(key, context));

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
