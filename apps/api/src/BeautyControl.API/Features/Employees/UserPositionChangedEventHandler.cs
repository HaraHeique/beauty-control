using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Infra.Data;
using MediatR;

namespace BeautyControl.API.Features.Employees
{
    public class UserPositionChangedEventHandler : INotificationHandler<UserPositionChangedEvent>
    {
        private readonly AppDataContext _context;
        private readonly ILogger<UserPositionChangedEventHandler> _logger;

        public UserPositionChangedEventHandler(AppDataContext context, ILogger<UserPositionChangedEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(UserPositionChangedEvent notification, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FindAsync(notification.Id, cancellationToken);

            if (employee is null)
            {
                _logger.LogCritical("O funcionário {Id} não existe na base de dados do sistema. Favor verificar isto urgentemente.", notification.Id);
                return;
            }

            employee.ChangePosition(notification.Position);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
