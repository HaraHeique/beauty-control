#nullable disable
namespace BeautyControl.API.Features.Account._Common
{
    public record LoggedUserResponse
    {
        public string AccessToken { get; init; }
        public double ExpiresInSeconds { get; init; }
        public UserTokenResponse TokenInfo { get; init; }
    }

    public record UserTokenResponse
    {
        public int Id { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public IEnumerable<UserClaimResponse> Claims { get; init; }
    }

    public record UserClaimResponse
    {
        public string Type { get; init; }
        public string Value { get; init; }
    }
}
