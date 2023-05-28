using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Infra.Data;
using Dapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Suppliers.DeleteSupplier
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly AppDataContext _context;

        public CommandHandler(AppDataContext context) => _context = context;

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var affectedRows = await DeleteSupplier(request);

            if (affectedRows == 0) return Result.Fail(new NotFoundError());

            return Result.Ok();
        }

        private async Task<int> DeleteSupplier(Command request)
        {
            using var dbConnection = _context.Database.GetDbConnection();

            var @params = new { request.Id };

            return await dbConnection.ExecuteAsync(@"
                DELETE FROM [BeautyControl].[Business].[Suppliers]
                WHERE Id = @Id
            ", @params);
        }
    }
}
