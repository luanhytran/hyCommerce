using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace hyCommerce.Extensions.Handlers.ErrorHandler;

public static class GlobalExceptionConfig
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                var error = exceptionFeature?.Error;

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error",
                    Detail = error?.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            });
        });

        return app;
    }
}