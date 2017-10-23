using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Site_Manager
{
    public sealed partial class Settings : Page
    {

        public Settings() => InitializeComponent();

        private void SaveFTPConfigButton_Click(object sender, RoutedEventArgs e) => SettingsManager.SetComposite(new ApplicationDataCompositeValue { [GlobalString.COMPOSITE_KEY_FTPCONFIG_USERNAME] = UsernameTextBox.Text, [GlobalString.COMPOSITE_KEY_FTPCONFIG_PASSWORD] = PasswordTextBox.Password, [GlobalString.COMPOSITE_KEY_FTPCONFIG_SERVER] = ServerTextBox.Text }, GlobalString.COMPOSITE_KEY_FTPCONFIG);

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FTPManager.LoadConfiguration();
            UsernameTextBox.Text = FTPManager.Username;
            PasswordTextBox.Password = FTPManager.Password;
            ServerTextBox.Text = FTPManager.Server;
            BlankDeployCheckBox.IsChecked = DeveloperOptions.GetBlankDeploy();
            UseTestDirectoryCheckBox.IsChecked = DeveloperOptions.GetUseTestDirectory();

            ToolTipService.SetToolTip(BlankDeployCheckBox, new ToolTip() { Content = GlobalString.TOOLTIP_DEVOPTIONS_BLANKDEPLOY });
            ToolTipService.SetToolTip(UseTestDirectoryCheckBox, new ToolTip() { Content = GlobalString.TOOLTIP_DEVOPTIONS_USETESTDIRECTORY });
        }

        private void BlankDeployCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DeveloperOptions.SetBlankDeploy(true);
            DeveloperOptions.Save();
        }

        private void BlankDeployCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DeveloperOptions.SetBlankDeploy(false);
            DeveloperOptions.Save();
        }

        private void UseTestDirectoryCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DeveloperOptions.SetUseTestDirectory(true);
            DeveloperOptions.Save();
        }

        private void UseTestDirectoryCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DeveloperOptions.SetUseTestDirectory(false);
            DeveloperOptions.Save();
        }
    }
}