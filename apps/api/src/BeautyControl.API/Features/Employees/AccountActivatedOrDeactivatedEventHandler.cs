using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Infra.Data;
using MediatR;

// TODO: Criar eventos aqui também para mandar email para funcionário avisando que foi ativado ou desativado
namespace BeautyControl.API.Features.Employees
{
    public class AccountActivatedEventHandler : INotificationHandler<AccountActivatedEvent>
    {
        private readonly AppDataContext _context;

        public AccountActivatedEventHandler(AppDataContext context) => _context = context;

        public async Task Handle(AccountActivatedEvent notification, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FindAsync(new object?[] { notification.Id }, cancellationToken: cancellationToken);

            employee!.ActiveEmployee();

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
    
    public class AccountDeactivatedEventHandler : INotificationHandler<AccountDeactivatedEvent>
    {
        private readonly AppDataContext _context;

        public AccountDeactivatedEventHandler(AppDataContext context) => _context = context;

        public async Task Handle(AccountDeactivatedEvent notification, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FindAsync(new object?[] { notification.Id }, cancellationToken: cancellationToken);

            employee!.DeactiveEmployee();

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
