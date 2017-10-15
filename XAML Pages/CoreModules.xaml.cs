using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Site_Manager
{
    public sealed partial class CoreModules : Page
    {

        public static ObservableCollection<string> Tags = new ObservableCollection<string>() { new CoreModule().Tag };

        private Storyboard ShowAnimationStoryboard, HideAnimationStoryboard;

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

            ShowAnimationStoryboard = (Storyboard)Resources["ShowAnimation"];
            HideAnimationStoryboard = (Storyboard)Resources["HideAnimation"];

            ShowAnimationStoryboard.Completed += (s, args) => { ShowAnimationStoryboard.Stop(); };
            HideAnimationStoryboard.Completed += (s, args) => { HideAnimationStoryboard.Stop(); };
        }

        private ObservableCollection<TextBox> GetTextBoxes()
        {
            ObservableCollection<TextBox> r = new ObservableCollection<TextBox>();
            foreach (UIElement element in (Content as StackPanel).Children)
            {
                if (element.GetType() == typeof(TextBox))
                {
                    // element is a TextBox
                    r.Add(element as TextBox);
                }
                // else, it is not a TextBox, so ignore it and continue
            }

            return r;
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

        private TextBox GetTextBoxByTag(string tag)
        {
            TextBox r = null;
            foreach (TextBox box in GetTextBoxes())
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
            System.Diagnostics.Debug.WriteLine("IndexOf " + tag + ": " + CoreManager.Modules.IndexOf(CoreManager.GetModuleByTag(tag)));
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

                Storyboard storyboard = ShowAnimationStoryboard;
                if (storyboard.GetCurrentState() != ClockState.Stopped)
                    return;
                Storyboard.SetTarget(storyboard.Children[0] as DoubleAnimation, box);
                storyboard.Begin();

                chevron.Content = GlobalString.DECODE_CHEVRON_UP;

                box.Visibility = Visibility.Visible;
            }
            else
            {
                // hide

                Storyboard storyboard = HideAnimationStoryboard;
                if (storyboard.GetCurrentState() != ClockState.Stopped)
                    return;
                Storyboard.SetTarget(storyboard.Children[0] as DoubleAnimation, box);
                storyboard.Begin();

                chevron.Content = GlobalString.DECODE_CHEVRON_DOWN;

                box.Visibility = Visibility.Collapsed;
            }
        }

    }
}