using BeautyControl.API.Features.Suppliers._Common;
using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Suppliers.UpdateSupplier
{
    [DisplayName("UpdateSupplierRequest")]
    public record Command(int Id) : BasicInfoSupplierCommand, IRequest<Result>;

    public class CommandValidation : AbstractValidator<Command>
    {
        public CommandValidation()
        {
            RuleFor(q => q.Id)
                .GreaterThan(0).WithMessage("O {PropertyName} não pode ser zero ou negativo.");

            Include(new BasicInfoSupplierCommandValidation());
        }
    }
}
