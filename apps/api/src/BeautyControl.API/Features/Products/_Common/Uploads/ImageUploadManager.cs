using BeautyControl.API.Domain.Products;
using BeautyControl.API.Features._Common.Users;
using FluentResults;

namespace BeautyControl.API.Features.Products._Common.Uploads
{
    public class ImageUploadManager
    {
        private const string _wwwrootDirectory = "images";

        private readonly ILogger<ImageUploadManager> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly CurrentUser _currentUser;

        public ImageUploadManager(
            ILogger<ImageUploadManager> logger,
            IWebHostEnvironment env,
            CurrentUser currentUser)
        {
            _logger = logger;
            _env = env;
            _currentUser = currentUser;
        }

        public async Task<Result> UploadImage(IImageUploadRequest request, CancellationToken cancellationToken)
        {
            if (request.ImageUpload is null || request.ImageUpload.Length == 0)
                return Result.Fail("É obrigatório fornecer uma imagem para este produto.")
                    .LogIfFailed<ImageUploadManager>();

            var imageName = $"{Guid.NewGuid()}_{request.ImageUpload.FileName}";
            var filePath = Path.Combine(_env.WebRootPath, _wwwrootDirectory, imageName);

            if (File.Exists(filePath))
                return Result.Fail($"Já existe uma imagem com o nome {imageName}.")
                    .LogIfFailed<ImageUploadManager>();

            using var stream = new FileStream(filePath, FileMode.Create);
            await request.ImageUpload.CopyToAsync(stream, cancellationToken);

            _logger.LogInformation("Upload da imagem {imageName} ({filePath}) realizada com sucesso!", imageName, filePath);

            var host = $"{_currentUser.HttpContext?.Request.Scheme}://{_currentUser.HttpContext?.Request.Host}";
            request.ImageUrlUpload = $"{host}/{_wwwrootDirectory}/{imageName}";
            request.Image = imageName;

            return Result.Ok();
        }

        public bool DeleteImage(int productId, Image? image)
        {
            if (image is null)
            {
                _logger.LogWarning("Não é possível deletar uma imagem nula para o produto ID {id}", productId);
                return false;
            }

            if (!Image.Validate(image.Name, image.Url))
            {
                _logger.LogWarning("Não é possível deletar imagem {@Imagem} do produto {Id} com campos inválidos.", image, productId);
                return false;
            }

            string path = Path.Combine(_env.WebRootPath, _wwwrootDirectory, image.Name);

            if (!File.Exists(path))
            {
                _logger.LogWarning("Não foi encontrada a imagem do produto ID {id}. Path: {Path}", productId, path);
                return false;
            }

            File.Delete(path);

            return true;
        }
    }
}