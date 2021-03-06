using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Minglesports.Tasks.BuildingBlocks.Exceptions;
using Minglesports.Tasks.Core.Exceptions;
using Minglesports.Tasks.Web.Models.Result;
using Minglesports.Tasks.Web.Services;
using Newtonsoft.Json;

namespace Minglesports.Tasks.Web.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseUserContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserContextMiddleware>();
        }

        public static IApplicationBuilder ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Use((ctx, next) =>
                {
                    var feature = ctx.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = feature.Error;

                    var (statusCode, httpCode) = exception switch
                    {
                        NotFoundException _ => (ApiConstants.ErrorCodes.NotFound, HttpStatusCode.UnprocessableEntity),
                        ArgumentException _ => (ApiConstants.ErrorCodes.BadRequest, HttpStatusCode.BadRequest),
                        ConcurrencyException _ => (ApiConstants.ErrorCodes.Conflict, HttpStatusCode.Conflict),
                        _ => (ApiConstants.ErrorCodes.InternalError, HttpStatusCode.InternalServerError)
                    };

                    var message = new ResultModel();
                    message.AddError(statusCode, exception.Message);

                    ctx.Response.ContentType = MediaTypeNames.Application.Json;
                    ctx.Response.StatusCode = (int) httpCode;
                    return ctx.Response.WriteAsync(JsonConvert.SerializeObject(message));
                });
            });

            return app;
        }
    }
}