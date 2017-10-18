using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Site_Manager
{
    public sealed partial class ChangeRedirectionDialog : ContentDialog
    {

        private string URL;
        string Destination { get; set; }

        public ChangeRedirectionDialog(string url, string d)
        {
            InitializeComponent();
            Title += url;
            RenameTextBox.PlaceholderText = url;
            Destination = d;
            DestinationTextBlock.Text = Destination;
            URL = url;
        }

        public void SetDestination(string d)
        {
            Destination = d;
            DestinationTextBlock.Text = Destination;
        }

        private void Apply(bool apply)
        {
            ApplyButton.IsEnabled = apply;
        }

        private void Deleted()
        {
            ConfirmationCheckBox.IsChecked = false;
            ConfirmationCheckBox.IsEnabled = false;
            DeleteRedirectionCheckBox.IsChecked = false;
            DeleteRedirectionCheckBox.IsEnabled = false;
        }

        private void Working(bool working)
        {
            Visibility v = Visibility.Collapsed;
            if (working)
            {
                v = Visibility.Visible;
                OkButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                OkButton.Visibility = Visibility.Visible;
            }

            WorkingStackPanel.Visibility = v;
            WorkingProgressRing.Visibility = v;
            WorkingProgressRing.IsActive = working;
            WorkingTextBlock.Visibility = v;
        }

        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Working(true);

            bool rename = RenameTextBox.Text.Length != 0;
            bool delete = (bool) ConfirmationCheckBox.IsChecked;

            // take action on applications
            if (delete)
            {
                await FTPManager.Connect();
                await FTPManager.DeleteFile("/go/" + URL + "/index.html");
                await FTPManager.DeleteDirectory("/go/" + URL);
                await FTPManager.Disconnect();
                // already deleted, can't delete again!
                DeleteRedirectionCheckBox.IsEnabled = false;
                // doesn't go anywhere!
                DestinationTextBlock.Text = "404 Not Found";
                Title = "(Removed)";
                // remove from existing redirections list view
                App.RedirectionRemoved = true;
                App.RemovedRedirectionListItemURL = URL;
            }
            else if (rename)
            {
                // TODO: rename short URL
            }
            else
            {
                // should never reach here (hopefully!)
            }
            Working(false);
            Apply(false);
            Deleted();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) => Hide();

        private void DeleteRedirectionCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteRedirectionCheckBox.IsChecked == true)
            {
                ConfirmationCheckBox.Visibility = Visibility.Visible;
            }
            else
            {
                ConfirmationCheckBox.Visibility = Visibility.Collapsed;
                if (RenameTextBox.Text.Length == 0)
                {
                    Apply(false);
                }
            }
            ConfirmationCheckBox.IsChecked = false;
        }

        private void ConfirmationCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmationCheckBox.IsChecked == true)
            {
                Apply(true);
            }
            else if (RenameTextBox.Text.Length == 0)
            {
                Apply(false);
            }
        }

        private void RenameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RenameTextBox.Text.Length > 0)
            {
                Apply(true);
            }
            else if (DeleteRedirectionCheckBox.IsChecked == false)
            {
                Apply(false);
            }
        }
    }
}