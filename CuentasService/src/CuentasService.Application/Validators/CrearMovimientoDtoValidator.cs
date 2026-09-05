using CuentasService.Application.DTOs;
using FluentValidation;

namespace CuentasService.Application.Validators;

public class CrearMovimientoDtoValidator : AbstractValidator<CrearMovimientoDto>
{
    public CrearMovimientoDtoValidator()
    {
        RuleFor(x => x.CuentaId).NotEmpty();
        RuleFor(x => x.TipoMovimiento).NotEmpty().Must(t => t.Equals("Deposito", StringComparison.OrdinalIgnoreCase)
            || t.Equals("Retiro", StringComparison.OrdinalIgnoreCase))
            .WithMessage("TipoMovimiento debe ser 'Deposito' o 'Retiro'.");
        RuleFor(x => x.Valor).NotEqual(0).WithMessage("El valor del movimiento no puede ser cero.");
    }
}
