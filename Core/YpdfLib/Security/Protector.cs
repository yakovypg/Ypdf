using iText.Kernel.Pdf;
using System.Text;
using YpdfLib.Models.Security;

namespace YpdfLib.Security
{
    public static class Protector
    {
        public static void SetPassword(string inputFile, string destPath, IPdfPassword password)
        {
            string userPassword = password.GetNotNullUserPassword();
            string ownerPassword = password.GetNotNullOwnerPassword();

            byte[] userPassBytes = Encoding.Default.GetBytes(userPassword);
            byte[] ownerPassBytes = Encoding.Default.GetBytes(ownerPassword);

            var props = new WriterProperties();

            props.SetStandardEncryption(
                userPassword: userPassBytes,
                ownerPassword: ownerPassBytes,
                permissions: EncryptionConstants.ALLOW_PRINTING,
                encryptionAlgorithm: password.EncryptionAlgorithm
            );

            var reader = new PdfReader(inputFile);
            var writer = new PdfWriter(new FileStream(destPath, FileMode.Create), props);

            var pdfDocument = new PdfDocument(reader, writer);
            pdfDocument.Close();
        }

        public static void RemovePassword(string inputFile, string destPath, IPdfPassword password)
        {
            string correctPassword;
            string userPassword = password.GetNotNullUserPassword();
            string ownerPassword = password.GetNotNullOwnerPassword();

            correctPassword = TryOpenPdf(inputFile, userPassword)
                ? userPassword
                : TryOpenPdf(inputFile, ownerPassword)
                ? ownerPassword
                : throw new IncorrectPasswordException(password);

            byte[] passwordBytes = Encoding.Default.GetBytes(correctPassword);

            var writer = new PdfWriter(destPath);

            var props = new ReaderProperties();
            props.SetPassword(passwordBytes);

            var reader = new PdfReader(inputFile, props);
            reader.SetUnethicalReading(true);

            var pdfDoc = new PdfDocument(reader, writer);
            pdfDoc.Close();
        }

        private static bool TryOpenPdf(string path, string password)
        {
            byte[] passwordBytes = Encoding.Default.GetBytes(password);

            var props = new ReaderProperties();
            props.SetPassword(passwordBytes);

            try
            {
                var pdfDoc = new PdfDocument(new PdfReader(path, props));
                pdfDoc.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
