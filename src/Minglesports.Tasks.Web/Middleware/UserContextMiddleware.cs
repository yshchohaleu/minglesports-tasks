using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Web.Services;

namespace Minglesports.Tasks.Web.Middleware
{
    internal class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IUserContextProvider userContextProvider)
        {
            UserContextProvider userContextProviderImp = userContextProvider as UserContextProvider;

            if (userContextProviderImp == null)
            {
                throw new ApplicationException(
                    "This middleware requires `UserContextProvider` to implement `IUserContextProvider` interface");
            }

            var userId = GetUserId(httpContext.Request.Headers);
            userContextProviderImp.Initialize(new CurrentUserContext(new User(userId)));

            await _next(httpContext);
        }

        private static UserId GetUserId(IHeaderDictionary headers)
        {
            if (!headers.ContainsKey(ApiConstants.HttpHeaders.UserIdHeader))
                throw new InvalidOperationException($"{ApiConstants.HttpHeaders.UserIdHeader} is missing from the request headers.");

            string userIdAsString = headers[ApiConstants.HttpHeaders.UserIdHeader];
            return UserId.Define(userIdAsString);
        }
    }
}