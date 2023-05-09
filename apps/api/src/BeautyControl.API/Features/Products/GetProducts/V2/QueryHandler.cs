using BeautyControl.API.Features._Common.Contracts;
using BeautyControl.API.Features.Products._Common;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8602
namespace BeautyControl.API.Features.Products.GetProducts.V2
{
    public class QueryHandler : IRequestHandler<Query, Result<PaginatedResponse<ProductResponse>>>
    {
        private readonly AppDataContext _context;

        public QueryHandler(AppDataContext context) => _context = context;

        public async Task<Result<PaginatedResponse<ProductResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var queryableResponse = _context.Products
               .AsNoTracking()
               .OrderByDescending(p => p.CreationDate)
               .Select(p => new ProductResponse
               {
                   Id = p.Id,
                   Name = p.Name,
                   Description = p.Description,
                   ImageUrl = p.Image.Url, // Apesar de ser nullable o EF Core é inteligente de reconhecer automaticamente e não dar NullException
                   Quantity = p.Quantity,
                   RunningOutOfStock = p.RunningOutOfStock,
                   Category = p.Category,
                   StatusStock = p.Status
               });

            var paginatedResponse = await PaginatedResponse<ProductResponse>
                .CreateAsync(queryableResponse, request.PageNumber, request.PageSize, cancellationToken);

            return Result.Ok(paginatedResponse);
        }
    }
}
