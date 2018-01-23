namespace Site_Manager
{
    class WebPageListItem
    {

        public string PageTitle { get; set; }
        public string URL { get; set; }
        public string LastUpdated { get; set; }

        public WebPageListItem(string title, string url, string lastUpdated)
        {
            PageTitle = title;
            URL = url;
            LastUpdated = lastUpdated;
        }

    }
}