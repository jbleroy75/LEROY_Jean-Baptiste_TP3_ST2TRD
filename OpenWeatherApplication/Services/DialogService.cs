using System.Windows;

namespace OpenWeatherApplication.Services
{
    public class DialogService : IDialogService
    {
        public bool ShowConfirmationRequest(string message, string caption = "")
        {
            var result = MessageBox.Show(message, caption, MessageBoxButton.OKCancel);
            return result.HasFlag(MessageBoxResult.OK);
        }

        public void ShowNotification(string message, string caption = "")
        {
            MessageBox.Show(message, caption);
        }
    }
}
