namespace Site_Manager
{
    class Debug
    {
        public static void Out(object o) => System.Diagnostics.Debug.WriteLine(o.ToString());
        public static void Out(object o, string tag) => Out("[" + tag + "] " + o.ToString());
    }
}