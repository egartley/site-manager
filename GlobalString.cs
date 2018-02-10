namespace Site_Manager
{
    class GlobalString
    {
        public const string COMPOSITE_KEY_FTPCONFIG = "FileTransferProtocolConfiguration";
        public const string COMPOSITE_KEY_FTPCONFIG_USERNAME = "username";
        public const string COMPOSITE_KEY_FTPCONFIG_PASSWORD = "password";
        public const string COMPOSITE_KEY_FTPCONFIG_SERVER = "server";

        public const string COMPOSITE_KEY_COREMODULES = "CoreModules";
        public const string COMPOSITE_KEY_COREMODULES_PREFIX = "module_";

        public const string COMPOSITE_KEY_DEVOPTIONS = "DeveloperOptions";
        public const string COMPOSITE_KEY_DEVOPTIONS_BLANKDEPLOY = "blankdeploy";
        public const string COMPOSITE_KEY_DEVOPTIONS_USETESTDIRECTORY = "usetestdirectory";

        public const string FILENAME_WEBPAGES = "webpages.json";
        public const string FILENAME_CORE_MODULES = "modules.json";

        public const string TOOLTIP_DEVOPTIONS_BLANKDEPLOY = "Nothing will be deployed (useful for testing)";
        public const string TOOLTIP_DEVOPTIONS_USETESTDIRECTORY = "Everything will be deployed to \"/test\" instead of \"/\"";

        public static string DECODE_CHEVRON_UP = System.Net.WebUtility.HtmlDecode("&#xE972;");
        public static string DECODE_CHEVRON_DOWN = System.Net.WebUtility.HtmlDecode("&#xE971;");
    }
}