namespace YpdfLib.Models.Security
{
    public interface IPdfPassword : IDeepCloneable<IPdfPassword>
    {
        string? OwnerPassword { get; set; }
        string? UserPassword { get; set; }
        int EncryptionAlgorithm { get; set; }

        string? MasterPassword { get; }

        void SetCommonPassword(string password);
        string GetNotNullUserPassword();
        string GetNotNullOwnerPassword();
        string GetNotNullMasterPassword();
    }
}