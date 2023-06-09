using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Features._Common.Users;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Suppliers.EvaluateSupplier
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly AppDataContext _context;
        private readonly CurrentUser _currentUser;

        public CommandHandler(AppDataContext context, CurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FindAsync(_currentUser.Id, cancellationToken);

            if (employee is null) return Result.Fail(new NotFoundError("Funcionário associado ao usuário não encontrado"));

            var supplier = await _context.Suppliers
                .Include(s => s.SupplierRatings)
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (supplier is null) return Result.Fail(new NotFoundError("Fornecedor não encontrado"));

            supplier.Evaluate(request.Rating, employee);

            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
