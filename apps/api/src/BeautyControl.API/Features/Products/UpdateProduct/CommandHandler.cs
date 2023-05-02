using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Domain.Products;
using BeautyControl.API.Infra.Data;
using FluentResults;
using MediatR;

namespace BeautyControl.API.Features.Products.UpdateProduct
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly AppDataContext _context;

        public CommandHandler(AppDataContext context) => _context = context;

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var productDb = await _context.Products.FindAsync(request.Id);

            if (productDb is null) return Result.Fail(new NotFoundError());

            Image? image = GetNewImage(request);

            productDb!.UpdateBasicInfo(
                request.Name, request.Description, image,
                request.RunningOutOfStock, request.Category
            );

            _context.Products.Update(productDb);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }

        private static Image? GetNewImage(Command request)
        {
            Image? image = null;

            if (Image.Validate(request.Image, request.ImageUrlUpload))
                image = new Image(request.Image!, request.ImageUrlUpload!);

            return image;
        }
    }
}
