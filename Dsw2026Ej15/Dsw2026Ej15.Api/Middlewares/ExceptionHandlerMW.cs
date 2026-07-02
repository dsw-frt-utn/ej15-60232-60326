using Dsw2026Ej15.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Dsw2026Ej15.Api.Middlewares;

public class ExceptionHandlerMW
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMW(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        HttpStatusCode status = HttpStatusCode.InternalServerError; // larga un 500 por default para los errores no controlados
        string message = "Ocurrió un error inesperado al ejecutar la solicitud";

        if (ex is ValidationException ve)
        {
            status = HttpStatusCode.BadRequest; // 400
            message = ve.Message;
        }
        else if (ex is EntityNotFoundException enf)
        {
            status = HttpStatusCode.NotFound;
            message = enf.Message;
        }


        var result = JsonSerializer.Serialize(new { message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        await context.Response.WriteAsync(result);
    }
}

