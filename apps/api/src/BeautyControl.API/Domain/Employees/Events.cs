using BeautyControl.API.Infra.Identity.Models;
using MediatR;

namespace BeautyControl.API.Domain.Employees
{
    public sealed record NewAccountCreatedEvent(int Id, string Name, string EmailAddress, Position Position)
        : INotification;
}
