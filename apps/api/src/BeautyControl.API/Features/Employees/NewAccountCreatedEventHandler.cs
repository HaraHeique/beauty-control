using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Infra.Data;
using MediatR;

namespace BeautyControl.API.Features.Employees
{
    public class CreateEmployeeWhenNewAccountCreatedEventHandler : INotificationHandler<NewAccountCreatedEvent>
    {
        private readonly AppDataContext _context;

        public CreateEmployeeWhenNewAccountCreatedEventHandler(AppDataContext context)
        {
            _context = context;
        }

        public async Task Handle(NewAccountCreatedEvent @event, CancellationToken cancellationToken)
        {
            // Não são feitas validações em eventos internos (domínio) porque são gerados internamentes então já sub-entende que o evento está em estado válido
            var employee = new Employee(@event.Id, @event.Name, new Email(@event.EmailAddress), @event.Position);

            await _context.Employees.AddAsync(employee, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
