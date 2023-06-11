using BeautyControl.API.Domain.Products;

namespace BeautyControl.API.Features.Reports._Common
{
    public record ProductWorkflowDataResponse(int Id, string Name, Category Category, int Quantity);
}
