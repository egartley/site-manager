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

        /// <summary>
        /// Loads previously saved modules into memory (if not already loaded)
        /// </summary>
        public static async void Load()
        {
            if (Loaded)
                return; // already loaded

            try
            {
                StorageFile file = await FileManager.GetStorageFile(GlobalString.CORE_MODULES_FILENAME);
                if (file == null)
                {
                    // create collection of empty modules
                    Modules = GetEmptyModules();
                    // save default modules
                    await Save();
                    // don't load again, will be in memory
                    Loaded = true;
                    // don't bother reading from file, return
                    return;
                }

                // recursively load from file
                IList<string> lines = await FileIO.ReadLinesAsync(file);
                foreach (string line in lines)
                    Modules.Add(new CoreModule(line));
                
                // don't bother loading again, will always be in memory (right?!)
                Loaded = true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }

        /// <summary>
        /// Saves modules that are currently in memory as a composite
        /// </summary>
        public static async Task Save()
        {
            string[] asStrings = new string[Modules.Count];
            for (int i = 0; i < asStrings.Length; i++)
                asStrings[i] = Modules[i].GetAsString();

            if (!await FileManager.GetExists(GlobalString.CORE_MODULES_FILENAME))
                await FileManager.CreateStorageFile(GlobalString.CORE_MODULES_FILENAME);
            await FileManager.WriteToFile(await FileManager.GetStorageFile(GlobalString.CORE_MODULES_FILENAME), asStrings);
        }

        public static ObservableCollection<CoreModule> GetEmptyModules()
        {
            ObservableCollection<CoreModule> r = new ObservableCollection<CoreModule>();
            foreach (string tag in CoreModules.Tags)
                r.Add(new CoreModule() { Code = "", Tag = tag });
            return r;
        }

        /// <summary>
        /// Returns the module with specified tag, otherwise null if not found or were not previously loaded with Load()
        /// </summary>
        /// <param name="tag">Tag to search for</param>
        /// <returns>CoreModule with specified tag, otherwise null</returns>
        public static CoreModule GetModuleByTag(string tag)
        {
            if (!Loaded)
                return null;
            System.Diagnostics.Debug.WriteLine(Modules.Count);
            foreach (CoreModule module in Modules)
            {
                System.Diagnostics.Debug.WriteLine(module.Tag);
                if (module.Tag.Equals(tag))
                    return module;
            }
            return null;
        }

    }
}