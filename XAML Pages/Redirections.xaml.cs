using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.UI;
using System.Collections.Generic;

namespace Site_Manager
{
    public sealed partial class Redirections : Page
    {

        private bool ExistsError = false;
        private ObservableCollection<TextBlock> ListItems = new ObservableCollection<TextBlock>();
        private SolidColorBrush White = new SolidColorBrush(Colors.White);
        private SolidColorBrush Red = new SolidColorBrush(Colors.Red);
        private Random Random = new Random();
        private TextBlock CurrentListItem;

        public Redirections()
        {
            InitializeComponent();
        }

        private void Ready()
        {
            MakeRedirectionStatusTextBlock.Foreground = White;
            MakeRedirectionStatusTextBlock.Text = "Enter a short link and destination to get started";
        }

        private void SortItems()
        {
            string[] old = new string[ListItems.Count];
            for (int i = 0; i < ListItems.Count; i++)
                old[i] = ListItems[i].Text;
            Array.Sort(old);
            ListItems.Clear();
            foreach (string s in old)
                ListItems.Add(new TextBlock() { Text = s });
            CurrentRedirectionsListView.ItemsSource = ListItems;
        }

        private async Task Error(string message)
        {
            MakeRedirectionStatusTextBlock.Foreground = Red;
            MakeRedirectionStatusTextBlock.Text = message;
            await Task.Delay(6000);
            Ready();
        }

        private async Task Success(string addedItemURL)
        {
            MakeRedirectionStatusTextBlock.Foreground = White;
            MakeRedirectionStatusTextBlock.Text = "Success!";

            ListItems.Add(new TextBlock() { Text = addedItemURL });
            SortItems();

            await Task.Delay(3500);

            Ready();
        }

        private async Task DeployRedirection(string shortURL, string destination)
        {
            string path = "/go/" + shortURL;

            MakeRedirectionStatusTextBlock.Text = "Writing file...";
            StorageFile file = await FileManager.CreateTemporaryFile(HTMLBuilder.GetRedirectionHTML(destination));

            MakeRedirectionStatusTextBlock.Text = "Connecting...";
            await FTPManager.ConnectAsync();

            MakeRedirectionStatusTextBlock.Text = "Checking for \"/" + shortURL + "\"...";
            ExistsError = await FTPManager.GetDirectoryExistsAsync(path);

            if (!ExistsError)
            {
                MakeRedirectionStatusTextBlock.Text = "Creating \"/" + shortURL + "\"...";
                await FTPManager.CreateDirectoryAsync(path);

                MakeRedirectionStatusTextBlock.Text = "Uploading file...";
                await FTPManager.UploadAsync(file, path);
            }

            MakeRedirectionStatusTextBlock.Text = "Disconnecting...";
            await FTPManager.DisconnectAsync();

            MakeRedirectionStatusTextBlock.Text = "Deleting file...";
            await file.DeleteAsync();
        }

        private async Task FetchCurrentRedirections()
        {
            if (ListItems.Count > 0)
                ListItems.Clear();

            FetchRedirectionsStatusTextBlock.Text = "Working...";
            List<FTPItem> listing = await FTPManager.GetDirectoryContentsAsync("/go", true);

            for (int i = 0; i < listing.Count; i++)
            {
                FTPItem currentItem = listing[i];

                if (!currentItem.IsDirectory)
                    continue; // exclude .htaccess

                else if (currentItem.Name == "cgi-bin" || currentItem.Name == ".well-known" || currentItem.Name == "error")
                    continue; // exclude protected and error page directories

                ListItems.Add(new TextBlock() { Text = currentItem.Name });
            }

            CurrentRedirectionsListView.ItemsSource = ListItems;
        }

        private async void RedirectSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // update UI
            RedirectionProgressRing.IsActive = true;
            RedirectionProgressRing.Visibility = Visibility.Visible;
            MakeRedirectionStatusTextBlock.Text = "Working...";
            MakeRedirectionStatusTextBlock.Foreground = White;

