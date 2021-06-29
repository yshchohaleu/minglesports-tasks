using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Minglesports.Tasks.Web.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Minglesports.Tasks.Web
{
    internal static class SwaggerConfiguration
    {
        internal static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "Minglesports.Tasks.Web", Version = "v1" });
                swaggerOptions.OperationFilter<MandatoryHeadersOperationFilter>();
            });
        }

        private class MandatoryHeadersOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                operation.Parameters ??= new List<OpenApiParameter>();
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = ApiConstants.HttpHeaders.UserIdHeader,
                    In = ParameterLocation.Header,
                    Description = "User Identifier",
                    Required = true
                });
            }
        }
    }
}