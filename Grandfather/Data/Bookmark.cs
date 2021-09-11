using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace Grandfather.Data
{
    class Bookmark
    {
        public string Title;
        public string Destination;

        public Bookmark(string title, string link)
        {
            Title = title;
            Destination = link;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
