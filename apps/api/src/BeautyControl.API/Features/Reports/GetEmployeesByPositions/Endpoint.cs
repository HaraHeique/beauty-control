using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using BeautyControl.API.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;
using BeautyControl.API.Domain.Employees;

namespace BeautyControl.API.Features.Reports.GetEmployeesByPositions
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.ReportsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Query>
        .WithActionResult<EmployeeReportResponse>
    {
        private readonly AppDataContext _context;

        public Endpoint(AppDataContext context) => _context = context;

        [HttpGet("employees-by-positions")]
        [SwaggerOperation(
            Summary = "Obtenção de todos os funcionários no sistema por posições",
            Description = "Este endpoint é responsável por fornecer todos os funcionário no sistema por posições independente se estão ativos ou inativos.",
            OperationId = "Reports.GetEmployeesByPositions",
            Tags = new[] { Tags.Reports }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeReportResponse))]
        public async override Task<ActionResult<EmployeeReportResponse>> HandleAsync([FromQuery] Query request, CancellationToken cancellationToken = default)
        {
            using var dbConnection = _context.Database.GetDbConnection();

            var sqlQuery = BuildSqlQuery(request);
            var @params = new { request.Active };

            var response = (await dbConnection.QueryAsync<EmployeeReportDataResponse>(sqlQuery, @params))
                .GroupBy(erd => erd.Position);

            return Ok(new EmployeeReportResponse(
                Salesmans: response.FirstOrDefault(x => x.Key == Position.Salesman) ?? Enumerable.Empty<EmployeeReportDataResponse>(),
                Managers: response.FirstOrDefault(x => x.Key == Position.Manager) ?? Enumerable.Empty<EmployeeReportDataResponse>()
            ));
        }

        private static string BuildSqlQuery(Query request)
        {
            return @$"
                SELECT
	                E.Email,
	                E.Name,
	                E.Active,
	                E.Position
                FROM [BeautyControl].[Business].[Employees] AS E
                { (request.Active.HasValue ? "WHERE E.Active = @Active" : string.Empty) }
                ORDER BY E.Name;
            ";
        }
    }
}
