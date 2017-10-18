using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace Site_Manager
{
    class CoreManager
    {

        public static ObservableCollection<CoreModule> Modules = new ObservableCollection<CoreModule>();
        public static bool Loaded = false;

        public static async void Load()
        {
            Debug.Out("Loading modules...");
            if (Loaded)
            {
                Debug.Out("Modules have already been loaded!", "WARNING");
                return;
            }

            try
            {
                Debug.Out("Getting the \"" + GlobalString.CORE_MODULES_FILENAME + "\" file...");
                StorageFile file = await FileManager.GetStorageFile(GlobalString.CORE_MODULES_FILENAME);
                if (file == null)
                {
                    Debug.Out("The \"" + GlobalString.CORE_MODULES_FILENAME + "\" file did not exist, creating an empty collection of modules", "WARNING");
                    // create collection of empty modules
                    Modules = GetEmptyModules();
                    // save default modules
                    await Save();
                    // don't load again, will be in memory
                    Loaded = true;
                    // don't bother reading from file, return
                    return;
                }

                Debug.Out("Reading \"" + GlobalString.CORE_MODULES_FILENAME + "\" file...");
                // recursively load from file
                IList<string> lines = await FileIO.ReadLinesAsync(file);
                foreach (string line in lines)
                {
                    Modules.Add(new CoreModule(line));
                }

                // don't bother loading again, will always be in memory (right?!)
                Loaded = true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }

        public static async Task Save()
        {
            Debug.Out("Saving modules...");
            string[] asStrings = new string[Modules.Count];
            for (int i = 0; i < asStrings.Length; i++)
            {
                asStrings[i] = Modules[i].GetAsString();
            }
            if (!await FileManager.GetExists(GlobalString.CORE_MODULES_FILENAME))
            {
                Debug.Out("The \"" + GlobalString.CORE_MODULES_FILENAME + "\" file doesn't exist, so will create an empty one", "WARNING");
                await FileManager.CreateStorageFile(GlobalString.CORE_MODULES_FILENAME);
            }
            Debug.Out("Writing to " + GlobalString.CORE_MODULES_FILENAME + "\" file ...");
            await FileManager.WriteToFile(await FileManager.GetStorageFile(GlobalString.CORE_MODULES_FILENAME), asStrings);
        }

        public static CoreModule GetModuleByTag(string tag)
        {
            if (!Loaded)
            {
                return null;
            }

            foreach (CoreModule module in Modules)
            {
                if (module.Tag.Equals(tag))
                {
                    return module;
                }
            }
            return null;
        }

        public static ObservableCollection<CoreModule> GetEmptyModules()
        {
            ObservableCollection<CoreModule> r = new ObservableCollection<CoreModule>();
            foreach (string tag in CoreModules.Tags)
            {
                r.Add(new CoreModule() { Code = "", Tag = tag });
            }
            return r;
        }

    }
}