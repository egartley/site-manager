using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Site_Manager
{
    public sealed partial class RedirectionDialog : ContentDialog
    {

        string url, destination;
        ChangeRedirectionDialog changeDialog;

        public RedirectionDialog(string url, ChangeRedirectionDialog changeDialog)
        {
            InitializeComponent();
            Title += url;
            this.url = url;
            this.changeDialog = changeDialog;
            FetchDestination(url);
        }

        private async void FetchDestination(string url)
        {
            DestinationTextBlock.Visibility = Visibility.Collapsed;
            DestinationProgressBar.Visibility = Visibility.Visible;

            StorageFile file = await FTPManager.DownloadAsync("/go/" + url + "/", "index.html", true); // get the actual html file
            string contents = await FileManager.GetFileContents(file), delimiter = "window.location=\"";
            string d = "";
            if (contents.IndexOf(delimiter) != -1)
                d = contents.Substring(contents.IndexOf(delimiter) + delimiter.Length); // extract destination
            else
                d = "There was an error while parsing\"";
            DestinationTextBlock.Text = d.Substring(0, d.IndexOf("\""));
            destination = DestinationTextBlock.Text;

            DestinationTextBlock.Visibility = Visibility.Visible;
            DestinationProgressBar.Visibility = Visibility.Collapsed;
            IsPrimaryButtonEnabled = true;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Change
            Hide();
            changeDialog.SetDestination(destination);
            await changeDialog.ShowAsync();
        }

        private async void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // OK
            if (FTPManager.Connected)
                await FTPManager.DisconnectAsync();

            Hide();
        }
    }
}