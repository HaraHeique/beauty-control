using BeautyControl.API.Domain._Common.ValueObjects;
using BeautyControl.API.Domain.Suppliers;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;

namespace BeautyControl.API.Features.Suppliers.CreateSupplier
{
    public class CommandHandler : IRequestHandler<Command, Result<int>>
    {
        private readonly AppDataContext _context;

        public CommandHandler(AppDataContext context) => _context = context;

        public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            var supplier = new Supplier(
                request.Name, request.Observation, 
                Telephone.Create(request.Telephones).ToList(), 
                Email.Create(request.Emails).ToList()
            );

            await _context.Suppliers.AddAsync(supplier, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok(supplier.Id);
        }
    }
}
