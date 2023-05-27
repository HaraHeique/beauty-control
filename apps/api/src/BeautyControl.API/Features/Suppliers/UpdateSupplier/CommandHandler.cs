using BeautyControl.API.Domain._Common.Errors;
using FluentResults;
using MediatR;

namespace BeautyControl.API.Features.Suppliers.UpdateSupplier
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly DataAccess _dataAccess;

        public CommandHandler(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            // Alternativas abaixo, mas tudo depende do contexto e complexidade do projeto faz muito sentido
            //UpdateSupplier(dbConnection, request);
            //await DataAccess.UpdateSupplier(dbConnection, request);

            var affectedRows = await _dataAccess.UpdateSupplier(request);

            if (affectedRows == 0)
                return Result.Fail(new NotFoundError());

            return Result.Ok();
        }

        //private async Task UpdateSupplier(IDbConnection dbConnection, Command request)
        //{
        //    var @params = new
        //    {
        //        request.Id,
        //        request.Name,
        //        request.Observation,
        //        request.Telephone
        //    };

        //    await dbConnection.ExecuteAsync(@"
        //        UPDATE [BeautyControl].[Business].[Suppliers]
        //        SET [Name] = @Name,
        //            [Observation] = @Observation,
        //            [Telephone] = @Telephone
        //         WHERE Id = @Id
        //    ", @params);
        //}
    }
}
