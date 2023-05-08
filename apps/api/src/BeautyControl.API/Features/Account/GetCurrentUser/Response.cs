#nullable disable
namespace BeautyControl.API.Features.Account.GetCurrentUser
{
    public record CurrentUserResponse
    {
        public int Id { get; init; }
        public string Email { get; init; }
        public string UserName { get; init; }
        public IEnumerable<string> Roles { get; init; }
        public IEnumerable<CurrentUserClaimResponse> Claims { get; init; }
    }

    public record CurrentUserClaimResponse
    {
        public string Type { get; init; }
        public string[] Values { get; init; }
    }
}
