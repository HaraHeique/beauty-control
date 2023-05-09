using BeautyControl.API.Features._Common.Contracts;
using BeautyControl.API.Features.Products._Common;
using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Products.GetProducts.V2
{
    [DisplayName("GetProductsPaginatedRequest")]
    public record Query : IRequest<Result<PaginatedResponse<ProductResponse>>>
    {
        // TODO: Caso queira pode passar vários campos de filtros para os produtos aqui

        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class QueryValidation : AbstractValidator<Query>
    {
        public QueryValidation()
        {
            RuleFor(q => q.PageNumber)
                .GreaterThan(0).WithMessage("O número da página selecionado deve ser maior que zero.");
            
            RuleFor(q => q.PageSize)
                .GreaterThan(0).WithMessage("O tamanho da página selecionado deve ser maior que zero.");
        }
    }
}
