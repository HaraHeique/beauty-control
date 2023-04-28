﻿using BeautyControl.API.Domain.Products;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;

namespace BeautyControl.API.Features.Products.CreateProduct
{
    public class CommandHandler : IRequestHandler<Command, Result<int>>
    {
        private readonly AppDataContext _context;

        public CommandHandler(AppDataContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.ImageUpload is not null)
            {

            }

            var product = new Product(
                request.Name, request.Description, null, 
                request.RunningOutOfStock, request.Category
            );

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok(product.Id);
        }
    }
}
