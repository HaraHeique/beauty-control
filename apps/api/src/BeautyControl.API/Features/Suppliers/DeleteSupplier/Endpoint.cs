using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Infra.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;

namespace BeautyControl.API.Features.Suppliers.DeleteSupplier
{
    [Authorize(Roles = UserRoles.AdminName)]
    [ApiVersion("1")]
    [Route(Routes.SuppliersUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Remover um fornecedor.",
            Description = "Remove um fornecedor existente na base de dados.",
            OperationId = "Suppliers.DeleteSupplier",
            Tags = new[] { Tags.Suppliers }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async override Task<ActionResult> HandleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new Command(id), cancellationToken);

            return this.Response(result);
        }
    }
}
