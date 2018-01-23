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
            Opened += async (s, args) => { await Deploy(); };
        }

        private void Log(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            LogTextBlock.Text += message + " \n";
            LogScrollViewer.ChangeView(0, LogScrollViewer.ScrollableHeight, 1);
        }

        public async Task Deploy()
        {
            DateTime start = DateTime.Now;
            double interval = 100.0 / Pages.Count;
            int  i = 1;
            DeployProgressBar.Value = 0;

            foreach (ManagedWebPage page in Pages)
            {
                page.Submitted();
            }

            await FTPManager.Connect();
            foreach (ManagedWebPage page in Pages)
            {
                string path = page.RelativeURL;
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
                    Progress<double> progress = new Progress<double>(x =>
                    {
                        if (x < 100)
                        {
                            LogTextBlock.Text = LogTextBlock.Text.Replace(msg, msg + " (" + x + "% complete)");
                        }
                        else
                        {
                            LogTextBlock.Text = LogTextBlock.Text.Replace("(Uploading...)", "(Finished)");
                        }
                    });

                    StorageFile file = await FileManager.CreateTemporaryFile(HTMLBuilder.GetFullPageHTML(page));
                    Debug.Out("Uploading... (" + file.Path + " to " + path + ")");
                    System.IO.FileStream stream = System.IO.File.OpenRead(file.Path);
                    await FTPManager.Client.UploadAsync(stream, path + "/" + file.Name, FluentFTP.FtpExists.Overwrite, true, System.Threading.CancellationToken.None, progress);
                    stream.Dispose();
                    await file.DeleteAsync();
                }

                DeployProgressBar.Value += interval;
                i++;
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