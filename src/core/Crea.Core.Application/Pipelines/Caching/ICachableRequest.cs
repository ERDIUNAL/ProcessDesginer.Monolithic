using Crea.Core.Application.Requests;

namespace Crea.Core.Application.Pipelines.Caching;

public interface ICachableRequest
{
    bool BypassCache { get; }
    string CacheKey { get; }
    PageRequest PageRequest { get; }
    TimeSpan? SlidingExpiration { get; }
}