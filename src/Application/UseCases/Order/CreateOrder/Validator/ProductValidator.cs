using App.InvoiSysTest.Application.UseCases.Order.CreateOrder.Input;
using FluentValidation;

namespace App.InvoiSysTest.Application.UseCases.Order.CreateOrder.Validator;

public class ProductValidator : AbstractValidator<ProductInput>
{
    public ProductValidator()
    {
        RuleFor(p => p.ProductCode)
           .NotEmpty().WithMessage("O código do produto é obrigatório.");

        RuleFor(p => p.ProductDescription)
           .NotEmpty().WithMessage("A descrição do produto é obrigatória.");

        RuleFor(p => p.Quantity)
           .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");

        RuleFor(p => p.ProductPrice)
           .GreaterThan(0).WithMessage("O preço do produto deve ser maior que zero.");
    }
}