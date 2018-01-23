namespace Site_Manager
{
    class CoreModule
    {
        public CoreModule()
        {
            Tag = "";
            Code = "";
        }
        public CoreModule(string tag)
        {
            Tag = tag;
            Code = "";
        }
        public CoreModule(string tag, string code)
        {
            Tag = tag;
            Code = code;
        }
        public string Code { get; set; }
        public string Tag { get; set; }
        public string GetAsString() => Newtonsoft.Json.JsonConvert.SerializeObject(this);
    }
}