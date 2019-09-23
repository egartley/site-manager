namespace Site_Manager
{
    class HTMLBuilder
    {
        public static string GetRedirectionHTML(string destination)
        {
            Debug.Out($"Generating HTML for redirection to \"{destination}\"", "HTML BUILDER");
            return "<!DOCTYPE html><head>" + GlobalString.HTML_BUILDER_SIGNATURE + "<script type=\"text/javascript\">window.location=\"" + destination + "\"</script></head><body></body></html>";
        }

        public static string GetNavigationBarHTML(string pageurl)
        {
            if (pageurl.Equals("/"))
            {
                pageurl = "home";
            }
            else
            {
                pageurl += "/";
                pageurl = pageurl.Substring(1);
                pageurl = pageurl.Substring(0, pageurl.IndexOf("/"));
            }
            string lookfor = $"page=\"{pageurl}";
            string original = CoreManager.GetModuleByTag("navbar").Code;
            if (!original.Contains(lookfor))
            {
                return original.Replace("page=\"", "class=\"hitbox\" page=\"");
            }
            else
            {
                return original.Replace(lookfor, "class=\"hitbox active\" page=\"");
            }
        }

        public static string GetFullPageHTML(ManagedWebPage page)
        {
            Debug.Out($"Generating full HTML for \"{page.Title}\" ({page.RelativeURL})", "HTML BUILDER");
            // return "<!DOCTYPE html>\n<head>\n" + GlobalString.HTML_BUILDER_SIGNATURE + "\n<title>" + page.Title + " - egartley.net</title>\n" + CoreManager.GetModuleByTag("header").Code + "\n" + page.AdditionalHeaderHTML + "\n</head>\n<body>\n<div class=\"root-container\">\n<div class=\"container navbar-container\">\n" + GetNavigationBarHTML(page.RelativeURL.ToLower()) + "\n</div>\n<div class=\"container-container\">\n<div class=\"container pagecontent-container\">\n<div class=\"page-title\">\n<span>" + page.Title + "</span>\n</div\n><div class=\"page-meta\">\n<span class=\"secondary-text\">Last updated " + page.GetLastUpdatedAsString() + "</span>\n</div>\n<div class=\"page-content\">\n" + page.ContentHTML + "\n</div>\n</div>\n<div class=\"container sidebar-container\">\n" + CoreManager.GetModuleByTag("sidebar").Code + "\n</div>\n</div>\n</div>\n<div class=\"container footer-container\">\n" + CoreManager.GetModuleByTag("footer").Code + "\n</div>\n</body>\n</html>";
            return "<!DOCTYPE html>\n<head>\n" + GlobalString.HTML_BUILDER_SIGNATURE + "\n<title>" + page.Title + " - egartley.net</title>\n" + CoreManager.GetModuleByTag("header").Code + "\n" + page.AdditionalHeaderHTML + "\n</head>\n<body><div class=\"root-container\"><div class=\"navigation-bar-container\">" + GetNavigationBarHTML(page.RelativeURL.ToLower()) + "</div><div class=\"content-container\"><div class=\"page-container content-card\"><div class=\"page-content-container\"><span class=\"page-title\">" + page.Title + "</span><span class=\"gradient-spacer\"></span><span class=\"page-meta\">Last updated " + page.GetLastUpdatedAsString() + "</span><div class=\"content\">" + page.ContentHTML + "</div></div></div><div class=\"sidebar-container content-card\">" + CoreManager.GetModuleByTag("sidebar").Code + "</div></div><div class=\"footer-container\">" + CoreManager.GetModuleByTag("footer").Code + "</div></div></body></html>";
        }
    }
}