using System.Collections.ObjectModel;
using YpdfDesktop.Models.Paging;

namespace YpdfDesktop.Models.IO
{   
    public interface IPageCollectionContainer
    {
        ObservableCollection<PageInfo> Pages { get; }
    }
}
