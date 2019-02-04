using System;

namespace Site_Manager
{
    public class ManagedWebPage
    {
        public string RelativeURL { get; set; }
        public string ContentHTML { get; set; }
        public string AdditionalHeaderHTML { get; set; }
        public string Title { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime LastSubmitted { get; set; }

        public ManagedWebPage()
        {
            RelativeURL = "";
            ContentHTML = "";
            AdditionalHeaderHTML = "";
            Title = "Untitled";
            LastUpdated = DateTime.MinValue;
            LastSubmitted = DateTime.MinValue;
        }

        public ManagedWebPage(string url)
        {
            RelativeURL = url;
            ContentHTML = "";
            AdditionalHeaderHTML = "";
            Title = "Untitled";
            LastUpdated = DateTime.MinValue;
            LastSubmitted = DateTime.MinValue;
        }

        public ManagedWebPage(string url, string html)
        {
            RelativeURL = url;
            ContentHTML = html;
            AdditionalHeaderHTML = "";
            Title = "Untitled";
            LastUpdated = DateTime.MinValue;
            LastSubmitted = DateTime.MinValue;
        }

        /// <summary>
        /// Sets LastUpdated to DateTime.Now
        /// </summary>
        public void Updated() => LastUpdated = DateTime.Now;

        /// <summary>
        /// Sets LastSubmitted to DateTime.Now
        /// </summary>
        public void Submitted() => LastSubmitted = DateTime.Now;

        private string GetDateTimeAsFormattedString(DateTime time)
        {
            return time.ToString("m") + ", " + time.Year + " at " + time.ToString("t");
        }

        /// <summary>
        /// Returns LastUpdate as a formatted string (ex. July 29, 2015 at 12:00 PM)
        /// </summary>
        /// <returns></returns>
        public string GetLastUpdatedAsString()
        {
            if (LastUpdated.Equals(DateTime.MinValue))
            {
                return "Never";
            }
            return GetDateTimeAsFormattedString(LastUpdated);
        }

        public string GetLastSubmittedAsString()
        {
            if (LastSubmitted.Equals(DateTime.MinValue))
            {
                return "Never";
            }
            return GetDateTimeAsFormattedString(LastSubmitted);
        }

        public string GetAsString() => Newtonsoft.Json.JsonConvert.SerializeObject(this);

    }
}