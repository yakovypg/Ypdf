using System.Text;
using iText.Kernel.Pdf;

namespace Ypdf.Core.Security;

public static class PasswordVerifier
{
    public static bool IsPasswordCorrect(string pdfFilePath, string password)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(pdfFilePath, nameof(pdfFilePath));
        ExtendedArgumentNullException.ThrowIfNull(password, nameof(password));
        DefaultExceptions.ThrowIfFileNotExists(pdfFilePath, nameof(pdfFilePath));

        byte[] passwordBytes = Encoding.Default.GetBytes(password);

        var props = new ReaderProperties();
        props.SetPassword(passwordBytes);

        try
        {
            using var reader = new PdfReader(pdfFilePath, props);
            using var pdfDocument = new PdfDocument(reader);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
