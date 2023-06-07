using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Domain.Products;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Products.AddQuantityInStock
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly AppDataContext _context;
        private readonly IMediator _mediator;

        public CommandHandler(AppDataContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await HasSupplier(request, cancellationToken) == false)
                return Result.Fail(new NotFoundError("Fornecedor não encontrado no sistema"));

            var productDb = await _context.Products.FindAsync(new object?[] { request.ProductId }, cancellationToken);

            if (productDb is null)
                return Result.Fail(new NotFoundError("Produto não encontrado no sistema"));

            productDb.AddInStock(request.Quantity);

            _context.Products.Update(productDb);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new ProductItemsAddedInStockEvent(productDb.Id, request.SupplierId, request.Quantity), cancellationToken);

            return Result.Ok();
        }

        private async Task<bool> HasSupplier(Command request, CancellationToken cancellationToken)
        {
            return await _context.Suppliers.AnyAsync(s => s.Id == request.SupplierId, cancellationToken);
        }
    }
}
