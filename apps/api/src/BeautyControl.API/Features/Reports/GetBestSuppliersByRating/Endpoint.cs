using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using BeautyControl.API.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace BeautyControl.API.Features.Reports.GetBestSuppliersByRating
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.ReportsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Query>
        .WithActionResult<IEnumerable<Response>>
    {
        private readonly AppDataContext _context;

        public Endpoint(AppDataContext context) => _context = context;

        [HttpGet("best-suppliers")]
        [SwaggerOperation(
            Summary = "Obtenção dos melhores fornecedores",
            Description = "Este endpoint é responsável por entregar os fornecedores mais bem avaliados baseado na avaliação passado como parâmetro na query.",
            OperationId = "Reports.GetBestSuppliersByRating",
            Tags = new[] { Tags.Reports }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Response>))]
        public async override Task<ActionResult<IEnumerable<Response>>> HandleAsync([FromQuery] Query request, CancellationToken cancellationToken = default)
        {
            using var dbConnection = _context.Database.GetDbConnection();

            var sqlQuery = @"
                SELECT 
	                S.Id, 
	                S.Name, 
	                S.AverageRating
                FROM [BeautyControl].[Business].[Suppliers] AS S
                WHERE S.AverageRating >= @Rating
                ORDER BY 
	                S.AverageRating DESC, 
	                S.Name;
            ";

            var @params = new { request.Rating };

            var response = await dbConnection.QueryAsync<Response>(sqlQuery, @params);

            return Ok(response);
        }
    }
}
