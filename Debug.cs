namespace Site_Manager
{
    class Debug
    {
        public static void Out(object o) => System.Diagnostics.Debug.WriteLine("[DEBUG OUTPUT] " + o.ToString());

        public static void Out(object o, string tag) => System.Diagnostics.Debug.WriteLine("[" + tag + "] " + o.ToString());
    }
}