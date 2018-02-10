using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace Site_Manager
{
    public sealed partial class Home : Page
    {

        private ObservableCollection<WebPageListItem> ListItems = new ObservableCollection<WebPageListItem>();
        private ObservableCollection<string> AutoSuggestItems = new ObservableCollection<string>();
        private ObservableCollection<string> OriginalAutoSuggestItems = new ObservableCollection<string>();

        public Home() => InitializeComponent();

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await SyncListItems();
            UpdateDeployLocationTextBlock();
        }

        private async Task SyncListItems()
        {
            if (!WebPageManager.Loaded)
            {
                await WebPageManager.Load();
            }
            if (WebPageManager.Pages.Count != 0)
            {
                ListItems.Clear();
                foreach (ManagedWebPage page in WebPageManager.Pages)
                {
                    ListItems.Add(new WebPageListItem(page.Title, page.RelativeURL, page.GetLastUpdatedAsString()));
                }
            }
            else
            {
                ListItems = new ObservableCollection<WebPageListItem>();
            }
            WebPagesListView.ItemsSource = ListItems;

            // build autosuggestbox itemssource
            AutoSuggestItems.Clear();
            OriginalAutoSuggestItems.Clear();
            foreach (ManagedWebPage page in WebPageManager.Pages)
            {
                AutoSuggestItems.Add(page.RelativeURL);
                OriginalAutoSuggestItems.Add(page.RelativeURL);
            }
            NewPageTextBox.ItemsSource = AutoSuggestItems;
        }

        public void UpdateDeployLocationTextBlock()
        {
            if (DeveloperOptions.GetUseTestDirectory())
            {
                DeployLocationTextBlock.Text = "Test (\"/test\")";
            }
            else
            {
                DeployLocationTextBlock.Text = "Production";
            }
        }

        private void UpdateAutoSuggestItems(string query)
        {
            AutoSuggestItems.Clear();
            foreach (string url in OriginalAutoSuggestItems)
            {
                if (url.StartsWith(query))
                {
                    AutoSuggestItems.Add(url);
                }
            }
            NewPageTextBox.ItemsSource = AutoSuggestItems;
        }

        private WebPageListItem GetWebPageListItemByURL(string url)
        {
            if (url == null)
            {
                throw new System.ArgumentNullException(nameof(url));
            }
            foreach (WebPageListItem item in ListItems)
            {
                if (item.URL == url)
                {
                    return item;
                }
            }
            return null;
        }

        private async Task ThrowValidationError(string message)
        {
            AddNewPageStatusTextBlock.Text = message;

            // display error for 4.275 seconds
            await Task.Delay(4275);

            AddNewPageStatusTextBlock.Text = "";
        }

        private async void AddNewPageButton_Click(object sender, RoutedEventArgs e)
        {
            string url = NewPageTextBox.Text;
            // validate input
            if (url.Length == 0)
            {
                // don't throw a validation error, because it is pretty obvious what the problem is
                return;
            }
            if (!url.StartsWith("/"))
            {
                await ThrowValidationError("Must start with \"/\" (relative to \"https://egartley.net\")");
                return;
            }
            if (!url.EndsWith("/"))
            {
                await ThrowValidationError("Must end with \"/\"");
                return;
            }
            if (url.Contains(" "))
            {
                await ThrowValidationError("Cannot contain spaces (consider using \"-\" or \"%20\" instead)");
                return;
            }
            if (!Utils.IsValidURL(url))
            {
                await ThrowValidationError("Invalid URL");
                return;
            }
            if (OriginalAutoSuggestItems.Contains(url))
            {
                await ThrowValidationError("That page already exists");
                return;
            }

            // add new TextBlock (list item) to ListItems collection
            ListItems.Add(new WebPageListItem("Untitled Page", url, "Never"));

            // also add a new ManagedWebPage to WebPageManager.Pages
            WebPageManager.Pages.Add(new ManagedWebPage(url));

            if (ListItems.Count > 1)
            {
                WebPageManager.Sort();
                ListItems.Clear();
                foreach (ManagedWebPage page in WebPageManager.Pages)
                {
                    ListItems.Add(new WebPageListItem(page.Title, page.RelativeURL, page.GetLastUpdatedAsString()));
                }
            }
            // otherwise there is only one page, so there is no point in sorting

            // save new page
            await WebPageManager.Save();

            // update UI
            NewPageTextBox.Text = "";
            WebPagesListView.ItemsSource = ListItems;
        }

        private void NewPageTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                // pressed enter
                AddNewPageButton_Click(null, null);
            }
        }

        private void NewPageTextBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            if (NewPageTextBox.Text.Length == 0)
            {
                AddNewPageButton.IsEnabled = false;
            }
            else
            {
                AddNewPageButton.IsEnabled = true;
                if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    UpdateAutoSuggestItems(NewPageTextBox.Text);
                }
            }
        }

        private void NewPageTextBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            // do nothing (since suggestions are pages that already exist, cannot create a new page with the same url)
        }

        private void NewPageTextBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // set the textbox's text to whatever was chosen
            NewPageTextBox.Text = args.SelectedItem as string;
        }

        private void WebPagesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            WebPageManager.SelectedPageIndex = WebPageManager.Pages.IndexOf(WebPageManager.GetPage(((WebPageListItem)e.ClickedItem).URL));
            Frame.Navigate(typeof(EditWebPage), null, new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
        }

        private async void DeployButton_Click(object sender, RoutedEventArgs e) => await Deployer.DeployAll();

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}