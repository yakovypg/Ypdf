using YpdfLib.Models.Security;

namespace YpdfLib.Security
{
    public class IncorrectPasswordException : ApplicationException
    {
        public IPdfPassword Password { get; }

        public IncorrectPasswordException(string password, string? message = null, Exception? innerException = null) :
            base(message ?? GetDefaultMessage(password), innerException)
        {
            var pdfPassword = new PdfPassword();
            pdfPassword.SetCommonPassword(password);

            Password = pdfPassword;
        }

        public IncorrectPasswordException(IPdfPassword password, string? message = null, Exception? innerException = null) :
            base(message ?? GetDefaultMessage(password), innerException)
        {
            Password = password;
        }

        private static string GetDefaultMessage(string password)
        {
            return $"Password {password} is incorrect.";
        }

        private static string GetDefaultMessage(IPdfPassword password)
        {
            var passwords = new List<string>();

            if (password.UserPassword is not null)
                passwords.Add(password.UserPassword);

            if (password.OwnerPassword is not null)
                passwords.Add(password.OwnerPassword);

            return passwords.Count switch
            {
                0 => "Password is incorrect.",
                1 => GetDefaultMessage(passwords[0]),
                _ => $"Passwords {string.Join(" and ", passwords)} are incorrect."
            };
        }
    }
}
