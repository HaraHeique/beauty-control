﻿using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Features.Account._Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BeautyControl.API.Features.Account.LoginAccount
{
    [AllowAnonymous]
    [ApiVersion("1")]
    [Route(Routes.AccountUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Command>
        .WithActionResult<LoggedUserResponse>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Login do usuário",
            Description = "Realizar o login de usuário a partir de Email e Senha",
            OperationId = "Account.LoginAccount",
            Tags = new[] { SwaggerOperations.Tags.Account }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoggedUserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        public async override Task<ActionResult<LoggedUserResponse>> HandleAsync([FromBody] Command request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return this.Response(result);
        }
    }
}
