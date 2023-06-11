using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using BeautyControl.API.Infra.Data;
using Dapper;
using BeautyControl.API.Domain.StockMovements;
using BeautyControl.API.Features.Reports.GetStockWorkflow;
using Microsoft.EntityFrameworkCore;
using BeautyControl.API.Domain.Suppliers;

namespace BeautyControl.API.Features.Reports.GetProductsPurchasedBySuppliers
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.ReportsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Query>
        .WithActionResult<Response>
    {
        private readonly AppDataContext _context;

        public Endpoint(AppDataContext context) => _context = context;

        [HttpGet("products-purchased-by-suppliers")]
        [SwaggerOperation(
            Summary = "Obtenção dos produtos comprados por fornecedor",
            Description = "Este endpoint é responsável por fornecer todos os produtos comprados por cada fornecedor.",
            OperationId = "Reports.GetProductsPurchasedBySuppliers",
            Tags = new[] { Tags.Reports }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        public async override Task<ActionResult<Response>> HandleAsync([FromQuery] Query request, CancellationToken cancellationToken = default)
        {
            using var dbConnection = _context.Database.GetDbConnection();

            var sqlQuery = BuildSqlQuery(request);

            var dataModels = await dbConnection.QueryAsync<RootDataModel, SupplierDataModel, ProductDataModel, RootDataModel>(sqlQuery, (root, supplier, product) => 
            {
                root.Supplier = supplier;
                root.Product = product;

                return root;

            }, splitOn: "Id,SupplierId,ProductId", param: BuildParams(request));

            return Ok();
        }

        private static string BuildSqlQuery(Query request)
        {
            var dynamicFilterQuery = !request.HasAnyParams() ? string.Empty : $@"
	            {(request.Start.HasValue ? "AND SM.Date >= @StartDate" : string.Empty)}
                {(request.End.HasValue ? "AND SM.Date <= @EndDate" : string.Empty)}
            ";

            return @$"
                S.Id AS SupplierId,
	            S.Id,
	            S.Name,
	            P.Id AS ProductId,
	            P.Id,
	            P.Name,
	            P.Category,
	            SM.Id,
	            SM.Quantity
                FROM [BeautyControl].[Business].[Suppliers] AS S
                LEFT JOIN [BeautyControl].[Business].[StockMovements] AS SM ON SM.SupplierId = S.Id
                LEFT JOIN [BeautyControl].[Business].[Products] AS P ON P.Id = SM.ProductId
                WHERE 
	                SM.Process = @InputProcess
                    {dynamicFilterQuery};
            ";
        }

        private static DynamicParameters BuildParams(Query request)
        {
            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("@InputProcess", StockProcess.Input);

            if (!request.HasAnyParams()) return dynamicParams;

            request.Deconstruct(out var startDate, out var endDate);

            if (startDate.HasValue)
                dynamicParams.Add("@StartDate", startDate.Value);

            if (endDate.HasValue)
                dynamicParams.Add("@EndDate", endDate.Value);

            return dynamicParams;
        }
    }
}
