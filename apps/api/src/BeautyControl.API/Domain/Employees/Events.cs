using MediatR;

namespace BeautyControl.API.Domain.Employees
{
    public sealed record NewAccountCreatedEvent(int Id, string Name, string EmailAddress, Position Position) 
        : INotification;
    
    public sealed record AccountActivatedEvent(int Id) : INotification;

    public sealed record AccountDeactivatedEvent(int Id) : INotification;

    public sealed record UserAccountNameUpdatedEvent(int Id, string FullName) : INotification;

    public sealed record UserPositionChangedEvent(int Id, Position Position) : INotification;
}
