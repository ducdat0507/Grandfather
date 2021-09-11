using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace Grandfather.Popups
{
    class AddBookmarkPopup : Popup
    {
        public override void ConstructWindow(Window window)
        {
            window.Title = "Add bookmark";
            ScaleAndCenter(56, 8);
            window.Add(new Label("Title") { X = 2, Y = 1 });
            TextField title;
            window.Add(title = new TextField(Program.Tabs[Program.CurrentTab].Title) { 
                X = 2, Y = 2, Width = 24, ColorScheme = Program.ViewFieldScheme 
            });
            window.Add(new Label("Destination") { X = 28, Y = 1 });
            TextField link;
            window.Add(link = new TextField(Program.Tabs[Program.CurrentTab].Destination) { 
                X = 28, Y = 2, Width = 24, ColorScheme = Program.ViewFieldScheme 
            });
            PlainButton add;
            window.Add(add = new PlainButton(" Add ") {
                X = 37, Y = 4, ColorScheme = Program.ButtonScheme,
                Shortcut = Key.ShiftMask | Key.Enter,
            });
            add.Clicked += () => {
                Program.Bookmarks.Add(new Data.Bookmark(title.Text.ToString(), link.Text.ToString()));
                Close();
                new BookmarkPopup().Open();
            };
            PlainButton cancel;
            window.Add(cancel = new PlainButton(" Cancel ") {
                X = 44, Y = 4, ColorScheme = Program.ButtonScheme,
                Shortcut = Key.Esc,
            });
            cancel.Clicked += Close;
        }
    }
}
