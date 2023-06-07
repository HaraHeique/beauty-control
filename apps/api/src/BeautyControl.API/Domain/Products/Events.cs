using MediatR;

namespace BeautyControl.API.Domain.Products
{
    public sealed record ProductDeletedEvent(Product Product) : INotification;

    public sealed record ProductItemsAddedInStockEvent(int ProductId, int SupplierId, int Quantity) : INotification; 

    public sealed record ProductItemsRemovedFromStockEvent(int ProductId, int Quantity) : INotification; 
}
