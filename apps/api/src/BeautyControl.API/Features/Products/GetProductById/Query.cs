using BeautyControl.API.Features.Products._Common;
using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Products.GetProductById
{
    [DisplayName("GetProductByIdRequest")]
    public record Query(int Id) : IRequest<Result<ProductResponse?>>;

    public class QueryValidation : AbstractValidator<Query>
    {
        public QueryValidation()
        {
            RuleFor(q => q.Id)
                .NotEmpty().WithMessage("O {PropertyName} está com valor inválido.")
                .GreaterThanOrEqualTo(0).WithMessage("O {PropertyName} não pode ser negativo.");
        }
    }
}
