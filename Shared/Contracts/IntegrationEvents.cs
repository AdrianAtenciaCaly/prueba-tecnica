namespace Shared.Contracts;

/// <summary>
/// Contratos de eventos de integración publicados/consumidos de forma asíncrona (RabbitMQ vía MassTransit)
/// entre ClientesService y CuentasService. Viven en un proyecto compartido, versionado, para desacoplar
/// a los consumidores del modelo interno de cada microservicio (cada servicio sigue siendo dueño de su propio dominio).
/// </summary>

/// <summary>Se publica cuando se crea un nuevo cliente en ClientesService.</summary>
public record ClienteCreadoIntegrationEvent(
    string ClienteId,
    string Nombre,
    bool Estado,
    DateTime FechaEvento);

/// <summary>Se publica cuando cambian los datos relevantes (nombre/estado) de un cliente.</summary>
public record ClienteActualizadoIntegrationEvent(
    string ClienteId,
    string Nombre,
    bool Estado,
    DateTime FechaEvento);

/// <summary>Se publica cuando se elimina un cliente, para que CuentasService pueda reaccionar (p. ej. bloquear cuentas).</summary>
public record ClienteEliminadoIntegrationEvent(
    string ClienteId,
    DateTime FechaEvento);
