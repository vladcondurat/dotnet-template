using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication.Extensions
{
    public static class Swagger
    {
        public static IServiceCollection AddSwaggerProperties(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TelegramClone.WebApi", Version = "v1" });
                c.AddSecurityDefinition(AuthorizationType.Bearer, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Scheme = AuthorizationType.Bearer,
                    Description = "Please insert the JWT token"
                });
                c.OperationFilter<BasicAuthOperationsFilter>();
                //c.EnableAnnotations();
            });
        }

        /// <summary>
        /// Marks routes annotated with [AllowAnonymous] attribute as being public. All other routes are marked as requiring authentication.
        /// </summary>
        private class BasicAuthOperationsFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var noAuthRequired = context.ApiDescription.CustomAttributes().Any(attr => attr.GetType() == typeof(AllowAnonymousAttribute));

                if (noAuthRequired)
                {
                    return;
                }

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Name = AuthorizationType.Bearer,
                                In = ParameterLocation.Header,
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = AuthorizationType.Bearer
                                },
                            },
                            new string[] { }
                        }
                    }
                };
            }
        }

        private class AuthorizationType
        {
            public const string Basic = "basic";
            public const string Bearer = "Bearer";
        }
    }
}