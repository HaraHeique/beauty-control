using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Infra.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BeautyControl.API.Features.Suppliers.CreateSupplier
{
    [Authorize(Roles = UserRoles.AdminName)]
    [ApiVersion("1")]
    [Route(Routes.SuppliersUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Command>
        .WithActionResult<int>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar novo fornecedor",
            Description = "Criar um novo fornecedor passando as informações básicas.",
            OperationId = "Suppliers.CreateSupplier",
            Tags = new[] { SwaggerOperations.Tags.Suppliers }
        )]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async override Task<ActionResult<int>> HandleAsync([FromBody] Command request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return this.Response(result, result.IsSuccess ? HttpStatusCode.Created : HttpStatusCode.BadRequest);
        }
    }
}
