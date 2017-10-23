using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Site_Manager
{
    class Utils
    {

        private static Random Random = new Random();
        public static bool IsValidURL(string s) => new Regex(@"^[a-zA-Z0-9-_/]+$").IsMatch(s);
        public static bool IsProtectedDirectory(string s) => s.Equals("cgi-bin") || s.Equals(".well-known") || s.Equals("error");
        public static string RandomString(int length) => new string(Enumerable.Repeat("0123456789abcdefghijklmnopqrstuvwxyz", length).Select(s => s[Random.Next(s.Length)]).ToArray());

    }
}