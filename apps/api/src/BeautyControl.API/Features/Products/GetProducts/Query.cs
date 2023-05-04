using BeautyControl.API.Features.Products._Common;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Products.GetProducts
{
    public record Query : IRequest<IEnumerable<ProductResponse>>;
}
