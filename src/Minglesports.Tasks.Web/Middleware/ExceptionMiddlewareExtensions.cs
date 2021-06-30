using System;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Minglesports.Tasks.BuildingBlocks.Exceptions;
using Minglesports.Tasks.Core.Exceptions;
using Minglesports.Tasks.Web.Models;
using Minglesports.Tasks.Web.Services;
using Newtonsoft.Json;

namespace Minglesports.Tasks.Web.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Use(async (ctx, next) =>
                {
                    var feature = ctx.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = feature.Error;

                    var (statusCode, httpCode) = exception switch
                    {
                        NotFoundException _ => (ApiConstants.ErrorCodes.NotFound, HttpStatusCode.NotFound),
                        ConcurrencyException _ => (ApiConstants.ErrorCodes.Conflict, HttpStatusCode.Conflict),
                        _ => (ApiConstants.ErrorCodes.InternalError, HttpStatusCode.InternalServerError)
                    };

                    var message = new ResultModel();
                    message.AddError(statusCode, exception.Message);
                    
                    ctx.Response.ContentType = MediaTypeNames.Application.Json;
                    ctx.Response.StatusCode = (int) httpCode;
                    await ctx.Response.WriteAsync(JsonConvert.SerializeObject(message));
                });
            });
        }
    }
}