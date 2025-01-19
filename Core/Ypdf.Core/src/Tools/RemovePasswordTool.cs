using System.Text;
using iText.Kernel.Pdf;
using Ypdf.Core.Security;

namespace Ypdf.Core.Tools;

public class RemovePasswordTool : ITool
{
    public RemovePasswordTool(PdfPassword password)
    {
        Password = password;
    }

    protected PdfPassword Password { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        string password = PasswordVerifier.IsPasswordCorrect(inputPath, Password.OwnerPassword)
            ? Password.OwnerPassword
            : PasswordVerifier.IsPasswordCorrect(inputPath, Password.UserPassword)
            ? Password.UserPassword
            : throw new IncorrectPasswordException(null, Password);

        byte[] passwordBytes = Encoding.Default.GetBytes(password);

        using var writer = new PdfWriter(outputPath);

        var readerProperties = new ReaderProperties();
        readerProperties.SetPassword(passwordBytes);

        using var reader = new PdfReader(inputPath, readerProperties);
        reader.SetUnethicalReading(true);

        using var pdfDocument = new PdfDocument(reader, writer);
    }
}