            // validate input
            bool textError = DestinationTextBox.Text.Contains(" ") || ShortURLTextBox.Text.Contains(" ");
            if (!textError)
                textError = DestinationTextBox.Text.StartsWith("www.") || ShortURLTextBox.Text.StartsWith("/");
            if (!textError)
                textError = DestinationTextBox.Text.EndsWith("/") || ShortURLTextBox.Text.EndsWith("/");

            // do work, assuming input it valid
            if (!textError)
                await DeployRedirection(ShortURLTextBox.Text, DestinationTextBox.Text);

            // update UI
            RedirectionProgressRing.Visibility = Visibility.Collapsed;
            RedirectionProgressRing.IsActive = false;
            string dir = ShortURLTextBox.Text, dest = DestinationTextBox.Text;
            if (ShortURLTextBox.IsEnabled == false)
                ShortURLTextBox.IsEnabled = true;
            ShortURLTextBox.Text = "";
            DestinationTextBox.Text = "";
            RandomizeShortURLToggleSwitch.IsOn = false;

            // update status with error or success
            if (ExistsError)
                await Error("Failed to create \"/" + dir + "\" (already exists)!");
            else if (textError)
                await Error("Invalid short link or destination!");
            else
                await Success(dir);
        }

        private async void FetchRedirectionsButton_Click(object sender, RoutedEventArgs e)
        {
            // update UI
            FetchRedirectionsButton.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
            FetchProgressAnimation.Begin();
            FetchRedirectionsButton.IsEnabled = false;

            // do work
            await FetchCurrentRedirections();

            // update UI
            FetchProgressAnimation.Stop();
            FetchRedirectionsButton.Foreground = new SolidColorBrush(Colors.White);
            FetchRedirectionsButton.IsEnabled = true;
            FetchRedirectionsStatusTextBlock.Text = "";
        }

        private void RandomizeShortURLToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (RandomizeShortURLToggleSwitch.IsOn)
            {
                // turned on
                ShortURLTextBox.IsEnabled = false;
                ShortURLTextBox.Text = Utils.RandomString(6);
            }
            else
            {
                // turned off
                ShortURLTextBox.Text = "";
                ShortURLTextBox.IsEnabled = true;
            }
        }

        private void ChangeDialog_Closed()
        {
            if (App.RedirectionRemoved)
            {
                App.RedirectionRemoved = false;
                ObservableCollection<TextBlock> old = ListItems, current = old;
                ObservableCollection<string> oldURLs = new ObservableCollection<string>();
                // "convert" current list items to their urls
                for (int i = 0; i < old.Count; i++)
                {
                    oldURLs.Add(old[i].Text);
                }
                // iterate through each url and find the index of the removed url
                foreach (string url in oldURLs)
                {
                    if (url.Equals(App.RemovedRedirectionListItemURL))
                    {
                        current.RemoveAt(oldURLs.IndexOf(url));
                        break;
                    }
                }
                // finally update list items and UI
                ListItems = current;
                CurrentRedirectionsListView.ItemsSource = ListItems;

                // this is probably way too complicated, but whatever
            }
        }

        private async void CurrentRedirectionsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            TextBlock item = (TextBlock)e.ClickedItem;
            CurrentListItem = item;

            ChangeRedirectionDialog changeDialog = new ChangeRedirectionDialog(item.Text, ""); // destination will be determined later
            changeDialog.Closed += (s, args) => ChangeDialog_Closed();

            RedirectionDialog dialog = new RedirectionDialog(item.Text, changeDialog);
            await dialog.ShowAsync();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DestinationTextBox.Text.Length > 0 && ShortURLTextBox.Text.Length > 0)
                SubmitRedirectionButton.IsEnabled = true;
            else
                SubmitRedirectionButton.IsEnabled = false;
        }
    }
}