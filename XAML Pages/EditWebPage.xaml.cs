using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Site_Manager
{
    public sealed partial class EditWebPage : Page
    {

        private static ManagedWebPage Page;
        private static bool MadeChanges = false;

        public EditWebPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDetails();
        }

        private void LoadDetails()
        {
            Page = WebPageManager.GetSelectedPage();

            TitleTextBox.Text = Page.Title;
            RelativeURLTextBlock.Text = Page.RelativeURL;
            IsRootTextBlock.Text = Page.IsRoot.ToString().ToLower();
            LastUpdatedTextBlock.Text = Page.GetLastUpdatedAsString();
            LastSubmittedTextBlock.Text = Page.GetLastSubmittedAsString();
            ContentHTMLTextBox.Text = Page.ContentHTML;
            HeaderHTMLTextBox.Text = Page.AdditionalHeaderHTML;
        }

        private void TextChanged(object sender, KeyRoutedEventArgs e)
        {
            MadeChanges = true;
            SaveChangesButton.IsEnabled = true;
        }

        private void HTMLTextBox_Paste(object sender, TextControlPasteEventArgs e)
        {
            // stupid fix for MadeChanges after pasting text (ex. from Sublime Text)
            TextChanged(sender, null);
        }

        private async void OkayButton_Click(object sender, RoutedEventArgs e)
        {
            if (MadeChanges)
            {
                // changes were made that weren't saved, confirm with user
                ContentDialog confirmation = new ContentDialog
                {
                    Title = "Are you sure?",
                    Content = "Would you like to save the changes you made?",
                    PrimaryButtonText = "No",
                    SecondaryButtonText = "Yes"
                };

                ContentDialogResult click = await confirmation.ShowAsync();

                if (click == ContentDialogResult.Primary)
                {
                    // no

                    // go back to home after dismissing dialog (after this "if" statement)
                    confirmation.Hide();
                }
                else
                {
                    // yes

                    // dismiss dialog
                    confirmation.Hide();
                    // don't go back home
                    return;
                }
            }
            // else, no changes were made, so just go back home

            // go back to home page with drill animation
            Frame.Navigate(typeof(Home), null, new DrillInNavigationTransitionInfo());
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            Page.Updated();
            Page.Title = TitleTextBox.Text;

            string t = ContentHTMLTextBox.Text;
            Page.ContentHTML = t;

            t = HeaderHTMLTextBox.Text;
            Page.AdditionalHeaderHTML = t;

            WebPageManager.SetSelectedPage(Page);
            WebPageManager.Save();

            MadeChanges = false;
            SaveChangesButton.IsEnabled = false;

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
                    name = Utils.RandomString(8); // ensure file with same name doesn't already exist (next to zero chance)

                await Windows.System.Launcher.LaunchFileAsync(await FileManager.CreateTemporaryFile(contentHTML, name + ".txt"));
            }
        }

        private async void DeployButton_Click(object sender, RoutedEventArgs e)
        {
            WebPageManager.SetSelectedPage(Page);
            WebPageManager.Save();

            DeployDialog dialog = new DeployDialog(WebPageManager.GetSelectedPage());
            await dialog.ShowAsync();

            LoadDetails();
        }

        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog confirmation = new ContentDialog
            {
                Title = "Are you sure?",
                Content = "Once deleted, this page and its contents cannot be restored!",
                PrimaryButtonText = "No",
                SecondaryButtonText = "I'm sure"
            };

            ContentDialogResult click = await confirmation.ShowAsync();

            if (click == ContentDialogResult.Primary)
            {
                // no

                // dismiss dialog and do nothing else
                confirmation.Hide();
            }
            else
            {
                // i'm sure

                // actually remove the page
                WebPageManager.Pages.Remove(Page);
                WebPageManager.Save();
                // dismiss dialog
                confirmation.Hide();
                // go back home with drill animation
                Frame.Navigate(typeof(Home), null, new DrillInNavigationTransitionInfo());
            }
        }

    }
}