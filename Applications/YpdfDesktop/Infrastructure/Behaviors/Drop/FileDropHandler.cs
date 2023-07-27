using Avalonia.Input;
using Avalonia.Xaml.Interactions.DragAndDrop;
using System.Linq;
using YpdfDesktop.Models.IO;

namespace YpdfDesktop.Infrastructure.Behaviors.Drop
{
    public class FileDropHandler : DropHandlerBase
    {
        public override bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            return Validate(e, sourceContext, targetContext, true);
        }

        public override bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            return Validate(e, sourceContext, targetContext, false);
        }

        private static bool Validate(DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute)
        {
            if (sourceContext is not null ||
                targetContext is not IFilePathContainer filePathContainer ||
                !e.Data.Contains(DataFormats.FileNames))
            {
                return false;
            }

            string? path = e.Data.GetFileNames()?.FirstOrDefault();

            if (path is null || !path.ToLower().EndsWith(".pdf"))
                return false;

            if (!bExecute)
                return true;

            filePathContainer.SetFilePath(path);
            return true;
        }
    }
}
