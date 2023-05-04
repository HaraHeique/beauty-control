using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;

namespace BeautyControl.API.Features.Products.DeleteProduct
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
            var productDb = await _context.Products
                .FindAsync(request.Id, cancellationToken);

            if (productDb is null) return Result.Fail(new NotFoundError());

            _context.Products.Remove(productDb);
            await _context.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
