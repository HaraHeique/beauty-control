using BeautyControl.API.Domain.Products;
using BeautyControl.API.Features.Products._Common.Uploads;
using MediatR;

namespace BeautyControl.API.Features.Products._EventHandlers
{
    public class DeleteImageWhenProductDeletedEventHandler : INotificationHandler<ProductDeletedEvent>
    {
        private readonly ImageUploadManager _imageManager;

        public DeleteImageWhenProductDeletedEventHandler(ImageUploadManager imageManager)
        {
            _imageManager = imageManager;
        }

        // TODO: O ideal mesmo é realizar esse processamento de forma assíncrona em segundo plano (thread separada). O NotificationHandler do MediatR trabalha com event bus em memória e é SÍNCRONO. Ou seja, se ocorrer um erro/exceção aqui dentro vai propagar o erro para todas outros NotificationHandler e para o RequestHandler que o chamou.
        public Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
        {
            notification.Deconstruct(out Product product);

            if (product.Image is not null)
                _imageManager.DeleteImage(product.Id, product.Image);

            return Task.CompletedTask;
        }
    }
}
