using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Features.Account._Common;
using BeautyControl.API.Infra.Identity;
using Dapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Account.GetUserById
{
    public class QueryHandler : IRequestHandler<Query, Result<UserResponse>>
    {
        private readonly AppIdentityContext _context;

        public QueryHandler(AppIdentityContext context) => _context = context;

        public async Task<Result<UserResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = _context.Database.GetDbConnection();

            var @params = new { request.Id };

            // Sei que o ideal não seria obter os dados de dois contextos delimitados diferentes, mas por questões práticas e simples vai assim mesmo. No mundo ideal talvez consumir os dados de cada contexto e fazer um ViewComposition seria o ideal
            var response = await connection.QueryFirstOrDefaultAsync<UserResponse>(@"
                SELECT 
	                [Users].Id,
	                [Users].Email,
	                [Users].Active,
	                [Users].UserName,
	                [Employees].[Name] AS FullName,
	                [Employees].Position
                FROM [Identity].[AspNetUsers] AS [Users]
                INNER JOIN [Business].[Employees] AS [Employees] ON [Users].Id = [Employees].Id
                WHERE [Users].Id = @Id;
            ", @params);

            return response is null ? Result.Fail(new NotFoundError()) : Result.Ok(response);
        }
    }
}
