using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Products.DeleteProductImage
{
    [DisplayName("DeleteProductImageRequest")]
    public record Command(int ProductId) : IRequest<Result>;

    public class CommandValidation : AbstractValidator<Command>
    {
        public CommandValidation()
        {
            RuleFor(q => q.ProductId)
                .NotEmpty().WithMessage("O Id do produto está com valor inválido.")
                .GreaterThanOrEqualTo(0).WithMessage("O Id do produto não pode ser negativo.");
        }
    }
}
