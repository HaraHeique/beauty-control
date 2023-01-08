using System.ComponentModel;

namespace BeautyControl.API.Features.Account.Common
{
    [DisplayName("LoggedUserResponse")]
    public record LoggedUserResponse
    {
        public string AccessToken { get; init; }
        public double ExpiresIn { get; init; }
        public UserToken TokenInfo { get; init; }

        public record UserToken
        {
            public int Id { get; init; }
            public string UserName { get; init; }
            public string Email { get; init; }
            public IEnumerable<UserClaim> Claims { get; init; }

            public record UserClaim
            {
                public string Type { get; init; }
                public string Value { get; init; }
            }
        }
    }
}
