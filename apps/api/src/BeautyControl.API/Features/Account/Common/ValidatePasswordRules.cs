using System.Text.RegularExpressions;

namespace BeautyControl.API.Features.Account.Common
{
    public class ValidatePasswordRules
    {
        public static bool HasValidPassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasMinAndMaxChars = new Regex(@".{8,15}");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            return hasNumber.IsMatch(password) &&
                   hasUpperChar.IsMatch(password) &&
                   hasLowerChar.IsMatch(password) &&
                   hasMinAndMaxChars.IsMatch(password) &&
                   hasSymbols.IsMatch(password);
        }
    }
}
