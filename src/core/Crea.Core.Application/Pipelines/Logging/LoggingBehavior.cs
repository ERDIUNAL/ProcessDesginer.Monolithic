using Crea.Core.CrossCuttingConcerns.Logging;
using Crea.Core.CrossCuttingConcerns.Logging.SeriLog;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Crea.Core.Application.Pipelines.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ILoggableRequest
{
    private readonly LoggerServiceBase _loggerServiceBase;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingBehavior(LoggerServiceBase loggerServiceBase, IHttpContextAccessor httpContextAccessor)
    {
        _loggerServiceBase = loggerServiceBase;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        List<LogParameter> logParameters = new();
        logParameters.Add(new LogParameter
        {
            Type = request.GetType().Name,
            Name = request.GetType().FullName,
            Value = request
        });

        LogDetail logDetail = new()
        {
            MethodName = next.Method.Name,
            FullName = next.GetType().FullName,
            Parameters = logParameters,
            User = _httpContextAccessor.HttpContext == null ||
                   _httpContextAccessor.HttpContext.User.Identity.Name == null
                       ? "?"
                       : _httpContextAccessor.HttpContext.User.Identity.Name
        };

        _loggerServiceBase.Info(JsonConvert.SerializeObject(logDetail));

        return next();
    }
}
