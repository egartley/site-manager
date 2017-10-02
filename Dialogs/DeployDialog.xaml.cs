using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Site_Manager
{
    public sealed partial class DeployDialog : ContentDialog
    {

        private ObservableCollection<ManagedWebPage> Pages;

        public DeployDialog(ManagedWebPage page)
        {
            InitializeComponent();
            Pages = new ObservableCollection<ManagedWebPage>() { page };

            Opened += async (s, args) => { await Deploy(); };
        }

        public DeployDialog(ObservableCollection<ManagedWebPage> pages)
        {
            InitializeComponent();
            Pages = pages;

            Opened += async (s, args) => { await Deploy(); };
        }

        private void Log(string message)
        {
            LogTextBlock.Text += message + " \n";
            LogScrollViewer.ChangeView(0, LogScrollViewer.ScrollableHeight, 1);
        }

        public async Task Deploy()
        {
            DateTime start = DateTime.Now;
            int interval = 100 / Pages.Count, i = 1;
            DeployProgressBar.Value = 0;
            
            foreach (ManagedWebPage page in Pages)
                page.Submitted();

            await FTPManager.ConnectAsync();

            foreach (ManagedWebPage page in Pages)
            {
                string path = "/test" + page.RelativeURL;
                string percent = DeployProgressBar.Value.ToString() + "%";
                string msg = "Deploying \"" + page.Title + "\"...";

                StatusTextBlock.Text = percent + " complete (" + i + " of " + Pages.Count + ")";

                Log(msg);
                StorageFile file = await FileManager.CreateTemporaryFile(HTMLBuilder.GetFullPageHTML(page));
                
                await FTPManager.UploadAsync(file, path);
                await file.DeleteAsync();

                DeployProgressBar.Value += interval;
                i++;

                LogTextBlock.Text = LogTextBlock.Text.Replace(msg, msg + " (Done!)");
            }

            await FTPManager.DisconnectAsync();

            WebPageManager.Save();

            string timeTaken = DateTime.Now.Subtract(start).TotalSeconds.ToString("0.0");

            DeployProgressBar.Value = 100;
            StatusTextBlock.Text = "100% complete (" + timeTaken + " seconds)";
            IsPrimaryButtonEnabled = true;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // OK
            Hide();
        }
    }
}