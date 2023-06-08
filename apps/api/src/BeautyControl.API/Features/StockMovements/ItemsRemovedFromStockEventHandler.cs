using BeautyControl.API.Domain._Common.Exceptions;
using BeautyControl.API.Domain.Products;
using BeautyControl.API.Domain.StockMovements;
using BeautyControl.API.Features._Common.Users;
using BeautyControl.API.Infra.Data;
using MediatR;

namespace BeautyControl.API.Features.StockMovements
{
    public class RegisterOutputStockMovementWhenItemsRemovedFromStockEventHandler : INotificationHandler<ProductItemsRemovedFromStockEvent>
    {
        private readonly AppDataContext _context;
        private readonly CurrentUser _currentUser;

        public RegisterOutputStockMovementWhenItemsRemovedFromStockEventHandler(
            AppDataContext context,
            CurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task Handle(ProductItemsRemovedFromStockEvent notification, CancellationToken cancellationToken)
        {
            if (_currentUser.Id == 0) throw new DomainException("Usuário não existe na base!");

            var outputStockMovement = StockMovement.OutputStockMovement(
                notification.Quantity, notification.ProductId, _currentUser.Id
            );

            await _context.StockMovements.AddAsync(outputStockMovement, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
