using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;

namespace BeautyControl.API.Features.Products.UpdateProduct
{
    public record Request
    {
        [FromRoute(Name = "id")] public int Id { get; init; }
        [FromForm] public Command? FormData { get; init; }
    }

    //[Authorize(Roles = UserRoles.AdminDisplayName)]
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.ProductsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Request>
        .WithActionResult
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Atualizar produto existente",
            Description = "Atualizar um produto passando as informações básicas e valor de configuração chamado RunningOutOfStock (indicando a partir de qual valor que determina que está saindo fora do estoque). " +
            "A imagem é um dado opcional e não precisa conter no produto e quando é enviado o request com nova imagem é substituída a antiga.",
            OperationId = "Products.UpdateProduct",
            Tags = new[] { Tags.Products }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async override Task<ActionResult> HandleAsync([FromRoute] Request request, CancellationToken cancellationToken = default)
        {
            int id = request.Id; 
            var command = request.FormData;

            if (id != command?.Id) 
                return this.ErrorResponse("ID passado na rota é diferente do informado no formulário fornecido.");

            var result = await _mediator.Send(command, cancellationToken);

            return this.Response(result);
        }
    }
}
