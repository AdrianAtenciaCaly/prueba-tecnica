using System.Net;
using System.Text.Json;
using ClientesService.Domain.Exceptions;

namespace ClientesService.API.Middleware;

/// <summary>
/// Middleware transversal de manejo de excepciones (requisito explícito del ejercicio: "Se debe aplicar
/// un manejo de excepciones"). Centraliza la traducción de excepciones de dominio a respuestas HTTP
/// consistentes, evitando try/catch repetido en cada controlador (SRP + DRY).
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
            ClienteNoEncontradoException => (HttpStatusCode.NotFound, "Recurso no encontrado"),
            ClienteIdentificacionDuplicadaException => (HttpStatusCode.Conflict, "Conflicto de datos"),
            ClienteDomainException => (HttpStatusCode.BadRequest, "Solicitud inválida"),
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
