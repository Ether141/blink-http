﻿using BlinkHttp.Authentication;
using BlinkHttp.Http;
using System.Net;
using System.Text;
using System;
using BlinkHttp.Logging;

namespace BlinkHttp.Handling.Pipeline;

internal class Auth : IMiddleware
{
    public MiddlewareDelegate Next { get; set; }

    private readonly ILogger logger = LoggerFactory.Create<Auth>();
    private readonly IAuthorizer? authorizer;

#pragma warning disable CS8618
    public Auth(IAuthorizer? authorizer)
#pragma warning restore CS8618
    {
        this.authorizer = authorizer;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Route!.Endpoint.IsSecure)
        {
            if (authorizer == null)
            {
                logger.Warning("Endpoint is secure (marked with [Authorize] attribute), but authorization is turned off on server.");
            }
            else
            {
                AuthorizationResult authorizationResult = authorizer.Authorize(context.Request, context.Route!.Endpoint.AuthenticationRules);

                if (!authorizationResult.Authorized)
                {
                    logger.Debug($"This endpoint is secure and requires authorization, but client did not provide required credentials. Reason: {authorizationResult.Message}");
                    context.Response.StatusCode = (int)authorizationResult.HttpCode;
                    context.Buffer = Encoding.UTF8.GetBytes(authorizationResult.Message);
                    return;
                }

                context.User = authorizationResult.User;
            } 
        }

        await Next(context);
    }
}
