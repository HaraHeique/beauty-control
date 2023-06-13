using BeautyControl.API.Features.Suppliers._Common;
using BeautyControl.API.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Suppliers.GetSuppliers.V1
{
    public class QueryHandler : IRequestHandler<Query, IEnumerable<SupplierResponse>>
    {
        private readonly AppDataContext _context;

        public QueryHandler(AppDataContext context) => _context = context;

        public async Task<IEnumerable<SupplierResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Suppliers
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .Select(s => new SupplierResponse(s.Telephones, s.Emails)
                {
                    Id = s.Id,
                    Name = s.Name,
                    Observation = s.Observation,
                    AverageRating = s.AverageRating
                })
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
