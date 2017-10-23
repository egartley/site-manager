namespace Site_Manager
{
    class Debug
    {
        public static void Out(object o) =>
#if DEBUG
            System.Diagnostics.Debug.WriteLine("[DEBUG OUTPUT] " + o.ToString());
#endif


        public static void Out(object o, string tag) =>
#if DEBUG
            System.Diagnostics.Debug.WriteLine("[" + tag + "] " + o.ToString());
#endif

    }
}