using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace Grandfather.Popups
{
    class BookmarkPopup : Popup
    {
        public override void ConstructWindow(Window window)
        {
            window.Title = "Bookmarks";
            ScaleAndCenter(70, 20);

            window.Add(new ListView(Program.Bookmarks) {
                Width = 50, Height = 13, X = 2, Y = 1, ColorScheme = Program.ViewScheme,
            });
            PlainButton add;
            window.Add(add = new PlainButton("Add page")
            {
                Width = 12, X = 54, Y = 14, ColorScheme = Program.ButtonScheme,
            });
            add.Clicked += () => {
                Close();
                new AddBookmarkPopup().Open();
            };
            PlainButton close;
            window.Add(close = new PlainButton("Close") {
                Width = 12, X = 54, Y = 16, ColorScheme = Program.ButtonScheme,
                Shortcut = Key.Esc,
            });
            close.Clicked += Close;
        }
    }
}
