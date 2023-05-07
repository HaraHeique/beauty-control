﻿using MediatR;

namespace BeautyControl.API.Domain.Employees
{
    public sealed record NewAccountCreatedEvent(int Id, string Name, string EmailAddress, Position Position)
        : INotification;
    
    public sealed record AccountActivedEvent(int Id) : INotification;

    public sealed record AccountDeactivedEvent(int Id) : INotification;
}
