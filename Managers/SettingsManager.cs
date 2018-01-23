using System.Collections.Generic;
using Windows.Storage;

namespace Site_Manager
{
    class SettingsManager
    {

        private static ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public static void SetComposite(ApplicationDataCompositeValue composite, string key)
        {
            Debug.Out($"Setting composite \"{key}\" with {composite.Values.Count.ToString()} values", "SETTINGS MANAGER");
            LocalSettings.Values[key] = composite;
        }

        public static ApplicationDataCompositeValue GetComposite(string key)
        {
            Debug.Out($"Returning composite \"{key}\"", "SETTINGS MANAGER");
            return (ApplicationDataCompositeValue)LocalSettings.Values[key];
        }
    }
}
