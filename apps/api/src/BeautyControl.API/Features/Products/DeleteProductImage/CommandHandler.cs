using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Features.Products._Common.Uploads;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;

namespace BeautyControl.API.Features.Products.DeleteProductImage
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly AppDataContext _context;
        private readonly ImageUploadManager _uploadManager;

        public CommandHandler(AppDataContext context, ImageUploadManager uploadManager)
        {
            _context = context;
            _uploadManager = uploadManager;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var productDb = await _context.Products.FindAsync(request.ProductId);

            if (productDb?.Image is null)
                return Result.Fail(new NotFoundError($"Não foi encontrada imagem para o produto com ID {request.ProductId}."));

            var result = _uploadManager.DeleteImage(request.ProductId, productDb.Image);

            if (result.IsSuccess)
            {
                productDb.DeleteImage();
                await _context.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}
