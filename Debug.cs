namespace Site_Manager
{
    class Debug
    {
        public static void Out(object o) => Out(o, "DEBUG");

        public static void Out(object o, string tag) => System.Diagnostics.Debug.WriteLine("[" + tag + "] " + o.ToString());

        public static void Out(System.Exception e) => System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
    }
}