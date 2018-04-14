using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Site_Manager
{
    public sealed partial class EditWebPage : Page
    {

        private static ManagedWebPage Page;

        public EditWebPage() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e) => LoadDetails();

        private void LoadDetails()
        {
            Page = WebPageManager.GetSelectedPage();

            TitleTextBox.Text = Page.Title;
            RelativeURLTextBlock.Text = Page.RelativeURL;
            ToolTipService.SetToolTip(RelativeURLTextBlock, new ToolTip() { Content = Page.RelativeURL });
            IsRootTextBlock.Text = Page.IsRoot.ToString().ToLower();
            LastUpdatedTextBlock.Text = Page.GetLastUpdatedAsString();
            LastSubmittedTextBlock.Text = Page.GetLastSubmittedAsString();
            ContentHTMLTextBox.Text = Page.ContentHTML;
            HeaderHTMLTextBox.Text = Page.AdditionalHeaderHTML;
        }

        private void OkayButton_Click(object sender, RoutedEventArgs e)
        {
            // go back to home page with drill animation
            Frame.Navigate(typeof(Home), null, new DrillInNavigationTransitionInfo());
        }

        private async void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            Page.Updated();
            Page.Title = TitleTextBox.Text;

            string t = ContentHTMLTextBox.Text;
            Page.ContentHTML = t;

            t = HeaderHTMLTextBox.Text;
            Page.AdditionalHeaderHTML = t;

            WebPageManager.SetSelectedPage(Page);
            await WebPageManager.Save();

            LoadDetails();
        }

        private async void SublimeTextButton_Click(object sender, RoutedEventArgs e)
        {
            string contentHTML = ContentHTMLTextBox.Text;
            string headerHTML = HeaderHTMLTextBox.Text;

            if (contentHTML.Length != 0 && headerHTML.Length != 0)
            {
                string name = Utils.RandomString(8);
                while (await FileManager.GetExists(ApplicationData.Current.TemporaryFolder, name + ".txt"))
                {
                    name = Utils.RandomString(8); // ensure file with same name doesn't already exist (next to zero chance)
                }
                await Windows.System.Launcher.LaunchFileAsync(await FileManager.CreateTemporaryFile(contentHTML, name + ".txt"));
            }
        }

        private async void DeployButton_Click(object sender, RoutedEventArgs e)
        {
            WebPageManager.SetSelectedPage(Page);
            await WebPageManager.Save();

            DeployDialog dialog = new DeployDialog(WebPageManager.GetSelectedPage());
            await dialog.ShowAsync();

            LoadDetails();
        }

        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog confirmation = new ContentDialog
            {
                Title = "Are you sure?",
                Content = "Once deleted, this page and its contents cannot be recovered",
                PrimaryButtonText = "Nevermind",
                SecondaryButtonText = "I'm sure"
            };
            if (await confirmation.ShowAsync() == ContentDialogResult.Primary)
            {
                // nevermind

                // dismiss dialog and do nothing else
                confirmation.Hide();
            }
            else
            {
                // i'm sure

                // actually remove the page
                WebPageManager.Pages.Remove(Page);
                await WebPageManager.Save();
                // dismiss dialog
                confirmation.Hide();
                // go back home with drill animation
                Frame.Navigate(typeof(Home), null, new DrillInNavigationTransitionInfo());
            }
        }
    }
}