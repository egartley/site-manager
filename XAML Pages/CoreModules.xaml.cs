using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Site_Manager
{
    public sealed partial class CoreModules : Page
    {

        public static ObservableCollection<string> Tags;

        private Storyboard Storyboard = new Storyboard();
        private DoubleAnimation Rotate180Animation;
        private DoubleAnimation CollaspeAnimation;

        public CoreModules()
        {
            InitializeComponent();

            Tags = new ObservableCollection<string>();
            foreach (TextBox box in GetTextBoxes())
                Tags.Add(box.Tag.ToString());

            Storyboard.Completed += (s, args) => { Storyboard.Stop(); Storyboard.Children.Clear(); };
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
                GetTextBoxByTag(tag).Text = CoreManager.GetModuleByTag(tag).Code;
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

        private void SaveCode(object sender)
        {
            // get tag of module to deploy
            string tag = (sender as Button).Tag.ToString();
            // update that module with the updated code
            CoreManager.Modules[CoreManager.Modules.IndexOf(CoreManager.GetModuleByTag(tag))] = new CoreModule() { Code = GetTextBoxByTag(tag).Text, Tag = tag };
            // save changes
            CoreManager.Save();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) => SaveCode(sender);

        private void Chevron_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();
            TextBox box = GetTextBoxByTag(tag);
            Visibility vis = box.Visibility;
            bool hidden = vis == Visibility.Collapsed;
            int to = 180, from = 0;

            if (hidden)
            {
                to = 0;
                from = 180;
            }

            // Chevron animation
            Rotate180Animation = new DoubleAnimation { To = to, From = from, Duration = new TimeSpan(0, 0, 0, 0, 250) };
            Storyboard.SetTarget(Rotate180Animation, sender as Button);
            Storyboard.SetTargetProperty(Rotate180Animation, "(UIElement.RenderTransform).Angle");
            Storyboard.Children.Add(Rotate180Animation);

            if (hidden)
            {
                to = 1;
                from = 0;
            }
            else
            {
                to = 0;
                from = 1;
            }

            // TextBox animation
            CollaspeAnimation = new DoubleAnimation { To = to, From = from, Duration = new TimeSpan(0, 0, 0, 0, 250) };
            Storyboard.SetTarget(CollaspeAnimation, box);
            Storyboard.SetTargetProperty(CollaspeAnimation, "(UIElement.RenderTransform).(CompositeTransform.ScaleY)");
            Storyboard.Children.Add(CollaspeAnimation);

            if (!hidden)
                box.Visibility = Visibility.Collapsed;
            else
            {
                box.Visibility = Visibility.Visible;
            }

            Storyboard.Begin();
        }

    }
}