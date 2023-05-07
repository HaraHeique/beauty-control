using MediatR;

namespace BeautyControl.API.Domain.Products
{
    public sealed record ProductDeletedEvent(Product Product) : INotification;
}
