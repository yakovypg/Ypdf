using System.IO;
using System.Text;
using iText.Kernel.Pdf;
using Ypdf.Core.Security;

namespace Ypdf.Core.Tools;

public class SetPasswordTool : ITool
{
    public SetPasswordTool(PdfPassword password)
    {
        Password = password;
    }

    protected PdfPassword Password { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        byte[] userPassword = Encoding.Default.GetBytes(Password.UserPassword);
        byte[] ownerPassword = Encoding.Default.GetBytes(Password.OwnerPassword);

        var writerProperties = new WriterProperties();

        writerProperties.SetStandardEncryption(
            userPassword: userPassword,
            ownerPassword: ownerPassword,
            permissions: EncryptionConstants.ALLOW_PRINTING,
            encryptionAlgorithm: Password.EncryptionAlgorithm);

        var outputStream = new FileStream(outputPath, FileMode.Create);

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputStream, writerProperties);
        using var pdfDocument = new PdfDocument(reader, writer);
    }
}
