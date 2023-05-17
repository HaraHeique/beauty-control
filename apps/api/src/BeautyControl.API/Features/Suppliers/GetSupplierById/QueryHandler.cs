using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Features.Suppliers._Common;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Suppliers.GetSupplierById
{
    public class QueryHandler : IRequestHandler<Query, Result<SupplierResponse?>>
    {
        private readonly AppDataContext _context;

        public QueryHandler(AppDataContext context) => _context = context;

        public async Task<Result<SupplierResponse?>> Handle(Query request, CancellationToken cancellationToken)
        {
            var queryResponse = await _context.Suppliers
                .AsNoTracking()
                .Where(p => p.Id == request.Id)
                .Select(p => new SupplierResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Observation = p.Observation,
                    Telephone = p.Telephone.FormattedNumber,
                    AverageRating = p.AverageRating
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (queryResponse is null)
                return Result.Fail(new NotFoundError());

            return Result.Ok<SupplierResponse?>(queryResponse);
        }
    }
}
