using BeautyControl.API.Infra.Data;
using Dapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Suppliers.UpdateSupplier
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly AppDataContext _context;

        public CommandHandler(AppDataContext context) => _context = context;

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            using var dbConnection = _context.Database.GetDbConnection();

            await DataAccess.UpdateSupplier(dbConnection, request);

            return Result.Ok();
        }
    }
}
