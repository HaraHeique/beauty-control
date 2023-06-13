using BeautyControl.API.Domain._Common.ValueObjects;
using BeautyControl.API.Domain.Suppliers;
using BeautyControl.API.Features.Suppliers._Common;
using Newtonsoft.Json;

#nullable disable
namespace BeautyControl.API.Features.Suppliers.GetSuppliers.V2
{
    public record SupplierDataModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Observation { get; init; }
        public string TelephonesJson { get; init; }
        public string EmailsJson { get; init; }
        public decimal AverageRating { get; init; }
    }

    public static class SupplierDataModelMapping 
    {
        public static IEnumerable<SupplierResponse> MapToResponse(this IEnumerable<SupplierDataModel> source)
        {
            return source.Select(item =>
            {
                var emails = item.EmailsJson != null ? JsonConvert.DeserializeObject<IEnumerable<Email>>(item.EmailsJson) : null;
                var telephones = item.TelephonesJson != null ? JsonConvert.DeserializeObject<IEnumerable<Telephone>>(item.TelephonesJson) : null;

                return new SupplierResponse(telephones, emails)
                {
                    Id = item.Id,
                    Name = item.Name,
                    AverageRating = item.AverageRating,
                    Observation = item.Observation
                };
            });
        }
    }
}
