using System.Text.RegularExpressions;

namespace Devenant
{
    public static class FieldValidator
    {
        public static bool ValidateNickname(string nickname)
        {
            return Regex.IsMatch(nickname, "^(?=.*?[A-Z])(?=.*?[a-z]).{2,}$");
        }

        public static bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, "^\\S+@\\S+\\.\\S+$");
        }

        public static bool ValidatePassword(string password)
        {
            return Regex.IsMatch(password, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$");
        }
    }
}
