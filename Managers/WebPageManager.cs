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
        public static bool Loaded = false;

        /// <summary>
        /// Returns the page with the same relative URL as the given one, in WebPageManager.Pages
        /// </summary>
        public static ManagedWebPage GetPage(string url)
        {
            if (!Loaded)
            {
                Debug.Out("Tried to get \"" + url + "\", but webpages haven't been loaded yet", "WARNING");
                return null;
            }
            foreach (ManagedWebPage page in Pages)
            {
                if (page.RelativeURL == url)
                {
                    Debug.Out("Returning \"" + url + "\"", "WEB PAGE MANAGER");
                    return page;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the page with the same relative URL as the given one, in the provided array
        /// </summary>
        public static ManagedWebPage GetPage(string url, ManagedWebPage[] array)
        {
            if (!Loaded)
            {
                Debug.Out("Tried to get \"" + url + "\", but webpages haven't been loaded yet", "WARNING");
                return null;
            }
            foreach (ManagedWebPage page in array)
            {
                if (page.RelativeURL == url)
                {
                    Debug.Out("Returning \"" + url + "\"", "WEB PAGE MANAGER");
                    return page;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the "selected" ManagedWebPage
        /// </summary>
        /// <returns></returns>
        public static ManagedWebPage GetSelectedPage() => Pages[SelectedPageIndex];

        /// <summary>
        /// Alphabetically sorts all pages by their relative URL
        /// </summary>
        public static void Sort()
        {
            Debug.Out("Sorting webpages alphabetically", "WEB PAGE MANAGER");
            string[] urls = new string[Pages.Count];
            ManagedWebPage[] originalPages = new ManagedWebPage[Pages.Count];
            Pages.CopyTo(originalPages, 0);

            for (int i = 0; i < Pages.Count; i++)
            {
                urls[i] = Pages[i].RelativeURL;
            }

            Array.Sort(urls);
            Pages.Clear();

            foreach (string url in urls)
            {
                Pages.Add(GetPage(url, originalPages));
            }
        }

        /// <summary>
        /// Sets the "selected" page, useful for coordinating between Home.xaml and EditWebPage.xaml
        /// </summary>
        public static void SetSelected(ManagedWebPage page)
        {
            Debug.Out("Setting selected page to \"" + page.RelativeURL + "\"", "WEB PAGE MANAGER");
            Pages[SelectedPageIndex] = page;
        }

        /// <summary>
        /// Saves all pages
        /// </summary>
        public static async Task Save()
        {
            ManagedWebPage[] pages = new ManagedWebPage[Pages.Count];
            Pages.CopyTo(pages, 0);

            if (!await FileManager.GetExists(GlobalString.FILENAME_WEBPAGES))
            {
                Debug.Out("\"" + GlobalString.FILENAME_WEBPAGES + "\" doesn't exist, creating an empty one", "WARNING");
                await FileManager.CreateStorageFile(GlobalString.FILENAME_WEBPAGES);
            }

            Debug.Out("Saving", "WEB PAGE MANAGER");
            await FileManager.WriteToFile(await FileManager.GetStorageFile(GlobalString.FILENAME_WEBPAGES), Newtonsoft.Json.JsonConvert.SerializeObject(pages));
        }

        /// <summary>
        /// Loads all of the pages that are stored in the webpages.dat file
        /// </summary>
        public static async Task Load()
        {
            Debug.Out("Loading", "WEB PAGE MANAGER");
            if (Loaded)
            {
                Debug.Out("Webpages have already been loaded", "WARNING");
                return;
            }

            try
            {
                Debug.Out("Getting \"" + GlobalString.FILENAME_WEBPAGES + "\"", "WEB PAGE MANAGER");
                StorageFile file = await FileManager.GetStorageFile(GlobalString.FILENAME_WEBPAGES);
                if (file == null)
                {
                    Debug.Out("\"" + GlobalString.FILENAME_WEBPAGES + "\" did not exist, creating an empty one", "WARNING");
                    Pages = new ObservableCollection<ManagedWebPage>();
                    await Save();
                    Loaded = true;
                    return;
                }

                Debug.Out("Reading \"" + GlobalString.FILENAME_WEBPAGES + "\"", "WEB PAGE MANAGER");
                ManagedWebPage[] pages = Newtonsoft.Json.JsonConvert.DeserializeObject<ManagedWebPage[]>(await FileManager.GetFileContents(file));
                foreach (ManagedWebPage page in pages)
                {
                    Debug.Out("Adding \"" + page.RelativeURL + "\"", "WEB PAGE MANAGER");
                    Pages.Add(page);
                }

                Loaded = true;
            }
            catch (Exception e)
            {
                Debug.Out(e.StackTrace, "EXCEPTION");
                Debug.Out(e.Message, "EXCEPTION");
            }
        }
    }
}