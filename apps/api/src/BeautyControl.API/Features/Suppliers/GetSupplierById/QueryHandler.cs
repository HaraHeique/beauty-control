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
                .Where(s => s.Id == request.Id)
                .Select(s => new SupplierResponse(s.Telephones, s.Emails)
                {
                    Id = s.Id,
                    Name = s.Name,
                    Observation = s.Observation,
                    AverageRating = s.AverageRating
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (queryResponse is null)
                return Result.Fail(new NotFoundError());

            return Result.Ok<SupplierResponse?>(queryResponse);
        }
    }
}
