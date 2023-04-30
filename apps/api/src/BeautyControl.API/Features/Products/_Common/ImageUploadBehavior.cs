using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Products._Common
{
    public class ImageUploadBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IImageUploadRequest
        where TResponse : ResultBase
    {
        private const string _wwwrootDirectory = "images";

        private readonly IWebHostEnvironment _env;
        private readonly AppDataContext _context;
        private readonly ILogger<ImageUploadBehavior<TRequest, TResponse>> _logger;

        public ImageUploadBehavior(
            IWebHostEnvironment env, 
            AppDataContext context, 
            ILogger<ImageUploadBehavior<TRequest, TResponse>> logger)
        {
            _env = env;
            _context = context;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request?.ImageUpload is null) return await next();

            if (request.Id.HasValue) await DeleteImage(request);

            var uploadResult = await UploadImage(request, cancellationToken);

            if (uploadResult.IsFailed) return uploadResult as dynamic;

            return await next();
        }

        private async Task<Result> UploadImage(TRequest request, CancellationToken cancellationToken)
        {
            // TODO: implementar a extensão e feature de logs que o FluentResult fornece na própria Lib
            if (request.ImageUpload is null || request.ImageUpload.Length == 0)
                return Result.Fail("É obrigatório fornecer uma imagem para este produto.")
                    .LogIfFailed<ImageUploadBehavior<TRequest, TResponse>>();

            var imageName = $"{Guid.NewGuid()}_{request.ImageUpload.FileName}";
            var filePath = Path.Combine(_env.WebRootPath, _wwwrootDirectory, imageName);

            if (File.Exists(filePath))
                return Result.Fail($"Já existe uma imagem com o nome {imageName}.")
                    .LogIfFailed<ImageUploadBehavior<TRequest, TResponse>>();

            using var stream = new FileStream(filePath, FileMode.Create);
            await request.ImageUpload.CopyToAsync(stream, cancellationToken);

            _logger.LogInformation("Upload da imagem {imageName} ({filePath}) realizada com sucesso!", imageName, filePath);

            request.Image = imageName;

            return Result.Ok();
        }

        private async Task DeleteImage(TRequest request)
        {
            var imageName = await _context.Products
                .Where(p => p.Id == request.Id!.Value)
                .Select(p => p.Image)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(imageName))
            {
                _logger.LogWarning("Não foi encontrada a imagem para o produto ID {id}", request.Id);
                return;
            }

            string path = Path.Combine(_env.WebRootPath, _wwwrootDirectory, imageName);

            if (!File.Exists(path))
            {
                _logger.LogWarning("No processo de atualização da imagem do produto ID {id}, não foi encontrada no seguinte path: {Path}", request.Id, path);
                return;
            }
                
            File.Delete(path);
        }
    }
}
