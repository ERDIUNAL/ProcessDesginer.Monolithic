using Application.Services.Core.AuthService;
using Crea.Core.Application.Pipelines.Authorization;
using Crea.Core.Application.Pipelines.Caching;
using Crea.Core.Application.Pipelines.Logging;
using Crea.Core.Application.Pipelines.Performance;
using Crea.Core.Application.Pipelines.Transaction;
using Crea.Core.Application.Pipelines.Validation;
using Crea.Core.Application.Rules;
using Crea.Core.CrossCuttingConcerns.Logging.SeriLog;
using Crea.Core.CrossCuttingConcerns.Logging.SeriLog.Loggers;
using Crea.Core.Mailing;
using Crea.Core.Mailing.MailKit;
using Crea.Core.Security.Authenticator.Email;
using Crea.Core.Security.Authenticator.Otp;
using Crea.Core.Security.JWT;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionScopeBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheRemovingBehavior<,>));
        services.AddTransient<Stopwatch>();

        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

        services.AddSingleton<LoggerServiceBase, FileLogger>();
        //services.AddSingleton<LoggerServiceBase, MsSqlLogger>();

        services.AddScoped<ITokenHelper, JwtHelper>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        services.AddScoped<IMailService, MailKitMailService>();
        services.AddScoped<IOtpAuthenticatorHelper, OtpAuthenticatorHelper>();

        return services;
    }

    public static IServiceCollection AddSubClassesOfType(
        this IServiceCollection services,
        Assembly assembly,
        Type type,
        Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (var item in types)
        {
            if (addWithLifeCycle == null)
            {
                services.AddScoped(item);
            }
            else
            {
                addWithLifeCycle(services, type);
            }
        }
        return services;
    }
}
