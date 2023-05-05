using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Products.GetProductImage
{
    [DisplayName("GetProductImageRequest")]
    public record Query(int ProductId) : IRequest<Result<Response?>>;

    [DisplayName("ImageResponse")]
    public record Response(string Name, string Url);

    public class QueryValidation : AbstractValidator<Query>
    {
        public QueryValidation()
        {
            RuleFor(q => q.ProductId)
                .NotEmpty().WithMessage("O Id do produto está com valor inválido.")
                .GreaterThanOrEqualTo(0).WithMessage("O Id do produto não pode ser negativo.");
        }
    }
}
