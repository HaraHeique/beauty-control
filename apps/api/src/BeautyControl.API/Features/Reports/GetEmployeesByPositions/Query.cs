using BeautyControl.API.Domain.Employees;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

#nullable disable
namespace BeautyControl.API.Features.Reports.GetEmployeesByPositions
{
    [DisplayName("GetEmployeesByPositionsRequest")]
    public record Query(bool? Active);

    public record EmployeeReportResponse(
        IEnumerable<EmployeeReportDataResponse> Salesmans,
        IEnumerable<EmployeeReportDataResponse> Managers
    );

    public record EmployeeReportDataResponse
    {
        public string Name { get; init; }
        public string Email { get; init; }
        public bool Active { get; init; }


        [JsonIgnore]
        [SwaggerSchema(ReadOnly = true, Description = "Usado somente para fazer o agrupamento por Position em memória.")]
        public Position Position { get; set; }
    }
}
