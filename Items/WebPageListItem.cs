namespace Site_Manager
{
    class WebPageListItem
    {

        public string PageTitle { get; set; }
        public string URL { get; set; }
        public string LastModified { get; set; }

        public WebPageListItem(string title, string url, string lastModified)
        {
            PageTitle = title;
            URL = url;
            LastModified = lastModified;
        }

    }
}