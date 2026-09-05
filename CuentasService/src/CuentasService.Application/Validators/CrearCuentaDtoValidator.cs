using CuentasService.Application.DTOs;
using FluentValidation;

namespace CuentasService.Application.Validators;

public class CrearCuentaDtoValidator : AbstractValidator<CrearCuentaDto>
{
    public CrearCuentaDtoValidator()
    {
        RuleFor(x => x.NumeroCuenta).NotEmpty().MaximumLength(20);
        RuleFor(x => x.TipoCuenta).NotEmpty().Must(t => t.Equals("Ahorros", StringComparison.OrdinalIgnoreCase)
            || t.Equals("Corriente", StringComparison.OrdinalIgnoreCase))
            .WithMessage("TipoCuenta debe ser 'Ahorros' o 'Corriente'.");
        RuleFor(x => x.SaldoInicial).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ClienteId).NotEmpty();
    }
}
