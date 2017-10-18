using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Site_Manager
{
    public sealed partial class Settings : Page
    {

        public Settings() => InitializeComponent();

        private void SaveFTPConfigButton_Click(object sender, RoutedEventArgs e) => SettingsManager.SetComposite(new ApplicationDataCompositeValue { ["username"] = UsernameTextBox.Text, ["password"] = PasswordTextBox.Password, ["server"] = ServerTextBox.Text }, "FileTransferProtocolConfiguration");

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FTPManager.LoadConfiguration();
            UsernameTextBox.Text = FTPManager.Username;
            PasswordTextBox.Password = FTPManager.Password;
            ServerTextBox.Text = FTPManager.Server;
        }

        private void BlankDeployCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void BlankDeployCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}