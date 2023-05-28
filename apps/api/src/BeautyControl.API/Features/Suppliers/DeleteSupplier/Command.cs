using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Suppliers.DeleteSupplier
{
    [DisplayName("DeleteSupplierRequest")]
    public record Command(int Id) : IRequest<Result>;

    public class CommandValidation : AbstractValidator<Command>
    {
        public CommandValidation()
        {
            RuleFor(q => q.Id)
                .GreaterThan(0).WithMessage("O {PropertyName} deve ser maior que zero.");
        }
    }
}