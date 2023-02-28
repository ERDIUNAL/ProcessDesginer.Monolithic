using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Crea.Core.Application.Pipelines.Caching;

public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICacheRemoverRequest
{
    private IDistributedCache _cache;

    public CacheRemovingBehavior(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response;

        if (request.BypassCache)
        {
            return await next();
        }

        async Task<TResponse> GetResponseAndRemoveCache()
        {
            response = await next();

            List<string> listKeys = new List<string>();
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379,allowAdmin=true"))
            {
                var keys = redis.GetServer("localhost", 6379).Keys();
                listKeys.AddRange(keys.Select(key => (string)key).Where(x => x.StartsWith(request.CacheKey)).ToList());
            }

            foreach (var key in listKeys)
            {
                await _cache.RemoveAsync(key, cancellationToken);
            }

            return response;
        }

        response = await GetResponseAndRemoveCache();

        return response;
    }
}
