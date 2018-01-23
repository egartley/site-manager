using Windows.Storage;

namespace Site_Manager
{
    class DeveloperOptions
    {

        private static bool BlankDeploy, UseTestDirectory;
        private static bool Loaded = false;

        public static void Load()
        {
            if (Loaded)
            {
                Debug.Out("Developer options have already been loaded!", "WARNING");
            }
            ApplicationDataCompositeValue composite = SettingsManager.GetComposite(GlobalString.COMPOSITE_KEY_DEVOPTIONS);
            if (composite == null)
            {
                Debug.Out("Saving default values", "WARNING");
                // default values
                ApplicationDataCompositeValue defaultComposite = new ApplicationDataCompositeValue()
                {
                    [GlobalString.COMPOSITE_KEY_DEVOPTIONS_BLANKDEPLOY] = true,
                    [GlobalString.COMPOSITE_KEY_DEVOPTIONS_USETESTDIRECTORY] = true
                };

                SettingsManager.SetComposite(defaultComposite, GlobalString.COMPOSITE_KEY_DEVOPTIONS);
                composite = defaultComposite;
            }
            Debug.Out("Loading values from composite", "DEVELOPER OPTIONS");
            BlankDeploy = (bool)composite[GlobalString.COMPOSITE_KEY_DEVOPTIONS_BLANKDEPLOY];
            UseTestDirectory = (bool)composite[GlobalString.COMPOSITE_KEY_DEVOPTIONS_USETESTDIRECTORY];
            Loaded = true;
        }

        public static void Save()
        {
            Debug.Out("Saving to composite...", "DEVELOPER OPTIONS");
            SettingsManager.SetComposite(new ApplicationDataCompositeValue
            {
                [GlobalString.COMPOSITE_KEY_DEVOPTIONS_BLANKDEPLOY] = BlankDeploy,
                [GlobalString.COMPOSITE_KEY_DEVOPTIONS_USETESTDIRECTORY] = UseTestDirectory
            }, GlobalString.COMPOSITE_KEY_DEVOPTIONS);
        }

        public static void SetBlankDeploy(bool value)
        {
            Debug.Out($"Setting BlankDeploy to {value.ToString()}", "DEVELOPER OPTIONS");
            BlankDeploy = value;
        }

        public static void SetUseTestDirectory(bool value)
        {
            Debug.Out($"Setting UseTestDirectory to {value.ToString()}", "DEVELOPER OPTIONS");
            UseTestDirectory = value;
        }

        public static bool GetBlankDeploy()
        {
            if (!Loaded)
            {
                Load();
            }
            Debug.Out($"Returning BlankDeploy ({BlankDeploy.ToString()})", "DEVELOPER OPTIONS");
            return BlankDeploy;
        }

        public static bool GetUseTestDirectory()
        {
            if (!Loaded)
            {
                Load();
            }
            Debug.Out($"Returning UseTestDirectory ({UseTestDirectory.ToString()})", "DEVELOPER OPTIONS");
            return UseTestDirectory;
        }

    }
}