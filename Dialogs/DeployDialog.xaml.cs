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
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }
            InitializeComponent();
            Pages = new ObservableCollection<ManagedWebPage>() { page };
            Opened += async (s, args) => { await Deploy(); };
        }

        public DeployDialog(ObservableCollection<ManagedWebPage> pages)
        {
            InitializeComponent();
            Pages = pages ?? throw new ArgumentNullException(nameof(pages));
            Opened += async (s, args) =>
            {
                await Deploy();
            };
        }

        private void Log(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            LogTextBlock.Text += message + " \n";
            // auto scroll
            LogScrollViewer.ChangeView(0, LogScrollViewer.ScrollableHeight, 1);
        }

        private async Task Deploy()
        {
            DateTime start = DateTime.Now;
            double interval = 100.0 / Pages.Count;
            int i = 1;
            DeployProgressBar.Value = 0;

            foreach (ManagedWebPage page in Pages)
            {
                page.Submitted();
            }

            await FTPManager.Connect();
            foreach (ManagedWebPage page in Pages)
            {
                string path = page.RelativeURL;
                System.Diagnostics.Debug.WriteLine("deploying \"" + path + "\"");
                if (DeveloperOptions.GetUseTestDirectory())
                {
                    path = $"/test{path}";
                }
                string percent = DeployProgressBar.Value.ToString("0.00") + "%";
                string msg = "\"" + page.RelativeURL + "\"... (Uploading...)";

                StatusTextBlock.Text = percent + " complete (" + i + " of " + Pages.Count + ")";

                Log(msg);
                if (!DeveloperOptions.GetBlankDeploy())
                {
                    System.Diagnostics.Debug.WriteLine("create temp file");
                    StorageFile file = await FileManager.CreateTemporaryFile(HTMLBuilder.GetFullPageHTML(page));
                    Debug.Out("Uploading... (" + file.Path + " to " + path + ")");
                    System.Diagnostics.Debug.WriteLine("upload");
                    await FTPManager.UploadFile(file, path);
                    System.Diagnostics.Debug.WriteLine("remove temp file");
                    await file.DeleteAsync();
                    LogTextBlock.Text = LogTextBlock.Text.Replace("(Uploading...)", "(Finished)");
                }

                DeployProgressBar.Value += interval;
                i++;
                await Task.Delay(200);
            }
            await FTPManager.Disconnect();

            // save because submitted times were updated
            await WebPageManager.Save();

            string timeTaken = DateTime.Now.Subtract(start).TotalSeconds.ToString("0.0");

            DeployProgressBar.Value = 100;
            StatusTextBlock.Text = "All done! (" + timeTaken + " seconds)";
            IsPrimaryButtonEnabled = true;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // OK
            Hide();
        }
    }
}