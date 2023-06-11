using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using BeautyControl.API.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;
using BeautyControl.API.Domain.StockMovements;
using BeautyControl.API.Features.Reports._Common;

namespace BeautyControl.API.Features.Reports.GetStockWorkflow
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.ReportsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<GetProductsWorkflowQuery>
        .WithActionResult<ProductWorkflowResponse>
    {
        private readonly AppDataContext _context;

        public Endpoint(AppDataContext context) => _context = context;

        [HttpGet("stock-workflow")]
        [SwaggerOperation(
            Summary = "Obtenção dos entradas e saídas de produtos do estoque",
            Description = "Este endpoint é responsável por fornecer as entradas e saídas das unidades de produtos no estoque.",
            OperationId = "Reports.GetProductsWorkflow",
            Tags = new[] { Tags.Reports }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductWorkflowResponse))]
        public async override Task<ActionResult<ProductWorkflowResponse>> HandleAsync([FromQuery] GetProductsWorkflowQuery request, CancellationToken cancellationToken = default)
        {
            using var dbConnection = _context.Database.GetDbConnection();

            var sqlQuery = BuildSqlQuery(request);

            var inputWorkflowResult = await dbConnection
                .QueryAsync<ProductWorkflowDataResponse>(sqlQuery, BuildParams(StockProcess.Input, request));

            var outputWorkflowResult = await dbConnection
                .QueryAsync<ProductWorkflowDataResponse>(sqlQuery, BuildParams(StockProcess.Output, request));

            return Ok(new ProductWorkflowResponse(inputWorkflowResult, outputWorkflowResult));
        }

        private static string BuildSqlQuery(GetProductsWorkflowQuery request)
        {
            var dynamicFilterQuery = !request.HasParams() ? string.Empty : $@"
	            {(request.Start.HasValue ? "AND SM.Date >= @StartDate" : string.Empty)}
                {(request.End.HasValue ? "AND SM.Date <= @EndDate" : string.Empty)}
            ";

            return @$"
                SELECT
	                P.Id,
	                P.Name,
	                P.Category,
	                Quantity = SUM(SM.Quantity)
                FROM [BeautyControl].[Business].[Products] AS P
                LEFT JOIN [BeautyControl].[Business].[StockMovements] AS SM ON SM.ProductId = P.Id
                WHERE
	                SM.Process = @StockProcess
                    {dynamicFilterQuery}
                GROUP BY P.Id, P.Name, P.Category
                ORDER BY Quantity DESC, P.Name;
            ";
        }

        private static DynamicParameters BuildParams(StockProcess process, GetProductsWorkflowQuery request)
        {
            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("@StockProcess", process);

            if (!request.HasParams()) return dynamicParams;

            request.Deconstruct(out var startDate, out var endDate);

            if (startDate.HasValue)
                dynamicParams.Add("@StartDate", startDate.Value);

            if (endDate.HasValue)
                dynamicParams.Add("@EndDate", endDate.Value);

            return dynamicParams;
        }
    }
}
