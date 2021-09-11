using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Grandfather.Data
{
    class Feed
    {
        public string Title;
        public string Destination;

        public Feed(string title, string link)
        {
            Title = title;
            Destination = link;
        }

        public override string ToString()
        {
            return Title;
        }

        public static List<FeedEntry> ParseFeed(string content, Uri source)
        {
            MatchCollection matches = new Regex(@"^=>\s*([^\s]*)\s+(\d{4})-(\d{2})-(\d{2})\s[^\w]*(.*)", RegexOptions.Multiline).Matches(content);

            List<FeedEntry> entries = new List<FeedEntry>();

            foreach (Match match in matches)
            {
                string link = new Uri(source, match.Groups[1].Value).AbsoluteUri;
                int year = int.Parse(match.Groups[2].Value);
                int month = int.Parse(match.Groups[3].Value);
                int day = int.Parse(match.Groups[4].Value);
                DateTime date = new DateTime(year, month, day);
                string title = match.Groups[5].Value;

                entries.Add(new FeedEntry(title, link, date));
            }
            return entries;
        }
    }

    class FeedEntry
    {
        public string Title;
        public DateTime UpdateTime;
        public string Source;

        public FeedEntry(string title, string source, DateTime time)
        {
            Title = title;
            Source = source;
            UpdateTime = time;
        }

        public override string ToString()
        {
            string[] moy = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Nov", "Oct", "Nov", "Dec" };
            string time = UpdateTime.Day + " " + moy[UpdateTime.Month - 1] + " " + UpdateTime.Year;

            return time + " - " + Title;
        }

    }
}
