using ClientesService.API.Middleware;
using ClientesService.Application.Interfaces;
using ClientesService.Application.Services;
using ClientesService.Application.Validators;
using ClientesService.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging estructurado (buena práctica de observabilidad)
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration).WriteTo.Console());

// Application layer (Servicios de caso de uso)
builder.Services.AddScoped<IClienteAppService, ClienteAppService>();

// Validación de entrada
builder.Services.AddValidatorsFromAssemblyContaining<CrearClienteDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Infrastructure (EF Core + PostgreSQL + MassTransit/RabbitMQ + Repository + Hasher)
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ClientesService API",
        Version = "v1",
        Description = "Microservicio de Cliente/Persona - Prueba técnica de arquitectura de microservicios"
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment() || true) // Swagger habilitado también fuera de Development para facilitar la evaluación
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClientesService API v1"));
}

app.UseSerilogRequestLogging();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();

// Necesario para que WebApplicationFactory<Program> funcione en las pruebas de integración
public partial class Program { }
