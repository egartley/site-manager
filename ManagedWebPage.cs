using System;
using System.Runtime.Serialization;

namespace Site_Manager
{
    [DataContract()]
    public class ManagedWebPage
    {
        [DataMember()]
        public string RelativeURL { get; set; }
        [DataMember()]
        public string ContentHTML { get; set; }
        [DataMember()]
        public string AdditionalHeaderHTML { get; set; }
        [DataMember()]
        public string Title { get; set; }
        [DataMember()]
        public DateTime LastUpdated { get; set; }
        [DataMember()]
        public DateTime LastSubmitted { get; set; }
        [DataMember()]
        public bool IsRoot { get; set; }

        public ManagedWebPage(string url)
        {
            RelativeURL = url;
            IsRoot = RelativeURL.LastIndexOf("/") == 0;
            ContentHTML = "";
            AdditionalHeaderHTML = "";
            Title = "Untitled";
        }

        public ManagedWebPage(string url, string html)
        {
            RelativeURL = url;
            IsRoot = RelativeURL.LastIndexOf("/") == 0;
            ContentHTML = html;
            AdditionalHeaderHTML = "";
            Title = "Untitled";
        }

        public void Updated() => LastUpdated = DateTime.Now;

        public void Submitted() => LastSubmitted = DateTime.Now;

        public string GetLastUpdatedAsString()
        {
            if (LastUpdated.Equals(DateTime.MinValue))
            {
                return "Never";
            }
            return LastUpdated.ToString("m") + ", " + LastUpdated.Year + " at " + LastUpdated.ToString("t");
        }

        public string GetLastSubmittedAsString()
        {
            if (LastSubmitted.Equals(DateTime.MinValue))
            {
                return "Never";
            }
            return LastSubmitted.ToString("m") + ", " + LastSubmitted.Year + " at " + LastSubmitted.ToString("t");
        }

    }
}