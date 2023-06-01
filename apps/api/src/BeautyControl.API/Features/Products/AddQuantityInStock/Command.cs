using BeautyControl.API.Features._Common.PipelineBehaviors;
using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Products.AddQuantityInStock
{
    [DisplayName("AddQuantityInStockRequest")]
    public record Command : IRequest<Result>, IAtomicTransactionRequest
    {
        public int ProductId { get; init; }
        public int SupplierId { get; init; }
        public int Quantity { get; init; }
    }

    public class CommandValidation : AbstractValidator<Command>
    {
        private const string invalidIdMessageTemplate = "O {PropertyName} não pode ser zero ou negativo.";

        public CommandValidation()
        {
            RuleFor(c => c.ProductId)
                .GreaterThan(0).WithMessage(invalidIdMessageTemplate).WithName("Id do produto");
            
            RuleFor(c => c.SupplierId)
                .GreaterThan(0).WithMessage(invalidIdMessageTemplate).WithName("Id do fornecedor");
            
            RuleFor(c => c.Quantity)
                .GreaterThanOrEqualTo(1).WithMessage("A quantidade de produtos a serem adicionados no estoque deve ser de no mínimo uma unidade.");
        }
    }
}
