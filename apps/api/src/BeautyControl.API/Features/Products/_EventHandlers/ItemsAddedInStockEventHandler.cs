using BeautyControl.API.Domain._Common.Exceptions;
using BeautyControl.API.Domain.Products;
using BeautyControl.API.Domain.StockMovements;
using BeautyControl.API.Features._Common.Users;
using BeautyControl.API.Infra.Data;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeautyControl.API.Features.Products._EventHandlers
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

            using IDbConnection dbConnection = _context.Database.GetDbConnection();

            var @params = new
            {
                notification.Quantity,
                Date = DateTime.Now,
                Process = StockProcess.Input,
                notification.ProductId,
                notification.SupplierId,
                EmployeeId = _currentUser.Id
            };

            await dbConnection.ExecuteAsync(@"
                INSERT INTO [BeautyControl].[Business].[StockMovements](Quantity, Date, Process, ProductId, SupplierId, EmployeeId)
                VALUES (@Quantity, @Date, @Process, @ProductId, @SupplierId, @EmployeeId); 
            ", @params);
        }
    }
    
    public class RegisterOutputStockMovementWhenItemsAddedInStockEventHandler : INotificationHandler<ProductItemsRemovedFromStockEvent>
    {
        private readonly AppDataContext _context;
        private readonly CurrentUser _currentUser;

        public RegisterOutputStockMovementWhenItemsAddedInStockEventHandler(
            AppDataContext context, 
            CurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task Handle(ProductItemsRemovedFromStockEvent notification, CancellationToken cancellationToken)
        {
            if (_currentUser.Id == 0) throw new DomainException("Usuário não existe na base!");

            using IDbConnection dbConnection = _context.Database.GetDbConnection();

            var @params = new
            {
                notification.Quantity,
                Date = DateTime.Now,
                Process = StockProcess.Output,
                notification.ProductId,
                SupplierId = (int?)null,
                EmployeeId = _currentUser.Id
            };

            await dbConnection.ExecuteAsync(@"
                INSERT INTO [BeautyControl].[Business].[StockMovements](Quantity, Date, Process, ProductId, SupplierId, EmployeeId)
                VALUES (@Quantity, @Date, @Process, @ProductId, @SupplierId, @EmployeeId); 
            ", @params);
        }
    }
}
