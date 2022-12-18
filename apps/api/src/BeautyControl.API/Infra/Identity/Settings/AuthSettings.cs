#nullable disable
namespace BeautyControl.API.Infra.Identity.Settings
{
    public class AuthSettings
    {
        public const string Key = "AuthSettings";

        public int ExpiresInHours { get; set; }
        public string Issuer { get; set; }
        public string[] ValidOn { get; set; }
    }
}
