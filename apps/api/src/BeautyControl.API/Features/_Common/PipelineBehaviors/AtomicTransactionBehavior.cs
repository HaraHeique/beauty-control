using BeautyControl.API.Infra.Data;
using MediatR;

namespace BeautyControl.API.Features._Common.PipelineBehaviors
{
    public interface IAtomicTransactionRequest { }

    public class AtomicTransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IAtomicTransactionRequest
    {
        private readonly AppDataContext _context;

        public AtomicTransactionBehavior(AppDataContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var response = await next();
                
                await transaction.CommitAsync(cancellationToken);

                return response;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        }
    }
}
