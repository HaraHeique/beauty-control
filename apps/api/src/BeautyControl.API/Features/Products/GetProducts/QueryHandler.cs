using BeautyControl.API.Features.Products._Common;
using BeautyControl.API.Infra.Data;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Products.GetProducts
{
    public class QueryHandler : IRequestHandler<Query, IEnumerable<ProductResponse>>
    {
        private readonly AppDataContext _context;

        public QueryHandler(AppDataContext context) => _context = context;

        public async Task<IEnumerable<ProductResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var dbConnection = _context.Database.GetDbConnection();

            var response = await dbConnection.QueryAsync<ProductResponse>(@"
                SELECT 
                    [Id],
                    [Name],
                    [Description],
                    [ImageUrl],
                    [Quantity],
                    [RunningOutOfStock],
                    [StatusStock],
                    [Category]
                FROM [BeautyControl].[Business].[Products];
            ");

            return response;
        }
    }
}
