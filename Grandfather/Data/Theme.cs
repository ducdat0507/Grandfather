using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace Grandfather.Data
{
    [Serializable]
    public class Theme
    {

    }

    [Serializable]
    public class ColorPallete
    {
        public Color Background = Color.Black;
        public Color Foreground = Color.Black;

        public ColorPallete() { }

        public ColorPallete(Color back, Color fore) {
            Background = back;
            Foreground = fore;
        }
    }

}
