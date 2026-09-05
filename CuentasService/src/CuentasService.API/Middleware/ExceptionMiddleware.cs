using System.Net;
using System.Text.Json;
using CuentasService.Domain.Exceptions;

namespace CuentasService.API.Middleware;

/// <summary>
/// Manejo centralizado de excepciones (requisito del ejercicio). F3 en particular exige que al
/// intentar un movimiento sin saldo se "alerte" con el mensaje "Saldo no disponible": aquí se traduce
/// SaldoNoDisponibleException a un 400 Bad Request con ese mensaje exacto en el cuerpo de la respuesta.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            SaldoNoDisponibleException => (HttpStatusCode.BadRequest, "Saldo no disponible"),
            CuentaNoEncontradaException => (HttpStatusCode.NotFound, "Recurso no encontrado"),
            CuentaDuplicadaException => (HttpStatusCode.Conflict, "Conflicto de datos"),
            ClienteInvalidoException => (HttpStatusCode.UnprocessableEntity, "Cliente inválido"),
            CuentaDomainException => (HttpStatusCode.BadRequest, "Solicitud inválida"),
            _ => (HttpStatusCode.InternalServerError, "Error interno del servidor")
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Error no controlado procesando {Path}", context.Request.Path);
        else
            _logger.LogWarning("{Title}: {Message}", title, exception.Message);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problemDetails = new
        {
            type = $"https://httpstatuses.io/{(int)statusCode}",
            title,
            status = (int)statusCode,
            detail = exception.Message,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}
