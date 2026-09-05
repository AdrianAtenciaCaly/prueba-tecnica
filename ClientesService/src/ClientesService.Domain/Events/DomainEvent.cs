namespace ClientesService.Domain.Events;

/// <summary>Marcador simple para eventos de dominio internos (no confundir con los IntegrationEvents del proyecto Shared).</summary>
public abstract record DomainEvent(DateTime OcurrioEn);
