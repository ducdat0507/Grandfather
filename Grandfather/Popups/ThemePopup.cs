using Grandfather.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace Grandfather.Popups
{
    class ThemePopup : Popup
    {
        public override void ConstructWindow(Window window)
        {
            window.Title = "Themes";
            ScaleAndCenter(50, 16);

            window.Add(new ListView(new[] { "Default" })
            {
                Width = 30,
                Height = 12,
                X = 2,
                Y = 1,
                ColorScheme = Program.ViewScheme,
            });
            ColorPicker ncolor;
            window.Add(ncolor = new ColorPicker(new Data.ColorPallete(Color.White, Color.Black))
            {
                Text = "Normal",
                X = 34,
                Y = 1,
            });
            ColorPicker fcolor;
            window.Add(fcolor = new ColorPicker(new Data.ColorPallete(Color.White, Color.Red))
            {
                Text = "Hotkey",
                X = 40,
                Y = 1,
            });
            PlainButton apply;
            window.Add(apply = new PlainButton("Apply")
            {
                Width = 12,
                X = 34,
                Y = 10,
                ColorScheme = Program.ButtonScheme,
            });
            PlainButton close;
            window.Add(close = new PlainButton("Close")
            {
                Width = 12,
                X = 34,
                Y = 12,
                ColorScheme = Program.ButtonScheme,
                Shortcut = Key.Esc,
            });
            close.Clicked += Close;
        }
    }
}
