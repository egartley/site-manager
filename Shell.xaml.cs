using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Site_Manager
{

    public sealed partial class Shell : Page
    {

        private string LastPage = "";
        private ObservableCollection<NavigationViewItem> NavigationItems = new ObservableCollection<NavigationViewItem>();

        public Shell()
        {
            NavigationItems.Add(new NavigationViewItem() { Content = "Home", Icon = new SymbolIcon(Symbol.Home) });
            NavigationItems.Add(new NavigationViewItem() { Content = "Redirections", Icon = new SymbolIcon(Symbol.Forward) });
            NavigationItems.Add(new NavigationViewItem() { Content = "Core Modules", Icon = new SymbolIcon(Symbol.Library) });
            InitializeComponent();
        }

        private void ShellNavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (NavigationViewItem item in NavigationItems)
            {
                ShellNavigationView.MenuItems.Add(item);
            }
            // naviagte to home page by default
            LastPage = "Home";
            ShellFrame.Navigate(typeof(Home));
        }

        private void ShellNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                LastPage = "Settings";
                ShellFrame.Navigate(typeof(Settings));
                return; // don't do anything else
            }

            string clickedNavigationItem = args.InvokedItem.ToString();
            if (clickedNavigationItem == LastPage)
            {
                // don't navigate to page if already navigated to (duh!)
                Debug.Out("Navigation was annulled (tried going to the page already navigated to)");
                return;
            }
            else
            {
                LastPage = clickedNavigationItem;
            }

            switch (clickedNavigationItem)
            {
                case "Home":
                    Debug.Out("Navigating to Home.xaml");
                    ShellFrame.Navigate(typeof(Home));
                    break;

                case "Redirections":
                    Debug.Out("Navigating to Redirections.xaml");
                    ShellFrame.Navigate(typeof(Redirections));
                    break;

                case "Core Modules":
                    Debug.Out("Navigating to CoreModules.xaml");
                    ShellFrame.Navigate(typeof(CoreModules));
                    break;

                default:
                    Debug.Out("Unknown page \"" + clickedNavigationItem + "\" was attempted to be navigated to", "WARNING");
                    break;
            }
        }

        private void ShellFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(EditWebPage))
            {
                LastPage = "EditWebPage"; // be able to navigate back to home page from edit web page
            }
        }
    }
}