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

        public const string WEBPAGE_FILENAME = "webpages.dat";
        public const string CORE_MODULES_FILENAME = "modules.dat";

        public const string CORE_MODULE_STRING_DELIMITER = "[[]]";

        public static string DECODE_CHEVRON_UP = System.Net.WebUtility.HtmlDecode("&#xE972;");
        public static string DECODE_CHEVRON_DOWN = System.Net.WebUtility.HtmlDecode("&#xE971;");
    }
}