using BeautyControl.API.Features.Suppliers._Common;
using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Suppliers.CreateSupplier
{
    [DisplayName("CreateSupplierRequest")]
    public record Command : BasicInfoSupplierCommand, IRequest<Result<int>>;

    public class CommandValidation : AbstractValidator<Command>
    {
        public CommandValidation() => Include(new BasicInfoSupplierCommandValidation());
    }
}
