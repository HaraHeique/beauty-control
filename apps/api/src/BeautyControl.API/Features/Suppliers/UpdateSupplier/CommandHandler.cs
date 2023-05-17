using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;

namespace BeautyControl.API.Features.Suppliers.UpdateSupplier
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly AppDataContext _context;

        public CommandHandler(AppDataContext context) => _context = context;

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            // TODO: Fazer a lógica aqui usando o Dapper para mostrar uma forma diferente de utilizar isso sem de ser pelo EF Core
            throw new NotImplementedException();

            return Result.Ok();
        }
    }
}
