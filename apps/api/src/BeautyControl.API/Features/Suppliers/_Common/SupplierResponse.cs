using BeautyControl.API.Domain._Common.ValueObjects;
using BeautyControl.API.Domain.Suppliers;

#nullable disable
namespace BeautyControl.API.Features.Suppliers._Common
{
    public record SupplierResponse
    {
        public SupplierResponse() { }

        public SupplierResponse(IEnumerable<Telephone>? telephones, IEnumerable<Email>? emails)
        {
            Telephones = telephones?.Select(x => x.FormattedNumber) ?? Enumerable.Empty<string>();
            Emails = emails?.Select(x => x.Address) ?? Enumerable.Empty<string>();
        }

        public int Id { get; init; }
        public string Name { get; init; }
        public string Observation { get; init; }
        public IEnumerable<string> Telephones { get; init; }
        public IEnumerable<string> Emails { get; init; }
        public decimal AverageRating { get; init; }
    }
}
