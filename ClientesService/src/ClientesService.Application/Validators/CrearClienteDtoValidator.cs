using ClientesService.Application.DTOs;
using FluentValidation;

namespace ClientesService.Application.Validators;

/// <summary>Validación de entrada (capa de aplicación), separada de las reglas de negocio del dominio.</summary>
public class CrearClienteDtoValidator : AbstractValidator<CrearClienteDto>
{
    public CrearClienteDtoValidator()
    {
        RuleFor(x => x.ClienteId).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Genero).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Edad).InclusiveBetween(0, 120);
        RuleFor(x => x.Identificacion).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Direccion).NotEmpty().MaximumLength(250);
        RuleFor(x => x.Telefono).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Contrasena).NotEmpty().MinimumLength(6);
    }
}
