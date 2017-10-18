using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace Site_Manager
{
    public sealed partial class Home : Page
    {
        private ObservableCollection<TextBlock> ListItems = new ObservableCollection<TextBlock>();

        public Home() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e) => SyncListItems();

        private void SyncListItems()
        {
            WebPageManager.Load();
            if (WebPageManager.Pages.Count != 0)
            {
                ListItems.Clear();
                foreach (ManagedWebPage page in WebPageManager.Pages)
                {
                    ListItems.Add(new TextBlock() { Text = page.RelativeURL });
                }
            }
            else
            {
                ListItems = new ObservableCollection<TextBlock>();
            }
            WebPagesListView.ItemsSource = ListItems;
        }

        private TextBlock GetTextBlockByURL(string url)
        {
            if (url == null)
            {
                throw new System.ArgumentNullException(nameof(url));
            }
            foreach (TextBlock block in ListItems)
            {
                if (block.Text == url)
                {
                    return block;
                }
            }
            return null;
        }

        private async Task ThrowParsingError(string message)
        {
            AddNewPageStatusTextBlock.Visibility = Visibility.Visible;
            AddNewPageStatusTextBlock.Text = "Parsing error! (" + message + ")";

            // display error for 6 seconds
            await Task.Delay(6000);

            AddNewPageStatusTextBlock.Visibility = Visibility.Collapsed;
        }

        private async void AddNewPageButton_Click(object sender, RoutedEventArgs e)
        {
            string input = NewPageTextBox.Text;
            // validate input
            if (input.Length == 0)
            {
                return;
            }
            if (!input.Contains("/"))
            {
                await ThrowParsingError("Must contain at least one \"/\"");
                return;
            }
            if (!input.StartsWith("/"))
            {
                await ThrowParsingError("Must start with a \"/\"");
                return;
            }
            if (input.Contains(" "))
            {
                await ThrowParsingError("Cannot contain spaces");
                return;
            }
            if (!Utils.IsValidURL(input))
            {
                await ThrowParsingError("Invalid URL");
                return;
            }

            // add new TextBlock (list item) to ListItems collection
            ListItems.Add(new TextBlock() { Text = input });

            // also add a new ManagedWebPage to WebPageManager.Pages
            WebPageManager.Pages.Add(new ManagedWebPage(input));

            if (ListItems.Count > 1)
            {
                WebPageManager.Sort();
                ListItems.Clear();
                foreach (ManagedWebPage page in WebPageManager.Pages)
                {
                    ListItems.Add(new TextBlock() { Text = page.RelativeURL });
                }
            }
            // else there is only one page, no point in sorting

            // save new page
            WebPageManager.Save();

            // update UI
            NewPageTextBox.Text = "";
            WebPagesListView.ItemsSource = ListItems;
        }

        private void NewPageTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                AddNewPageButton_Click(null, null);
            }
        }

        private void NewPageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NewPageTextBox.Text.Length == 0)
            {
                AddNewPageButton.IsEnabled = false;
            }
            else
            {
                AddNewPageButton.IsEnabled = true;
            }
        }

        private void WebPagesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // TODO: right click context menu for removing/deploying (maybe)

            WebPageManager.SelectedPageIndex = WebPageManager.Pages.IndexOf(WebPageManager.GetPage(((TextBlock)e.ClickedItem).Text));
            // go to edit page with drill animation
            Frame.Navigate(typeof(EditWebPage), null, new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
        }

        private async void DeployButton_Click(object sender, RoutedEventArgs e) => await Deployer.DeployAll();
    }
}