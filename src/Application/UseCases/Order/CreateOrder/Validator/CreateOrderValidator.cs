using App.InvoiSysTest.Application.UseCases.Order.CreateOrder.Input;
using FluentValidation;

namespace App.InvoiSysTest.Application.UseCases.Order.CreateOrder.Validator;

public class CreateOrderValidator : AbstractValidator<CreateOrderInput>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.OrderNumber)
           .NotEmpty().WithMessage("O número do pedido é obrigatório.")
           .MaximumLength(50).WithMessage("O número do pedido deve ter no máximo 50 caracteres.");

        RuleFor(x => x.RequestDate)
           .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de solicitação não pode estar no futuro.");

        RuleFor(x => x.EstimatedDeliveryDate)
           .GreaterThan(x => x.RequestDate).WithMessage("A data estimada de entrega deve ser maior que a data de solicitação.");

        RuleFor(x => x.OrderNote)
           .NotEmpty().WithMessage("A observação do pedido é obrigatória.")
           .MaximumLength(500).WithMessage("A observação deve ter no máximo 500 caracteres.");

        RuleFor(x => x.Products)
           .NotNull().WithMessage("A lista de produtos é obrigatória.")
           .NotEmpty().WithMessage("A lista de produtos não pode estar vazia.");

        RuleForEach(x => x.Products).SetValidator(new ProductValidator());
    }
}