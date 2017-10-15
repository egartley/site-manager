namespace Site_Manager
{
    class CoreModule
    {
        public CoreModule()
        {
            Tag = "";
            Code = "";
        }

        public CoreModule(string s)
        {
            ParseFromString(s);
        }

        public string Code { get; set; }
        public string Tag { get; set; }
        public string GetAsString()
        {
            return Tag + GlobalString.CORE_MODULE_STRING_DELIMITER + System.Net.WebUtility.HtmlEncode(Code);
        }
        public void ParseFromString(string s)
        {
            if (s.IndexOf(GlobalString.CORE_MODULE_STRING_DELIMITER) == -1)
                return;
            Tag = s.Substring(0, s.IndexOf(GlobalString.CORE_MODULE_STRING_DELIMITER));
            Code = System.Net.WebUtility.HtmlDecode(s.Substring(s.IndexOf(GlobalString.CORE_MODULE_STRING_DELIMITER) + GlobalString.CORE_MODULE_STRING_DELIMITER.Length));
        }
    }
}