using BeautyControl.API.Features.Suppliers._Common;
using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Suppliers.GetSupplierById
{
    [DisplayName("GetSupplierByIdRequest")]
    public record Query(int Id) : IRequest<Result<SupplierResponse?>>;

    public class QueryValidation : AbstractValidator<Query>
    {
        public QueryValidation()
        {
            RuleFor(q => q.Id)
                .GreaterThan(0).WithMessage("O {PropertyName} não pode ser zero ou negativo.");
        }
    }
}
