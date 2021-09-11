using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace Grandfather.Popups
{
    class Popup
    {
        public static Window ActiveWindow = null;

        public void Open()
        {
            if (ActiveWindow == null) {
                ActiveWindow = new Window()
                {
                    ColorScheme = Program.PopupScheme,
                    Modal = true,
                };
                ConstructWindow(ActiveWindow);
                Application.Top.Add(ActiveWindow);
            } 
            else
            {
                Application.Top.Add(ActiveWindow);
            }
        }

        public void Close()
        {
            if (ActiveWindow != null)
            {
                Application.Top.Remove(ActiveWindow);
                ActiveWindow = null;
            }
        }

        public virtual void ConstructWindow(Window window)
        {
            window.Title = "Popup";
            this.ScaleAndCenter(22, 7);
            window.Add(new Label("This is a popup.") { X = 2, Y = 1 });
            Button button;
            window.Add(button = new Button("OK") { X = 12, Y = 3, ColorScheme = Program.ButtonScheme });
            button.Clicked += () => this.Close();
        }

        public void ScaleAndCenter(int width, int height)
        {
            Application.Top.GetCurrentWidth(out int tWidth);
            Application.Top.GetCurrentHeight(out int tHeight);
            ActiveWindow.Width = width; ActiveWindow.X = (tWidth - width) / 2;
            ActiveWindow.Height = height; ActiveWindow.Y = (tHeight - height) / 2;
        }
    }
}
