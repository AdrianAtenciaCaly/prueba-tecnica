using ClientesService.Application.DTOs;
using FluentValidation;

namespace ClientesService.Application.Validators;

public class ActualizarClienteDtoValidator : AbstractValidator<ActualizarClienteDto>
{
    public ActualizarClienteDtoValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Genero).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Edad).InclusiveBetween(0, 120);
        RuleFor(x => x.Direccion).NotEmpty().MaximumLength(250);
        RuleFor(x => x.Telefono).NotEmpty().MaximumLength(20);
    }
}
