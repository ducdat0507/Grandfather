using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace Grandfather.Popups
{
    class AboutPopup : Popup
    {
        public override void ConstructWindow(Window window)
        {
            window.Title = "About";
            ScaleAndCenter(40, 15);
            window.Add(new Label(
                "                                  \n" +
                "  ┬─┐           ┌┌┐  ┐ ┐          \n" +
                "  │─┐┬─┐┌─┐┬─┐┌─┤├┌─┐├ ├─┐┌─┐┬─┐  \n" +
                "  │ ││  ┌─┤│ ││ ││┌─┤│ │ │├─┘│    \n" +
                "  └─┴┴  └─┴┴ ┴└─┴┴└─┴└┘┴ ┴└─┘┴    \n" +
                "    a terminal smallweb client    \n" +
                "                                  "
            ){
                X = 2,
                Y = 1,
                Width = 34,
                Height = 7,
                ColorScheme = Program.TabScheme,
            });
            window.Add(new Label(
                "      created by ducdat0507       "
            )
            {
                X = 2,
                Y = 9,
                Width = 34,
                Height = 2,
            });
            PlainButton close;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            window.Add(close = new PlainButton(" Close ")
            {
                X = 29,
                Y = 11,
                ColorScheme = Program.ButtonScheme,
                Shortcut = Key.Esc,
            });
            close.Clicked += Close;
        }
    }
}
