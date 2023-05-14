using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Infra.Data;
using MediatR;

namespace BeautyControl.API.Features.Employees
{
    public class UserAccountNameUpdatedEventHandler : INotificationHandler<UserAccountNameUpdatedEvent>
    {
        private readonly AppDataContext _context;
        private readonly ILogger<UserAccountNameUpdatedEventHandler> _logger;

        public UserAccountNameUpdatedEventHandler(AppDataContext context, ILogger<UserAccountNameUpdatedEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(UserAccountNameUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FindAsync(notification.Id, cancellationToken);

            if (employee is null)
            {
                _logger.LogCritical("O funcionário {Id} não existe na base de dados do sistema. Favor verificar isto urgentemente.", notification.Id);
                return;
            }

            employee.ChangeName(notification.FullName);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
