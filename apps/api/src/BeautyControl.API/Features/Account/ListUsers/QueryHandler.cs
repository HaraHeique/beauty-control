using BeautyControl.API.Features._Common.Users;
using BeautyControl.API.Features.Account._Common;
using BeautyControl.API.Infra.Identity;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Account.ListUsers
{
    public class QueryHandler : IRequestHandler<Query, IEnumerable<UserResponse>>
    {
        private readonly AppIdentityContext _context;
        private readonly CurrentUser _currentUser;

        public QueryHandler(AppIdentityContext context, CurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<UserResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = _context.Database.GetDbConnection();

            var @params = new { CurrentUserId = _currentUser.Id };

            // Sei que o ideal não seria obter os dados de dois contextos delimitados diferentes, mas por questões práticas e simples vai assim mesmo. No mundo ideal talvez consumir os dados de cada contexto e fazer um ViewComposition seria o ideal
            var response = await connection.QueryAsync<UserResponse>(@"
                SELECT 
	                [Users].Id,
	                [Users].Email,
	                [Users].Active,
	                [Users].UserName,
	                [Employees].[Name] AS FullName,
	                [Employees].Position
                FROM [Identity].[AspNetUsers] AS [Users]
                INNER JOIN [Business].[Employees] AS [Employees] ON [Users].Id = [Employees].Id
                WHERE [Users].Id <> @CurrentUserId;
            ", @params);

            return response;
        }
    }
}
