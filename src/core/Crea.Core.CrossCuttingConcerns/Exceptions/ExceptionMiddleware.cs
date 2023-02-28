using Crea.Core.CrossCuttingConcerns.Exceptions.Handlers;
using Crea.Core.CrossCuttingConcerns.Logging;
using Crea.Core.CrossCuttingConcerns.Logging.SeriLog;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Crea.Core.CrossCuttingConcerns.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpExeptionHandler _httpExceptionHandler = new();
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly LoggerServiceBase _loggerService;

    public ExceptionMiddleware(RequestDelegate next, IHttpContextAccessor contextAccessor, LoggerServiceBase loggerService)
    {
        _next = next;
        _contextAccessor = contextAccessor;
        _loggerService = loggerService;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await LogException(context, exception);
            await HandleExceptionAsync(context.Response, exception);
        }
    }

    private Task HandleExceptionAsync(HttpResponse response, Exception exception)
    {
        response.ContentType = "application/json";
        _httpExceptionHandler.Response = response;
        return _httpExceptionHandler.HandleExceptionAsync(exception);
    }

    private Task LogException(HttpContext context, Exception exception)
    {
        List<LogParameter> logParameters = new() {
            new LogParameter
            {
                Type = context.GetType().Name,
                Value = context.Request.Method
            }
        };

        LogDetailWithException logDetail = new()
        {
            MethodName = _next.Method.Name,
            Parameters = logParameters,
            User = _contextAccessor.HttpContext?.User.Identity?.Name ?? "?",
            ExceptionMessage = exception.Message
        };

        _loggerService.Error(JsonConvert.SerializeObject(logDetail));
        return Task.CompletedTask;
    }
}
