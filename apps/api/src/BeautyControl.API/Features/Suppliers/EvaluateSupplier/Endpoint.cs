using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Infra.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;

#pragma warning disable CS8618
namespace BeautyControl.API.Features.Suppliers.EvaluateSupplier
{
    public sealed record Request
    {
        [FromRoute(Name = "id")] public int Id { get; init; }
        [FromBody] public EvaluateSupplierRequest Body { get; init; }

        public record EvaluateSupplierRequest(int Id, int Rating);
    }

    [Authorize(Roles = $"{UserRoles.AdminName},{UserRoles.EmployeeName}")]
    [ApiVersion("1")]
    [Route(Routes.SuppliersUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Request>
        .WithActionResult
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpPatch("{id:int}/rating")]
        [SwaggerOperation(
            Summary = "Avaliar um determinado fornecedor",
            Description = "Avalição de um determinado fornecedor realizada pelo usuário logado.",
            OperationId = "Suppliers.EvaluateSupplier",
            Tags = new[] { Tags.Suppliers }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async override Task<ActionResult> HandleAsync([FromRoute] Request request, CancellationToken cancellationToken = default)
        {
            if (request.Id != request.Body.Id)
                return this.ErrorResponse("ID do produto passado na rota é diferente do informado no corpo da requisição.");

            var result = await _mediator.Send(new Command(request.Id, request.Body.Rating), cancellationToken);

            return this.Response(result);
        }
    }
}
