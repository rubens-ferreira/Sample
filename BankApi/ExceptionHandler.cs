namespace BankApi;

using Microsoft.AspNetCore.Diagnostics;

public static class ExceptionHandler
{
    public static async Task HandleException(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionFeature?.Error;

        switch (exception)
        {
            case ArgumentException argumentException:
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { error = argumentException.Message });
                break;

            default:
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(
                    new { error = exception?.Message ?? "Unknown error" }
                );
                break;
        }
    }
}
