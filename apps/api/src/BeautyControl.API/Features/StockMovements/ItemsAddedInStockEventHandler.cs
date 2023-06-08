using BeautyControl.API.Domain._Common.Exceptions;
using BeautyControl.API.Domain.Products;
using BeautyControl.API.Domain.StockMovements;
using BeautyControl.API.Features._Common.Users;
using BeautyControl.API.Infra.Data;
using MediatR;

namespace BeautyControl.API.Features.StockMovements
{
    public class RegisterInputStockMovementWhenItemsAddedInStockEventHandler : INotificationHandler<ProductItemsAddedInStockEvent>
    {
        private readonly AppDataContext _context;
        private readonly CurrentUser _currentUser;

        public RegisterInputStockMovementWhenItemsAddedInStockEventHandler(
            AppDataContext context,
            CurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task Handle(ProductItemsAddedInStockEvent notification, CancellationToken cancellationToken)
        {
            if (_currentUser.Id == 0) throw new DomainException("Usuário não existe na base!");

            var inputStockMovement = StockMovement.InputStockMovement(
                notification.Quantity, notification.ProductId,
                notification.SupplierId, _currentUser.Id
            );

            await _context.StockMovements.AddAsync(inputStockMovement, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
