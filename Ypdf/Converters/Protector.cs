using iText.Kernel.Pdf;
using System.Text;
using Ypdf.Security;

namespace Ypdf.Converters
{
    public static class Protector
    {
        public static void SetPassword(string destPath, string inputPath, string userPassword, string ownerPassword, int encryptionAlgorithm = EncryptionConstants.ENCRYPTION_AES_128)
        {
            byte[] userPassBytes = Encoding.Default.GetBytes(userPassword);
            byte[] ownerPassBytes = Encoding.Default.GetBytes(ownerPassword);

            var props = new WriterProperties();

            props.SetStandardEncryption(
                userPassword: userPassBytes,
                ownerPassword: ownerPassBytes,
                permissions: EncryptionConstants.ALLOW_PRINTING,
                encryptionAlgorithm: encryptionAlgorithm
            );

            var reader = new PdfReader(inputPath);
            var writer = new PdfWriter(new FileStream(destPath, FileMode.Create), props);

            var pdfDocument = new PdfDocument(reader, writer);
            pdfDocument.Close();
        }

        public static void RemovePassword(string destPath, string inputPath, string password)
        {
            byte[] passwordBytes = Encoding.Default.GetBytes(password);

            var writer = new PdfWriter(destPath);

            var props = new ReaderProperties();
            props.SetPassword(passwordBytes);

            var reader = new PdfReader(inputPath, props);
            reader.SetUnethicalReading(true);

            var pdfDoc = new PdfDocument(reader, writer);
            pdfDoc.Close();
        }

        public static bool CrackPassword(string inputPath, IPasswordGenerator passwordGenerator, out string? password)
        {
            password = null;
            bool success = false;

            while (!passwordGenerator.IsOver)
            {
                string? currPass = passwordGenerator.Next() ?? string.Empty;
                bool isPasswordCorrect = TryOpenPdf(inputPath, currPass);

                if (isPasswordCorrect)
                {
                    password = currPass;
                    success = true;
                    break;
                }
            }

            return success;
        }

        private static bool TryOpenPdf(string path, string password)
        {
            byte[] passwordBytes = Encoding.Default.GetBytes(password);

            var props = new ReaderProperties();
            props.SetPassword(passwordBytes);

            try
            {
                var reader = new PdfReader(path, props);
                var pdfDoc = new PdfDocument(reader);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
