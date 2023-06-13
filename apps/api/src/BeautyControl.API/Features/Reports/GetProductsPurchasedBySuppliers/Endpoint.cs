using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using BeautyControl.API.Infra.Data;
using Dapper;
using BeautyControl.API.Domain.StockMovements;
using Microsoft.EntityFrameworkCore;
using BeautyControl.API.Features.Reports._Common;

namespace BeautyControl.API.Features.Reports.GetProductsPurchasedBySuppliers
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.ReportsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Query>
        .WithActionResult<IEnumerable<Response>>
    {
        private readonly AppDataContext _context;

        private readonly IDictionary<int, Response> _employeesLookup = new Dictionary<int, Response>();

        public Endpoint(AppDataContext context) => _context = context;

        [HttpGet("products-purchased-by-suppliers")]
        [SwaggerOperation(
            Summary = "Obtenção dos produtos comprados por fornecedor",
            Description = "Este endpoint é responsável por fornecer todos os produtos comprados por cada fornecedor.",
            OperationId = "Reports.GetProductsPurchasedBySuppliers",
            Tags = new[] { Tags.Reports }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Response>))]
        public async override Task<ActionResult<IEnumerable<Response>>> HandleAsync([FromQuery] Query request, CancellationToken cancellationToken = default)
        {
            using var dbConnection = _context.Database.GetDbConnection();

            var sqlQuery = BuildSqlQuery(request);

            await dbConnection.QueryAsync<Response, ProductWorkflowDataResponse, Response>(
                sqlQuery, map: MapResponse, 
                splitOn: "Id,Id", param: BuildParams(request)
            );

            return Ok(_employeesLookup.Values);
        }

        private static string BuildSqlQuery(Query request)
        {
            var dynamicFilterQuery = !request.HasAnyParams() ? string.Empty : $@"
	            {(request.Start.HasValue ? "AND SM.Date >= @StartDate" : string.Empty)}
                {(request.End.HasValue ? "AND SM.Date <= @EndDate" : string.Empty)}
            ";

            return @$"
                SELECT
	                S.Id,
	                S.Name,
	                P.Id,
	                P.Name,
	                P.Category,
	                Quantity = SUM(SM.Quantity)
                FROM [BeautyControl].[Business].[Suppliers] AS S
                LEFT JOIN [BeautyControl].[Business].[StockMovements] AS SM ON SM.SupplierId = S.Id
                LEFT JOIN [BeautyControl].[Business].[Products] AS P ON P.Id = SM.ProductId
                WHERE 
	                SM.Process = @InputProcess
	                {dynamicFilterQuery}
                GROUP BY S.Id, S.Name, P.Id, P.Name, P.Category
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

        private Response MapResponse(Response response, ProductWorkflowDataResponse product)
        {
            if (!_employeesLookup.TryGetValue(response.Id, out var employeeLookup))
            {
                employeeLookup = response;
                _employeesLookup.Add(response.Id, response);
            }

            employeeLookup.Products.Add(product);

            return employeeLookup;
        }
    }
}
