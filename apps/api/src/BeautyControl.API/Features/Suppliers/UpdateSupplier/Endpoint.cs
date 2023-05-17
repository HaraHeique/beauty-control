using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Infra.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

#pragma warning disable CS8618
namespace BeautyControl.API.Features.Suppliers.UpdateSupplier
{
    public record Request
    {
        [FromRoute(Name = "id")] public int Id { get; init; }
        [FromBody] public Command Body { get; init; }
    }

    [Authorize(Roles = UserRoles.AdminName)]
    [ApiVersion("1")]
    [Route(Routes.SuppliersUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Request>
        .WithActionResult
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Atualizar fornecedor existente.",
            Description = "Atualizar um fornecedor existente passando as mesmas informações básicas da criação de um novo fornecedor.",
            OperationId = "Suppliers.UpdateSupplier",
            Tags = new[] { SwaggerOperations.Tags.Suppliers }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async override Task<ActionResult> HandleAsync([FromRoute] Request request, CancellationToken cancellationToken = default)
        {
            if (request.Id != request.Body.Id)
                return this.ErrorResponse("ID passado na rota é diferente do informado no corpo da requisição.");

            var result = await _mediator.Send(request.Body, cancellationToken);

            return this.Response(result);
        }
    }
}
