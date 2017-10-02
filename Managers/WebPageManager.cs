using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace Site_Manager
{
    class WebPageManager
    {

        public static ObservableCollection<ManagedWebPage> Pages = new ObservableCollection<ManagedWebPage>();
        public static int SelectedPageIndex = -1;

        public static ManagedWebPage GetPage(string lookFor)
        {
            foreach (ManagedWebPage page in Pages)
                if (page.RelativeURL == lookFor)
                    return page;
            return null;
        }

        public static ManagedWebPage GetPage(string lookFor, ManagedWebPage[] array)
        {
            foreach (ManagedWebPage page in array)
                if (page.RelativeURL == lookFor)
                    return page;
            return null;
        }

        public static ManagedWebPage GetSelectedPage() => Pages[SelectedPageIndex];

        public static void SortAlphabetically()
        {
            string[] urls = new string[Pages.Count];
            ManagedWebPage[] originalPages = new ManagedWebPage[Pages.Count];
            Pages.CopyTo(originalPages, 0);

            for (int i = 0; i < Pages.Count; i++)
                urls[i] = Pages[i].RelativeURL;

            Array.Sort(urls);
            Pages.Clear();

            foreach (string url in urls)
                Pages.Add(GetPage(url, originalPages));
        }

        public static void SetSelectedPage(ManagedWebPage page) => Pages[SelectedPageIndex] = page;

        public static void Save() => StorageManager.SaveToFile(Pages, GlobalString.WEBPAGE_FILENAME);

        public static void Load()
        {
            Pages = (ObservableCollection<ManagedWebPage>)StorageManager.LoadFromFile(Pages.GetType(), GlobalString.WEBPAGE_FILENAME);
            if (Pages == null)
                Pages = new ObservableCollection<ManagedWebPage>();
        }

    }
}