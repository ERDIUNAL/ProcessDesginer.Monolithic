﻿using Crea.Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Crea.Core.Application.Pipelines.Authorization;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ISecuredOperation
{
    IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var userClaims = _httpContextAccessor.HttpContext.User.Claims;
        if (userClaims == null || userClaims.Count() == 0)
        {
            throw new UnauthorizedAccessException("User is not authorized.");
        }

        string[] requestRoles = request.Roles;
        if (requestRoles.Length == 0)
        {
            return next();
        }

        ICollection<string>? userRoles = _httpContextAccessor.HttpContext.User.ClaimsRoles();
        if (userRoles == null || userRoles.Count == 0)
        {
            throw new UnauthorizedAccessException("Role claims not found.");
        }

        bool isAuthorized = requestRoles.Any(r => userRoles.Contains(r));
        if (!isAuthorized)
        {
            throw new UnauthorizedAccessException("User is not authorized.");
        }

        return next();
    }
}
