using System;
using System.Collections.Generic;
using Terminal.Gui;
using System.Diagnostics;

namespace Grandfather.Controls
{
    class Tab : Worker
    {
        public string Title = "";
        public PlainButton TabButton;

        public List<string> History = new List<string>();
        public int Current;
        public int Redirects;

        public Tab(string url)
        {
            Navigate(url);
        }

        public void Navigate(string url, int redirects = 0)
        {
            if (redirects == 0)
            {
                if (Current != History.Count - 1 && History.Count > 0)
                    History.RemoveRange(Current + 1, History.Count - Current - 1);
                History.Add(url);
                Current = History.Count - 1;
            }
            Redirects = redirects;
            base.Navigate(url);
        }

        public bool CanGoBack()
        {
            return History.Count > 0 && Current > 0;
        }
        public void GoBack()
        {
            if (CanGoBack())
            {
                Current--;
                Navigate(History[Current], -1);
                Output = "\nConnecting to " + new Uri(History[Current]).Host + " . . .";
            }
        }
        public bool CanGoForward()
        {
            return History.Count > 0 && Current < History.Count - 1;
        }
        public void GoForward()
        {
            if (CanGoForward())
            {
                Current++;
                Navigate(History[Current], -1);
                Output = "\nConnecting to " + new Uri(History[Current]).Host + " . . .";
            }
        }
        public void Reload()
        {
            Navigate(History[Current], -1);
            Output = "\nConnecting to " + new Uri(History[Current]).Host + " . . .";
        }

        public static void OpenExternalUri(string url)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = url;
            p.Start();
        }
    }
}
