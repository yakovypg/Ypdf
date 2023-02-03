using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactions.DragAndDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YpdfDesktop.Models.IO;
using YpdfDesktop.ViewModels.Pages.Tools;

namespace YpdfDesktop.Infrastructure.Behaviors
{
    public class FileToSplitDropHandler : DropHandlerBase
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
                targetContext is not SplitViewModel viewModel ||
                !e.Data.Contains(DataFormats.FileNames))
            {
                return false;
            }

            string? path = e.Data.GetFileNames()?.FirstOrDefault();

            if (path is null || !path.ToLower().EndsWith(".pdf"))
                return false;

            if (!bExecute)
                return true;

            viewModel.SetFilePath(path);
            return true;
        }
    }
}
