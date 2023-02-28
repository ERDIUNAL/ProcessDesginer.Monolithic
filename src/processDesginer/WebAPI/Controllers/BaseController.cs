﻿using Crea.Core.Security.Entities;
using Crea.Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected string getIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"].ToString();
            }

            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        protected string? getRefreshTokenFromCookie()
        {
            return Request.Cookies["refreshToken"];
        }

        protected void setRefreshTokenToCookie(RefreshToken refreshToken)
        {
            Response.Cookies.Append(key: "refreshToken",
                                    refreshToken.Token,
                                    options: new CookieOptions
                                    {
                                        HttpOnly = true,
                                        Expires = refreshToken.ExpiresDate,
                                        Secure = true,
                                        SameSite = SameSiteMode.Strict
                                    });
        }

        protected int getUserIdFromToken()
        {
            return HttpContext.User.ClaimsNameIdentifier();

        }
    }
}
