using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;

namespace BeautyControl.API.Features.Products.AddQuantityInStock
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly AppDataContext _context;

        public CommandHandler(AppDataContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var productDb = await _context.Products.FindAsync(new object?[] { request.ProductId }, cancellationToken);

            if (productDb is null) 
                return Result.Fail(new NotFoundError());

            productDb.AddInStock(request.Quantity);

            _context.Products.Update(productDb);
            await _context.SaveChangesAsync(cancellationToken);

            // TODO: Publicar evento que foi adicionado produto no estoque. Talvez faça sentido a própria entidade de Product adicionar o evento para depois no momento de salvar a mudança publicar o evento

            return Result.Ok();
        }
    }
}
