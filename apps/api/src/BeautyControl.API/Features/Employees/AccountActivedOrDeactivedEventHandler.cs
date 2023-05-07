using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Infra.Data;
using MediatR;

// TODO: Criar eventos aqui também para mandar email para funcionário avisando que foi ativado ou desativado
namespace BeautyControl.API.Features.Employees
{
    public class AccountActivedEventHandler : INotificationHandler<AccountActivedEvent>
    {
        private readonly AppDataContext _context;

        public AccountActivedEventHandler(AppDataContext context) => _context = context;

        public async Task Handle(AccountActivedEvent notification, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FindAsync(new object?[] { notification.Id }, cancellationToken: cancellationToken);

            employee!.ActiveEmployee();

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
    
    public class AccountDeactivedEventHandler : INotificationHandler<AccountDeactivedEvent>
    {
        private readonly AppDataContext _context;

        public AccountDeactivedEventHandler(AppDataContext context) => _context = context;

        public async Task Handle(AccountDeactivedEvent notification, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FindAsync(new object?[] { notification.Id }, cancellationToken: cancellationToken);

            employee!.DeactiveEmployee();

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
