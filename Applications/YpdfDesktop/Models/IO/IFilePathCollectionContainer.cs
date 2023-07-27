using System.Collections.ObjectModel;

namespace YpdfDesktop.Models.IO
{
    public interface IFilePathCollectionContainer
    {
        ObservableCollection<InputFile> InputFiles { get; }
    }
}