using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Infra.Identity.Models;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BeautyControl.API.Features.Account.ActiveDeactiveAccount
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator _mediator;

        public CommandHandler(
            UserManager<AppUser> userManager,
            IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user is null) return Result.Fail(new NotFoundError());

            user.Active = request.Active;
            await _userManager.UpdateAsync(user);

            object @event = request.Active ? new AccountActivedEvent(user.Id) : new AccountDeactivedEvent(user.Id);
            await _mediator.Publish(@event, cancellationToken);

            return Result.Ok();
        }
    }
}
