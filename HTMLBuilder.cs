namespace Site_Manager
{
    class HTMLBuilder
    {
        public static string GetRedirectionHTML(string destination)
        {
            Debug.Out($"Returning HTML for redirection to \"{destination}\"", "HTML BUILDER");
            return "<!DOCTYPE html><head>" + GlobalString.HTML_BUILDER_SIGNATURE + "<script type=\"text/javascript\">window.location=\"" + destination + "\"</script></head><body></body></html>";
        }

        public static string GetNavigationBarHTML(string pageurl)
        {
            // lazy way to handle determining what nav item is active if any at all
            // dependent upon the inputted nav code, doesn't work without it being specifically configured
            // should probably be more dynamic in the future, but it works now, so...
            if (pageurl.Equals("/"))
            {
                pageurl = "home";
            }
            else
            {
                // lazy fix (again)
                pageurl += "/";
                // /views/conspiracies/
                pageurl = pageurl.Substring(1);
                // views/conspiracies
                pageurl = pageurl.Substring(0, pageurl.IndexOf("/"));
                // pageurl is now "views" (hopefully)
            }
            string anchor = "class=\"nav-item ";
            string modifiedcode = CoreManager.GetModuleByTag("navbar").Code;
            string returncode = modifiedcode.Substring(0, modifiedcode.IndexOf(anchor));
            string originalcode = modifiedcode;

            if (originalcode.IndexOf(pageurl) == -1)
            {
                // don't bother continuing, won't be in the nav bar
                return originalcode;
            }

            while (modifiedcode.IndexOf(anchor) != -1)
            {
                modifiedcode = modifiedcode.Substring(modifiedcode.IndexOf(anchor) + anchor.Length);
                string titleClass = modifiedcode.Substring(0, modifiedcode.IndexOf("\">")).ToLower();
                if (titleClass == pageurl)
                {
                    returncode = originalcode.Replace(anchor + titleClass, anchor + "active");
                    break;
                }
            }
            return returncode;
        }

        public static string GetFullPageHTML(ManagedWebPage page)
        {
            Debug.Out($"Returning HTML for page \"{page.Title}\" ({page.RelativeURL})", "HTML BUILDER");
            return "<!DOCTYPE html>\n<head>\n" + GlobalString.HTML_BUILDER_SIGNATURE + "\n<title>" + page.Title + " - egartley.net</title>\n" + CoreManager.GetModuleByTag("header").Code + "\n" + page.AdditionalHeaderHTML + "\n</head>\n<body>\n<div class=\"root-container\">\n<div class=\"container navbar-container\">\n" + GetNavigationBarHTML(page.RelativeURL.ToLower()) + "\n</div>\n<div class=\"container-container\">\n<div class=\"container pagecontent-container\">\n<div class=\"page-title\">\n<span>" + page.Title + "</span>\n</div\n><div class=\"page-meta\">\n<span class=\"secondary-text\">Last updated " + page.GetLastUpdatedAsString() + "</span>\n</div>\n<div class=\"page-content\">\n" + page.ContentHTML + "\n</div>\n</div>\n<div class=\"container sidebar-container\">\n" + CoreManager.GetModuleByTag("sidebar").Code + "\n</div>\n</div>\n<div class=\"container footer-container\">\n" + CoreManager.GetModuleByTag("footer").Code + "\n</div>\n</div>\n</body>\n</html>";
        }
    }
}