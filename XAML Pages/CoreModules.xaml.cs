using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Site_Manager
{
    public sealed partial class CoreModules : Page
    {

        private bool LoadedTextBoxes = false;

        public static ObservableCollection<string> Tags = new ObservableCollection<string>() { new CoreModule().Tag };
        public static ObservableCollection<TextBox> TextBoxes = new ObservableCollection<TextBox>();

        public CoreModules()
        {
            InitializeComponent();

            Tags = new ObservableCollection<string>();
            foreach (TextBox box in GetTextBoxes())
            {
                Tags.Add(box.Tag.ToString());
            }

            if (CoreManager.Modules.Count == 1)
            {
                CoreManager.Modules.Clear();
                for (int i = 0; i < Tags.Count; i++)
                {
                    CoreManager.Modules.Add(new CoreModule() { Tag = Tags[i] });
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // set text for every TextBox
            foreach (string tag in Tags)
            {
                if (CoreManager.GetModuleByTag(tag) == null)
                {
                    GetTextBoxByTag(tag).Text = "";
                }
                else
                {
                    GetTextBoxByTag(tag).Text = CoreManager.GetModuleByTag(tag).Code;
                }
            }
        }

        private ObservableCollection<TextBox> GetTextBoxes()
        {
            if (LoadedTextBoxes)
            {
                return TextBoxes;
            }
            TextBoxes.Clear();
            foreach (UIElement element in (Content as StackPanel).Children)
            {
                if (element.GetType() == typeof(StackPanel))
                {
                    foreach (UIElement subElement in (element as StackPanel).Children)
                    {
                        if (subElement.GetType() == typeof(TextBox))
                        {
                            // element is a TextBox
                            TextBoxes.Add(subElement as TextBox);
                        }
                    }
                }
                // otherwise it's not a TextBox, so ignore and continue
            }
            LoadedTextBoxes = true;
            return TextBoxes;
        }

        private TextBox GetTextBoxByTag(string tag)
        {
            TextBox r = null;
            foreach (TextBox box in TextBoxes)
            {
                if (box.Tag.ToString() == tag)
                {
                    r = box;
                    break;
                }
            }
            return r;
        }

        private async Task SaveCode(object sender)
        {
            // get tag of module to deploy
            string tag = (sender as Button).Tag.ToString();
            // update that module with the updated code
            Debug.Out("Saving \"" + tag + "\"...", "CORE MODULES");
            CoreManager.Modules[CoreManager.Modules.IndexOf(CoreManager.GetModuleByTag(tag))] = new CoreModule() { Code = GetTextBoxByTag(tag).Text, Tag = tag };
            // save changes
            await CoreManager.Save();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e) => await SaveCode(sender);

        private void Chevron_Click(object sender, RoutedEventArgs e)
        {
            Button chevron = sender as Button;
            string tag = chevron.Tag.ToString();
            TextBox box = GetTextBoxByTag(tag);

            if (box.Visibility == Visibility.Collapsed)
            {
                // show
                chevron.Content = GlobalString.DECODE_CHEVRON_UP;
                box.Visibility = Visibility.Visible;
            }
            else
            {
                // hide
                chevron.Content = GlobalString.DECODE_CHEVRON_DOWN;
                box.Visibility = Visibility.Collapsed;
            }
        }

    }
}