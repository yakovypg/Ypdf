using YpdfLib.Models;

namespace ExecutionLib.Configuration
{
    public interface IYpdfGlobalConfig : IDeepCloneable<IYpdfGlobalConfig>
    {
        string? PythonAlias { get; set; }

        string[] GetItems();
        void Reset(bool saveChanges = false);
        void Save();
    }
}
