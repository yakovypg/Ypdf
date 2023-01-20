using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using System.Threading.Tasks;

namespace YpdfDesktop.Infrastructure.Communication
{
    internal class QuickMessage : IQuickMessage
    {
        public string Message { get; set; }

        public QuickMessage(string? message = null)
        {
            Message = message ?? string.Empty;
        }

        public Task<ButtonResult> Show(string title = "", ButtonEnum buttons = ButtonEnum.Ok, Icon icon = Icon.None)
        {
            return MessageBoxManager.GetMessageBoxStandardWindow(title, Message, buttons, icon).Show();
        }

        public Task<ButtonResult> ShowEmpty(ButtonEnum buttons = ButtonEnum.Ok)
        {
            return MessageBoxManager.GetMessageBoxStandardWindow(string.Empty, Message, buttons, Icon.None).Show();
        }

        public Task<ButtonResult> ShowError(ButtonEnum buttons = ButtonEnum.Ok)
        {
            return MessageBoxManager.GetMessageBoxStandardWindow("Error", Message, buttons, Icon.Error).Show();
        }

        public Task<ButtonResult> ShowWarning(ButtonEnum buttons = ButtonEnum.Ok)
        {
            return MessageBoxManager.GetMessageBoxStandardWindow("Warning", Message, buttons, Icon.Warning).Show();
        }

        public Task<ButtonResult> ShowQuestion(ButtonEnum buttons = ButtonEnum.YesNo)
        {
            return MessageBoxManager.GetMessageBoxStandardWindow("Question", Message, buttons, Icon.Question).Show();
        }

        public Task<ButtonResult> ShowInformation(ButtonEnum buttons = ButtonEnum.Ok)
        {
            return MessageBoxManager.GetMessageBoxStandardWindow("Info", Message, buttons, Icon.Info).Show();
        }
    }
}
