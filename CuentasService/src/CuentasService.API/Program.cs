using CuentasService.API.Middleware;
using CuentasService.Application.Interfaces;
using CuentasService.Application.Services;
using CuentasService.Application.Validators;
using CuentasService.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration).WriteTo.Console());

builder.Services.AddScoped<ICuentaAppService, CuentaAppService>();
builder.Services.AddScoped<IMovimientoAppService, MovimientoAppService>();
builder.Services.AddScoped<IReporteAppService, ReporteAppService>();

builder.Services.AddValidatorsFromAssemblyContaining<CrearCuentaDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CuentasService API",
        Version = "v1",
        Description = "Microservicio de Cuenta/Movimientos - Prueba técnica de arquitectura de microservicios"
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CuentasService API v1"));
}

app.UseSerilogRequestLogging();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();

public partial class Program { }
