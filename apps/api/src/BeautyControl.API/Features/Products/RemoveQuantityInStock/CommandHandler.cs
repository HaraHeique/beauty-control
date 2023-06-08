using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Domain.Products;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;

namespace BeautyControl.API.Features.Products.RemoveQuantityInStock
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
            var productDb = await _context.Products.FindAsync(new object?[] { request.ProductId }, cancellationToken);

            if (productDb is null) 
                return Result.Fail(new NotFoundError());

            // OBS.: Posso usar o método público que me disponibiliza a verificação se posso ou não remover do estoque. Se ainda sim chegar na entidade/modelo de domínio inválido então é dado como um bug, sendo um caso exceptional
            //if (!productDb.IsAvaibleForRemoveFromStock(request.Quantity))
            //    return Result.Fail("A quantidade a ser debitada no estoque é maior que a quantidade total");

            productDb.RemoveFromStock(request.Quantity);

            _context.Products.Update(productDb);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new ProductItemsRemovedFromStockEvent(productDb.Id, request.Quantity), cancellationToken);

            return Result.Ok();
        }
    }
}
