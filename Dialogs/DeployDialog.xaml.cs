using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Site_Manager
{
    public sealed partial class DeployDialog : ContentDialog
    {

        private ObservableCollection<ManagedWebPage> DeployPages;

        public DeployDialog(ManagedWebPage page)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }
            InitializeComponent();
            DeployPages = new ObservableCollection<ManagedWebPage>() { page };
            Opened += async (s, args) => { await Deploy(); };
        }

        public DeployDialog(ObservableCollection<ManagedWebPage> pages)
        {
            InitializeComponent();
            DeployPages = pages ?? throw new ArgumentNullException(nameof(pages));
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
            double interval = 100.0 / DeployPages.Count;
            int i = 1;
            DeployProgressBar.Value = 0;

            bool test = DeveloperOptions.GetUseTestDirectory();
            bool blank = DeveloperOptions.GetBlankDeploy();

            if (DeveloperOptions.GetLocalDeploy())
            {
                StorageFolder deployFolder = null;
                if (await FileManager.GetExists(ApplicationData.Current.LocalFolder, "deploy"))
                {
                    deployFolder = (StorageFolder)(await ApplicationData.Current.LocalFolder.TryGetItemAsync("deploy"));
                    await deployFolder.DeleteAsync();
                }
                deployFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("deploy");

                foreach (ManagedWebPage page in DeployPages)
                {
                    StatusTextBlock.Text = $"{(DeployProgressBar.Value.ToString("0.00") + "%")} complete ({i} of {DeployPages.Count})";
                    Log($"\"{page.Title}\"... (Creating...)");

                    StorageFile indexHtml = await FileManager.CreateTemporaryFile(HTMLBuilder.GetFullPageHTML(page));
                    StorageFolder pageFolder = deployFolder, prev = deployFolder;
                    if (page.RelativeURL.Length > 1)
                    {
                        // recursively create folders
                        String[] dirs = page.RelativeURL.Split('/');
                        for (int ii = 0; ii < dirs.Length; ii++)
                        {
                            if (dirs[ii].Equals(""))
                                continue;
                            StorageFolder temp = prev;
                            if (await prev.TryGetItemAsync(dirs[ii]) == null)
                            {
                                prev = await temp.CreateFolderAsync(dirs[ii]);
                            }
                            else
                            {
                                prev = (StorageFolder)await temp.TryGetItemAsync(dirs[ii]);
                            }
                            pageFolder = prev;
                        }
                    }

                    // copy from temp to deploy
                    if (pageFolder != null)
                        await indexHtml.CopyAsync(pageFolder); // not home page, and needs its own directory
                    else
                        await indexHtml.CopyAsync(deployFolder); // must be home page

                    await indexHtml.DeleteAsync();

                    LogTextBlock.Text = LogTextBlock.Text.Replace("(Creating...)", "(Done)");
                    DeployProgressBar.Value += interval;
                    i++;
                    page.Submitted();
                }
                // save because submitted times were updated
                await WebPageManager.Save();

                DeployProgressBar.Value = 100;
                StatusTextBlock.Text = $"Finished ({(DateTime.Now.Subtract(start).TotalSeconds.ToString("0.0"))} seconds)";
                IsPrimaryButtonEnabled = true;

                Log("Opening in File Explorer...");
                await Windows.System.Launcher.LaunchFolderAsync(deployFolder);

                return;
            }

            await FTPManager.Connect();
            // await Task.Delay(5000);
            // await FTPManager.Disconnect();
            Debug.Out("Connected!", "DEPLOY DIALOG");
            foreach (ManagedWebPage page in DeployPages)
            {
                string path = page.RelativeURL;
                if (test)
                {
                    path = $"/test{path}";
                }
                string percent = DeployProgressBar.Value.ToString("0.00") + "%";
                string msg = "\"" + page.RelativeURL + "\"... (Uploading...)";

                StatusTextBlock.Text = percent + " complete (" + i + " of " + DeployPages.Count + ")";

                Log(msg);
                if (!blank)
                {
                    StorageFile file = await FileManager.CreateTemporaryFile(HTMLBuilder.GetFullPageHTML(page));
                    await FTPManager.UploadFile(file, path);
                    await file.DeleteAsync();
                    LogTextBlock.Text = LogTextBlock.Text.Replace("(Uploading...)", "(Finished)");
                }

                DeployProgressBar.Value += interval;
                i++;

                page.Submitted();
            }
            await FTPManager.Disconnect();

            // save because submitted times were updated
            await WebPageManager.Save();

            string timeTaken = DateTime.Now.Subtract(start).TotalSeconds.ToString("0.0");

            DeployProgressBar.Value = 100;
            StatusTextBlock.Text = "Finished (" + timeTaken + " seconds)";
            IsPrimaryButtonEnabled = true;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (FTPManager.Connected == true)
            {
                await FTPManager.Disconnect();
            }

            Hide();
        }
    }
}