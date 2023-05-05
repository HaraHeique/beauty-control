using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8602
namespace BeautyControl.API.Features.Products.GetProductImage
{
    public class QueryHandler : IRequestHandler<Query, Result<Response?>>
    {
        private readonly AppDataContext _context;

        public QueryHandler(AppDataContext context) => _context = context;

        public async Task<Result<Response?>> Handle(Query request, CancellationToken cancellationToken)
        {
            var response = await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == request.ProductId)
                .Select(p => new Response(p.Image.Name, p.Image.Url))
                .FirstOrDefaultAsync(cancellationToken);

            if (response is null)
                return Result.Fail(new NotFoundError());

            return Result.Ok<Response?>(response);
        }
    }
}
