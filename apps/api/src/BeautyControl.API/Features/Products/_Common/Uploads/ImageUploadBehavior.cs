using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Products._Common.Uploads
{
    public class ImageUploadBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IImageUploadRequest
        where TResponse : ResultBase
    {
        private readonly AppDataContext _context;
        private readonly ILogger<ImageUploadBehavior<TRequest, TResponse>> _logger;
        private readonly ImageUploadManager _uploadManager;

        public ImageUploadBehavior(
            AppDataContext context,
            ILogger<ImageUploadBehavior<TRequest, TResponse>> logger,
            ImageUploadManager uploadManager)
        {
            _context = context;
            _logger = logger;
            _uploadManager = uploadManager;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request?.ImageUpload is null) return await next();

            if (request.Id.HasValue) await DeleteImage(request);

            var uploadResult = await _uploadManager.UploadImage(request, cancellationToken);

            if (uploadResult.IsFailed) return uploadResult as dynamic;

            return await next();
        }

        private async Task DeleteImage(TRequest request)
        {
            var image = await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == request.Id!.Value)
                .Select(p => p.Image)
                .FirstOrDefaultAsync();

            _uploadManager.DeleteImage(request.Id!.Value, image);
        }
    }
}
