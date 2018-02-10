using System;
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
            Debug.Out("Loading modules...", "CORE MANAGER");
            if (Loaded)
            {
                Debug.Out("Modules have already been loaded!", "WARNING");
                return;
            }

            try
            {
                Debug.Out("Getting \"" + GlobalString.FILENAME_CORE_MODULES + "\"...", "CORE MANAGER");
                StorageFile file = await FileManager.GetStorageFile(GlobalString.FILENAME_CORE_MODULES);
                if (file == null)
                {
                    Debug.Out("\"" + GlobalString.FILENAME_CORE_MODULES + "\" did not exist, creating an empty collection of modules and writing to it...", "WARNING");
                    // create collection of empty modules
                    Modules = GetEmptyModules();
                    // save default modules
                    await Save();
                    // don't load again, will be in memory
                    Loaded = true;
                    // don't bother reading from file, return
                    return;
                }

                Debug.Out("Reading \"" + GlobalString.FILENAME_CORE_MODULES + "\"...", "CORE MANAGER");
                // recursively load from file
                CoreModule[] modules = Newtonsoft.Json.JsonConvert.DeserializeObject<CoreModule[]>(await FileManager.GetFileContents(file));
                foreach (CoreModule mod in modules)
                {
                    Modules.Add(mod);
                }

                // don't bother loading again, will always be in memory (right?)
                Loaded = true;
            }
            catch (Exception e)
            {
                Debug.Out(e.StackTrace, "EXCEPTION");
                Debug.Out(e.Message, "EXCEPTION");
            }
        }

        public static async Task Save()
        {
            Debug.Out("Saving modules...", "CORE MANAGER");
            CoreModule[] array = new CoreModule[Modules.Count];
            Modules.CopyTo(array, 0);

            if (!await FileManager.GetExists(GlobalString.FILENAME_CORE_MODULES))
            {
                Debug.Out("\"" + GlobalString.FILENAME_CORE_MODULES + "\" doesn't exist, creating an empty one...", "WARNING");
                await FileManager.CreateStorageFile(GlobalString.FILENAME_CORE_MODULES);
            }

            Debug.Out("Writing to " + GlobalString.FILENAME_CORE_MODULES + "\" ...", "CORE MANAGER");
            await FileManager.WriteToFile(await FileManager.GetStorageFile(GlobalString.FILENAME_CORE_MODULES), Newtonsoft.Json.JsonConvert.SerializeObject(array));
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
                r.Add(new CoreModule(tag));
            }
            return r;
        }

    }
}