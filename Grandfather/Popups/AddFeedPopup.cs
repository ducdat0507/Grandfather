using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using Grandfather.Data;

namespace Grandfather.Popups
{
    class AddFeedPopup : Popup
    {
        public override void ConstructWindow(Window window)
        {
            window.Title = "Add feed";
            ScaleAndCenter(56, 18);

            Controls.Tab tab = Program.Tabs[Program.CurrentTab];

            window.Add(new Label("Title") { X = 2, Y = 1 });
            TextField title;
            window.Add(title = new TextField(tab.Title)
            {
                X = 2,
                Y = 2,
                Width = 24,
                ColorScheme = Program.ViewFieldScheme
            });
            window.Add(new Label("Destination") { X = 28, Y = 1 });
            TextField link;
            window.Add(link = new TextField(tab.Destination)
            {
                X = 28,
                Y = 2,
                Width = 24,
                ColorScheme = Program.ViewFieldScheme
            });
            window.Add(new Label("Preview") { X = 2, Y = 4 });
            window.Add(new ListView(Feed.ParseFeed(tab.Output, tab.Uri))
            {
                X = 2,
                Y = 5,
                Width = 50,
                Height = 8,
                ColorScheme = Program.ViewScheme
            });
            PlainButton add;
            window.Add(add = new PlainButton(" Add ")
            {
                X = 37,
                Y = 14,
                ColorScheme = Program.ButtonScheme,
                Shortcut = Key.ShiftMask | Key.Enter,
            });
            add.Clicked += () => {
                Program.Bookmarks.Add(new Data.Bookmark(title.Text.ToString(), link.Text.ToString()));
                Close();
                new BookmarkPopup().Open();
            };
            PlainButton cancel;
            window.Add(cancel = new PlainButton(" Cancel ")
            {
                X = 44,
                Y = 14,
                ColorScheme = Program.ButtonScheme,
                Shortcut = Key.Esc,
            });
            cancel.Clicked += Close;
        }
    }
}
