﻿using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Features.Products._Common;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Products.GetProductById
{
    public class QueryHandler : IRequestHandler<Query, Result<ProductResponse?>>
    {
        private readonly AppDataContext _context;

        public QueryHandler(AppDataContext context) => _context = context;

        public async Task<Result<ProductResponse?>> Handle(Query request, CancellationToken cancellationToken)
        {
            var queryResponse = await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == request.Id)
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.Image != null ? p.Image.Url : null,
                    Quantity = p.Quantity,
                    RunningOutOfStock = p.RunningOutOfStock,
                    Category = p.Category,
                    StatusStock = p.Status
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (queryResponse is null)
                return Result.Fail(new NotFoundError());

            return Result.Ok<ProductResponse?>(queryResponse);
        }
    }
}
