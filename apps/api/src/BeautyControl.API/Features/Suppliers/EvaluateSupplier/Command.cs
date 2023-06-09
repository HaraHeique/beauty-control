using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Suppliers.EvaluateSupplier
{
    [DisplayName("EvaluateSupplierRequest")]
    public record Command(int Id, int Rating) : IRequest<Result>;

    public class CommandValidation : AbstractValidator<Command>
    {
        public CommandValidation()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("O {PropertyName} não pode ser zero ou negativo.");

            RuleFor(c => c.Rating)
                .GreaterThanOrEqualTo(1).WithMessage("A avaliação do fornecedor deve ser maior que zero.");
        }
    }
}
