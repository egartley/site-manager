using System;
using System.Collections.ObjectModel;
using Windows.Storage;

namespace Site_Manager
{
    class CoreManager
    {

        public static ObservableCollection<CoreModule> Modules = new ObservableCollection<CoreModule>();
        public static bool ModulesLoaded = false;

        /// <summary>
        /// Loads previously saved modules into memory, does nothing if already loaded
        /// </summary>
        public static void Load()
        {
            if (ModulesLoaded)
                return; // already loaded

            try
            {
                ApplicationDataCompositeValue composite = SettingsManager.GetComposite(GlobalString.COMPOSITE_KEY_COREMODULES);

                if (composite == null)
                {
                    // create collection of empty modules
                    Modules = GetEmptyModules();
                    // save default modules
                    Save();
                    // don't load again, will be in memory
                    ModulesLoaded = true;
                    // don't bother reading from file, return
                    return;
                }

                // get all the keys
                string[] keys = new string[composite.Keys.Count];
                composite.Keys.CopyTo(keys, 0);
                
                // recursively load from composite (by tag)
                foreach (string key in keys)
                    Modules.Add(new CoreModule() { Code = (string)composite[key], Tag = key.Substring(GlobalString.COMPOSITE_KEY_COREMODULES_PREFIX.Length) });
                
                // don't bother loading again, will always be in memory (right?!)
                ModulesLoaded = true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }

        /// <summary>
        /// Saves modules that are currently in memory as a composite
        /// </summary>
        public static void Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();

            foreach (CoreModule module in Modules)
                composite[GlobalString.COMPOSITE_KEY_COREMODULES_PREFIX + module.Tag] = module.Code;

            SettingsManager.SetComposite(composite, GlobalString.COMPOSITE_KEY_COREMODULES);
        }

        private static ObservableCollection<CoreModule> GetEmptyModules()
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
            if (!ModulesLoaded)
                return null;
            foreach (CoreModule module in Modules)
                if (module.Tag.ToString() == tag)
                    return module;
            return null;
        }

    }
}