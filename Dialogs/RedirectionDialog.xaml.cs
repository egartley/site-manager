using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Site_Manager
{
    public sealed partial class RedirectionDialog : ContentDialog
    {

        private string URL, Destination;
        private ChangeRedirectionDialog ChangeDialog;

        public RedirectionDialog(string url, ChangeRedirectionDialog changeDialog)
        {
            InitializeComponent();
            Title += url;
            URL = url;
            ChangeDialog = changeDialog;
            FetchDestination(url);
        }

        private async void FetchDestination(string url)
        {
            DestinationTextBlock.Visibility = Visibility.Collapsed;
            DestinationProgressBar.Visibility = Visibility.Visible;
            // get the actual html file
            StorageFile file = await FTPManager.Download("/go/" + url + "/", "index.html", true);
            string contents = await FileManager.GetFileContents(file), delimiter = "window.location=\"";
            string extractedDestination = "";
            if (contents.IndexOf(delimiter) != -1)
            {
                // extract destination
                extractedDestination = contents.Substring(contents.IndexOf(delimiter) + delimiter.Length);
            }
            else
            {
                // something went wrong
                extractedDestination = "There was an error while parsing\"";
            }
            // update UI
            DestinationTextBlock.Text = extractedDestination.Substring(0, extractedDestination.IndexOf("\""));
            Destination = DestinationTextBlock.Text;
            DestinationTextBlock.Visibility = Visibility.Visible;
            DestinationProgressBar.Visibility = Visibility.Collapsed;
            IsPrimaryButtonEnabled = true;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Change
            Hide();
            ChangeDialog.SetDestination(Destination);
            await ChangeDialog.ShowAsync();
        }

        private async void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // OK
            if (FTPManager.Connected)
            {
                // disconnect if still connected
                await FTPManager.Disconnect();
            }
            Hide();
        }
    }
}