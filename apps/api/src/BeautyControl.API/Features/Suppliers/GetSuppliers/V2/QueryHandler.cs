using BeautyControl.API.Features._Common.Contracts;
using BeautyControl.API.Features.Suppliers._Common;
using BeautyControl.API.Infra.Data;
using Dapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Suppliers.GetSuppliers.V2
{
    public class QueryHandler : IRequestHandler<Query, Result<PaginatedResponse<SupplierResponse>>>
    {
        private readonly AppDataContext _context;

        public QueryHandler(AppDataContext context) => _context = context;

        public async Task<Result<PaginatedResponse<SupplierResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = _context.Database.GetDbConnection();

            var @params = new { request.PageSize, request.PageNumber };

            var multipleQueries = await connection.QueryMultipleAsync(@"
                SELECT
                    [Id],
                    [Name],
                    [Observation],
                    TelephonesJson = [Telephones],
                    EmailsJson = [Emails],
                    [AverageRating]
                FROM [BeautyControl].[Business].[Suppliers] 
                ORDER BY [Name] 
                OFFSET @PageSize * (@PageNumber - 1) ROWS
                FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(Id) FROM [BeautyControl].[Business].[Suppliers];
            ", param: @params);

            var dataModelResponse = await multipleQueries.ReadAsync<SupplierDataModel>();
            var totalItens = await multipleQueries.ReadFirstAsync<int>();

            return Result.Ok(new PaginatedResponse<SupplierResponse>(
                items: dataModelResponse.MapToResponse(), totalItens, 
                request.PageNumber, request.PageSize)
            );
        }
    }
}
