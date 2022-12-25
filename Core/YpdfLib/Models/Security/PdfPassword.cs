using iText.Kernel.Pdf;

namespace YpdfLib.Models.Security
{
    public class PdfPassword : IPdfPassword, IDeepCloneable<PdfPassword>, IEquatable<PdfPassword?>
    {
        public string? UserPassword { get; set; }
        public string? OwnerPassword { get; set; }
        public int EncryptionAlgorithm { get; set; }

        public string? MasterPassword => OwnerPassword ?? UserPassword;

        public PdfPassword(string? userPassword = null, string? ownerPassword = null, int encryptionAlgorithm = EncryptionConstants.ENCRYPTION_AES_128)
        {
            UserPassword = userPassword;
            OwnerPassword = ownerPassword;
            EncryptionAlgorithm = encryptionAlgorithm;
        }

        public void SetCommonPassword(string password)
        {
            UserPassword = OwnerPassword = password;
        }

        public string GetNotNullUserPassword()
        {
            return UserPassword ?? string.Empty;
        }

        public string GetNotNullOwnerPassword()
        {
            return OwnerPassword ?? string.Empty;
        }

        public string GetNotNullMasterPassword()
        {
            return MasterPassword ?? string.Empty;
        }

        public PdfPassword Copy()
        {
            return new PdfPassword(UserPassword, OwnerPassword, EncryptionAlgorithm);
        }

        IPdfPassword IDeepCloneable<IPdfPassword>.Copy()
        {
            return Copy();
        }

        public bool Equals(PdfPassword? other)
        {
            return other is not null &&
                   UserPassword == other.UserPassword &&
                   OwnerPassword == other.OwnerPassword &&
                   EncryptionAlgorithm == other.EncryptionAlgorithm;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PdfPassword);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserPassword, OwnerPassword, EncryptionAlgorithm);
        }

        public static bool operator ==(PdfPassword? left, PdfPassword? right)
        {
            return EqualityComparer<PdfPassword>.Default.Equals(left, right);
        }

        public static bool operator !=(PdfPassword? left, PdfPassword? right)
        {
            return !(left == right);
        }
    }
}
