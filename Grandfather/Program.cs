using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terminal.Gui;
using Grandfather.Controls;

namespace Grandfather
{
    class Program
    {
        public static List<Tab> Tabs = new List<Tab>();
        public static int CurrentTab = 0;

        static string QueryText = "";
        static string Current;

        public static List<Data.Bookmark> Bookmarks = new List<Data.Bookmark>();

        static string StatusText = "";
        static Label Background;
        static Label Status;
        static PlainButton NewTabButton;
        static TextField PathField;
        static ScrollView MainView;
        static ScrollView Sidebar;

        static bool ShowSidebar;
        static bool KeyboardInteracted;

        public static ColorScheme TitleBarScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.White, Color.DarkGray),
            HotNormal = new Terminal.Gui.Attribute(Color.BrightYellow, Color.DarkGray),
            Focus = new Terminal.Gui.Attribute(Color.White, Color.Blue),
            HotFocus = new Terminal.Gui.Attribute(Color.BrightYellow, Color.Blue),
        };

        public static ColorScheme ViewScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.Black, Color.Gray),
            HotNormal = new Terminal.Gui.Attribute(Color.Blue, Color.Gray),
            Focus = new Terminal.Gui.Attribute(Color.Black, Color.BrightBlue),
            HotFocus = new Terminal.Gui.Attribute(Color.Brown, Color.BrightBlue),
        };

        public static ColorScheme ViewFieldScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.Black, Color.White),
            HotNormal = new Terminal.Gui.Attribute(Color.Blue, Color.White),
            Focus = new Terminal.Gui.Attribute(Color.Black, Color.White),
            HotFocus = new Terminal.Gui.Attribute(Color.Blue, Color.White),
        };

        public static ColorScheme PreScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.White, Color.DarkGray),
            HotNormal = new Terminal.Gui.Attribute(Color.BrightBlue, Color.DarkGray),
            Focus = new Terminal.Gui.Attribute(Color.White, Color.DarkGray),
            HotFocus = new Terminal.Gui.Attribute(Color.BrightBlue, Color.DarkGray),
        };

        public static ColorScheme PreHeaderScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.Black, Color.White),
            HotNormal = new Terminal.Gui.Attribute(Color.Blue, Color.White),
            Focus = new Terminal.Gui.Attribute(Color.Black, Color.White),
            HotFocus = new Terminal.Gui.Attribute(Color.Blue, Color.White),
        };

        static ColorScheme BodyScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.DarkGray, Color.Gray),
            HotNormal = new Terminal.Gui.Attribute(Color.Blue, Color.Gray),
            Focus = new Terminal.Gui.Attribute(Color.DarkGray, Color.Blue),
            HotFocus = new Terminal.Gui.Attribute(Color.Brown, Color.Blue),
        };

        public static ColorScheme LinkScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.BrightBlue, Color.Gray),
            HotNormal = new Terminal.Gui.Attribute(Color.Brown, Color.Gray),
            Focus = new Terminal.Gui.Attribute(Color.White, Color.Blue),
            HotFocus = new Terminal.Gui.Attribute(Color.BrightYellow, Color.Blue),
            Disabled = new Terminal.Gui.Attribute(Color.Black, Color.White),
        };

        public static ColorScheme TabScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.White, Color.Blue),
            HotNormal = new Terminal.Gui.Attribute(Color.BrightYellow, Color.Blue),
            Focus = new Terminal.Gui.Attribute(Color.White, Color.BrightBlue),
            HotFocus = new Terminal.Gui.Attribute(Color.BrightYellow, Color.BrightBlue),
            Disabled = new Terminal.Gui.Attribute(Color.BrightBlue, Color.Blue),
        };

        public static ColorScheme PopupScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.Black, Color.Cyan),
            HotNormal = new Terminal.Gui.Attribute(Color.BrightYellow, Color.Cyan),
            Focus = new Terminal.Gui.Attribute(Color.White, Color.BrightBlue),
            HotFocus = new Terminal.Gui.Attribute(Color.BrightYellow, Color.Blue),
            Disabled = new Terminal.Gui.Attribute(Color.Gray, Color.Cyan),
        };

        public static ColorScheme ButtonScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.Black, Color.Gray),
            HotNormal = new Terminal.Gui.Attribute(Color.Red, Color.Gray),
            Focus = new Terminal.Gui.Attribute(Color.DarkGray, Color.Gray),
            HotFocus = new Terminal.Gui.Attribute(Color.Red, Color.Gray),
            Disabled = new Terminal.Gui.Attribute(Color.Gray, Color.Cyan),
        };

        static void Main(string[] args)
        {
            ConsoleColor col = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("                                  ");
            Console.WriteLine("  ┬─┐           ┌┌┐  ┐ ┐          ");
            Console.WriteLine("  │─┐┬─┐┌─┐┬─┐┌─┤├┌─┐├ ├─┐┌─┐┬─┐  ");
            Console.WriteLine("  │ ││  ┌─┤│ ││ ││┌─┤│ │ │├─┘│    ");
            Console.WriteLine("  └─┴┴  └─┴┴ ┴└─┴┴└─┴└┘┴ ┴└─┘┴    ");
            Console.WriteLine("    a terminal smallweb client    ");
            Console.WriteLine("  v0.1                         ¤  ");
            Console.WriteLine("                                  ");
            Console.BackgroundColor = col;

            Console.Title = "Grandfather v0.1";
            Application.Init();
            Application.Top.Add(new MenuBar(new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("New _tab", "", () => NewTab("gemini://gemini.circumlunar.space"), shortcut: Key.CtrlMask | Key.T),
                    new MenuItem("_Close tab", "", () => CloseTab(Tabs[CurrentTab]), () => Tabs.Count > 1, shortcut: Key.CtrlMask | Key.W),
                    new MenuItem("_Previous tab", "", () => CurrentTab = (CurrentTab + Tabs.Count - 1) % Tabs.Count, () => Tabs.Count > 1, shortcut: Key.CtrlMask | Key.CursorLeft),
                    new MenuItem("_Next tab", "", () => CurrentTab = (CurrentTab + 1) % Tabs.Count, () => Tabs.Count > 1, shortcut: Key.CtrlMask | Key.CursorRight),
                    null,
                    new MenuItem("_Go back", "", () => Tabs[CurrentTab].GoBack(), () => Tabs.Count > 0 && Tabs[CurrentTab].CanGoBack(), shortcut: Key.AltMask | Key.CursorLeft),
                    new MenuItem("_Go forward", "", () => Tabs[CurrentTab].GoForward(), () => Tabs.Count > 0 && Tabs[CurrentTab].CanGoForward(), shortcut: Key.AltMask | Key.CursorRight),
                    new MenuItem("_Reload", "", () => Tabs[CurrentTab].Reload(), shortcut: Key.CtrlMask | Key.R),
                    null,
                    new MenuItem("_Quit Grandfather", "", Application.RequestStop, shortcut: Key.CtrlMask | Key.Q),
                }),
                new MenuBarItem("_View", new MenuItem[] {
                    new MenuItem("Table of _contents", "", () => { ShowSidebar = !ShowSidebar; UpdateUI(); }),
                    /*null,
                    new MenuItem("_Themes...", "", new Popups.ThemePopup().Open),*/
                }),
                /*new MenuBarItem("_Bookmarks", new MenuItem[] {
                    new MenuItem("Bookmark _page...", "", new Popups.AddBookmarkPopup().Open, shortcut: Key.CtrlMask | Key.D),
                    new MenuItem("_Bookmarks...", "", new Popups.BookmarkPopup().Open, shortcut: Key.CtrlMask | Key.B),
                    null,
                    new MenuItem("_Subscribe to page...", "", new Popups.AddFeedPopup().Open, shortcut: Key.CtrlMask | Key.AltMask | Key.D),
                    new MenuItem("_Feeds...", "", null),
                    null,
                    new MenuItem("_History...", "", null, shortcut: Key.CtrlMask | Key.H),
                }),
                new MenuBarItem("_Options", new MenuItem[] {
                    new MenuItem("_Navigation...", "", null),
                }),*/
                new MenuBarItem("_Help", new MenuItem[] {
                    new MenuItem("_About Grandfather...", "", new Popups.AboutPopup().Open),
                }),
            }){
                ColorScheme = TabScheme,
            });
            
            Application.Top.Add(new Label ("═[ ")
            {
                ColorScheme = ViewFieldScheme,
                Y = 2,
            });
            Application.Top.Add(new Label(" ]═")
            {
                ColorScheme = ViewFieldScheme,
                Width = 3,
                Height = 1,
                X = Pos.Right(Application.Top) - 3,
                Y = 2,
            });
            Background = new Label(" ")
            {
                ColorScheme = TabScheme,
                Width = Application.Top.Width,
                Height = 1,
                Y = 1,
            };
            Application.Top.Add(Background);
            NewTabButton = new PlainButton(" + ")
            {
                ColorScheme = TabScheme,
                Width = 3,
                Height = 1,
                Y = 1,
            };
            NewTabButton.Clicked += () => NewTab("gemini://gemini.circumlunar.space");
            Application.Top.Add(NewTabButton);
            PathField = new TextField()
            {
                ColorScheme = ViewFieldScheme,
                Width = Application.Top.Width - 6,
                Height = 1,
                X = 3,
                Y = 2,
            };
            PathField.KeyDown += (key) => {
                if (key.KeyEvent.Key == Key.Enter)
                {
                    string uri = PathField.Text.ToString();
                    Tabs[CurrentTab].Navigate(uri);
                    Tabs[CurrentTab].Output = "\nConnecting to " + new Uri(uri).Host + " . . .";
                }
            };
            Application.Top.Add(PathField);

            MainView = new ScrollView()
            {
                ColorScheme = ViewScheme,
                Width = Application.Top.Width,
                Height = Dim.Fill() - 1,
                X = 0,
                Y = 3,
                AutoHideScrollBars = false,
                ShowVerticalScrollIndicator = true,
            };
            Application.Top.Add(MainView);
            Sidebar = new ScrollView()
            {
                ColorScheme = ViewScheme,
                Width = 0,
                Height = Dim.Fill() - 1,
                X = 0,
                Y = 3,
                AutoHideScrollBars = false,
                ShowVerticalScrollIndicator = true,
            };
            Application.Top.Add(Sidebar);
            Application.Resized += OnWindowResize;
            Application.Iteration += () =>
            {
                if (Tabs[CurrentTab].Output != Current)
                {
                    SetDisplay(Tabs[CurrentTab].Output, Tabs[CurrentTab]);
                    MainView.ScrollUp(999999999);
                    Application.Refresh();
                    Current = Tabs[CurrentTab].Output;
                }

                int width = 0;
                Application.Top.GetCurrentWidth(out width);
                Background.Text = new string(' ', width);
                Background.Width = width;
                Status.Width = width;
                string status = StatusText.PadRight(width * 3 / 4);
                Status.Text = status + "│" + Tabs[CurrentTab].Type;
                width = Math.Max(Math.Min((width - 5) / Tabs.Count - 1, 20), 3);


                for (int a = 0; a < Tabs.Count; a++)
                {
                    Tab tab = Tabs[a];
                    tab.TabButton.ColorScheme = a == CurrentTab ? ViewFieldScheme : TabScheme;
                    tab.TabButton.Text = width <= 5 ? "" + a : 
                        tab.Title.Length > width ? tab.Title.Substring(0, width - 3) + "..." : tab.Title;
                    tab.TabButton.Shortcut = a > 9 ? Key.Null : Key.CtrlMask | (Key)"1234567890"[a];

                    int pos = (width + 1) * a;
                    Background.Text = Background.Text.ToString().Substring(0, pos) + 
                        (a == CurrentTab ? "▐" : a - 1 == CurrentTab ? "▌" : "│")
                        + Background.Text.Substring(pos + 1);
                    tab.TabButton.X = pos + 1;
                    tab.TabButton.Width = width;
                }
                {
                    int pos = (width + 1) * Tabs.Count;
                    Background.Text = Background.Text.ToString().Substring(0, pos) +
                        (Tabs.Count - 1 == CurrentTab ? "▌" : "│")
                        + Background.Text.Substring(pos + 1);
                    NewTabButton.X = pos + 1;
                }
            };
            Application.Top.Add(Status = new Label(" "){
                Width = Application.Top.Width,
                Y = Pos.Bottom(Application.Top) - 1,
                ColorScheme = ViewFieldScheme,
            });
            MainView.KeyDown += (e) =>
            {
                Key key = e.KeyEvent.Key;
                if (key == Key.CursorUp || key == Key.CursorDown)
                {
                    e.Handled = true;
                }
            };
            Application.Current.KeyDown += (e) =>
            {
            };

            NewTab("gemini://gemini.circumlunar.space");
            Application.Run();
        }

        static void UpdateUI()
        {
            Application.Top.GetCurrentWidth(out int width);
            Application.Top.GetCurrentHeight(out int height);

            PathField.Width = Application.Top.Width - 6;
            int sbWidth = ShowSidebar ? width / 4 + 2 : 0;
            Sidebar.Width = sbWidth;
            MainView.X = sbWidth;
            
            try { 
                MainView.Width = width - sbWidth;
            }
            catch { }

            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight);
            if (Tabs.Count > 0) SetDisplay(Tabs[CurrentTab].Output, Tabs[CurrentTab]);
        }

        private static void OnWindowResize(Application.ResizedEventArgs obj)
        {
            UpdateUI();
        }

        static void NewTab(string url)
        {
            Tab tab = new Tab(url);
            tab.Output = "";
            tab.OnNavigate += OnTabNavigate;
            tab.OnSuccess += OnTabSuccess;
            tab.OnError += OnTabError;
            Tabs.Add(tab);
            PlainButton button = new PlainButton(new Uri(url).Host)
            {
                X = 1,
                Y = 1,
                Width = 18,
                Height = 1,
                ColorScheme = ViewFieldScheme,
            }; 
            Action action = () =>
            {
                SetTab(tab);
            };
            button.ShortcutAction += action;
            button.Clicked += action;
            Application.Top.Add(button);
            tab.TabButton = button;
            SetTab(tab);
        }

        static void SetTab(Tab tab)
        {
            CurrentTab = Tabs.IndexOf(tab);
            PathField.Text = tab.Destination;
        }

        static void CloseTab(Tab tab)
        {
            if (Tabs.Count <= 1) return;
            int index = Tabs.IndexOf(tab);
            Application.Top.Remove(tab.TabButton);
            Tabs.Remove(tab);
            if (CurrentTab > index) CurrentTab--;
            else CurrentTab = Math.Min(CurrentTab, Tabs.Count - 1);
        }

        static void OnTabNavigate(object sender, EventArgs e)
        {
            Tab tab = ((Tab)sender);
            PathField.Text = tab.Destination;
            tab.Title = tab.Uri.Host;
        }

        static void OnTabSuccess(object sender, EventArgs e)
        {
            Tab tab = (Tab)sender;
            Application.MainLoop.Invoke(Application.Refresh);
        }

        static void OnTabError(object sender, EventArgs e)
        {
            Tab tab = (Tab)sender;
            tab.Output = "There was an error trying to connect to " + tab.Uri.Host + ": " + ((UnhandledExceptionEventArgs)e).ExceptionObject;
            tab.Type = DocumentType.System;
            Application.Refresh();
        }

        static void SetDisplay(string data, Tab tab)
        {
            MainView.RemoveAll();
            Sidebar.RemoveAll();
            int height = 0;
            int sheight = 1;
            data = data.Replace("\r\n", "\n");

            int scrWidth = MainView.Frame.Width - 2;
            int conWidth = scrWidth * 3 / 4;

            int sbWidth = Sidebar.Frame.Width;

            string tabTitle = "";

            bool pre = false;

            if (tab.Type == DocumentType.Gemini)
            {
                foreach (string line in data.Split("\n"))
                {
                    int lines = 1;

                    if (height == 0)
                    {
                        height = 1;
                        if (line != "")
                        {
                            if (!new Regex(@"\d\d *").Match(line).Success)
                            {
                                string title = "Broken Header";
                                string body = "The server did not sent a valid Gemini header. This may be caused by the server malfunctioning, or the server isn't a Gemini server.";
                                MainView.Add(new Label(WrapString(title, conWidth, out lines))
                                {
                                    X = (scrWidth - conWidth) / 2 + 1,
                                    Y = height,
                                    Width = conWidth,
                                });
                                height += lines + 1;
                                MainView.Add(new Label(WrapString(body, conWidth, out lines))
                                {
                                    X = (scrWidth - conWidth) / 2 + 1,
                                    Y = height,
                                    Width = conWidth,
                                    ColorScheme = BodyScheme,
                                });
                                height += lines + 1;

                                break;
                            }

                            string status = line.Substring(0, 2).Trim();
                            string meta = line.Remove(0, 2).Trim();

                            // Input
                            if (status[0] == '1')
                            {
                                MainView.Add(new Label(WrapString(status + ": Please enter input.", conWidth, out lines))
                                {
                                    X = (scrWidth - conWidth) / 2 + 1,
                                    Y = height,
                                    Width = conWidth,
                                });
                                height += lines + 1;
                                MainView.Add(new Label(WrapString(meta, conWidth, out lines))
                                {
                                    X = (scrWidth - conWidth) / 2 + 1,
                                    Y = height,
                                    Width = conWidth,
                                    ColorScheme = BodyScheme,
                                });
                                height += lines + 1;

                                TextField QueryField = new TextField(QueryText)
                                {
                                    ColorScheme = ViewFieldScheme,
                                    X = (scrWidth - conWidth) / 2 + 1,
                                    Y = height,
                                    Width = conWidth,
                                };
                                QueryField.KeyDown += (key) =>
                                {
                                    if (key.KeyEvent.Key == Key.Enter)
                                    {
                                        string uri = Tabs[CurrentTab].Uri.AbsoluteUri + "?" + Uri.EscapeUriString(QueryText);
                                        Tabs[CurrentTab].Navigate(uri);
                                        Tabs[CurrentTab].Output = "\nConnecting to " + new Uri(uri).Host + " . . .";
                                        QueryText = "";
                                    }
                                    else
                                    {
                                        QueryText = QueryField.Text.ToString() + ((char)key.KeyEvent.Key);
                                    }
                                };
                                MainView.Add(QueryField);
                                height += 1;
                                break;
                            }

                            // OK
                            if (status[0] == '2')
                            {
                                continue;
                            }

                            // Redirect
                            if (status[0] == '3')
                            {
                                Uri uri;
                                string title = "Malformed redirect";
                                string body = meta;
                                if (Uri.TryCreate(Tabs[CurrentTab].Uri, meta, out uri)) { 
                                    if (uri == Tabs[CurrentTab].Uri)
                                    {
                                        body = "This link seems to redirected to itself. Is this black magic or a server fault?";
                                    }
                                    else if (Tabs[CurrentTab].Redirects >= 5)
                                    {
                                        title = "Too many redirects";
                                        body = "This link redirected you too many times. You may have been caught into a redirect loop.";
                                    }
                                    else
                                    {
                                        Tabs[CurrentTab].Navigate(uri.AbsoluteUri, Tabs[CurrentTab].Redirects + 1);
                                        break;
                                    }
                                }
                                MakeHeader(title, out lines, scrWidth, conWidth, height);
                                height += lines + 1;
                                MainView.Add(new Label(WrapString(body, conWidth, out lines))
                                {
                                    X = (scrWidth - conWidth) / 2 + 1,
                                    Y = height,
                                    Width = conWidth,
                                    ColorScheme = BodyScheme,
                                });
                                height += lines + 1;

                                break;
                            }

                            // Temporary failure
                            if (status[0] == '4')
                            {
                                string title = "An unexpected error happened";
                                string body = meta;

                                switch (status[1])
                                {
                                    case '1': title = "Server is currently unavailable"; break;
                                    case '2': title = "A CGI error happened"; break;
                                    case '3': title = "A proxy error happened"; break;
                                    case '4': title = "Rate limited"; break;
                                }

                                MakeHeader(title, out lines, scrWidth, conWidth, height);
                                height += lines + 1;
                                MainView.Add(new Label(WrapString(body, conWidth, out lines))
                                {
                                    X = (scrWidth - conWidth) / 2 + 1,
                                    Y = height,
                                    Width = conWidth,
                                    ColorScheme = BodyScheme,
                                });
                                height += lines + 1;

                                break;
                            }

                            // Permanent failure
                            if (status[0] == '5')
                            {
                                string title = "An unexpected error happened.";
                                string body = meta;

                                switch (status[1])
                                {
                                    case '1': title = "Content not found"; break;
                                    case '2': title = "Content is no longer available"; break;
                                    case '3': title = "Server does not accept proxy requests"; break;
                                    case '9': title = "Bad request"; break;
                                }

                                MakeHeader(title, out lines, scrWidth, conWidth, height);
                                height += lines + 1;
                                MainView.Add(new Label(WrapString(body, conWidth, out lines))
                                {
                                    X = (scrWidth - conWidth) / 2 + 1,
                                    Y = height,
                                    Width = conWidth,
                                    ColorScheme = BodyScheme,
                                });
                                height += lines + 1;

                                break;
                            }

                            // Permanent failure
                            if (status[0] == '6')
                            {
                                string title = "Client authentication required";
                                string body = meta;

                                switch (status[1])
                                {
                                    case '1': title = "You do not have access to this content"; break;
                                    case '2': title = "Invalid client authentication"; break;
                                }

                                MakeHeader(title, out lines, scrWidth, conWidth, height);
                                height += lines + 1;
                                MainView.Add(new Label(WrapString(body, conWidth, out lines))
                                {
                                    X = (scrWidth - conWidth) / 2 + 1,
                                    Y = height,
                                    Width = conWidth,
                                    ColorScheme = BodyScheme,
                                });
                                height += lines + 1;

                                break;
                            }

                            {
                                string title = "Status machine broke";
                                string body = meta;

                                MakeHeader(title, out lines, scrWidth, conWidth, height);
                                height += lines + 1;
                                MainView.Add(new Label(WrapString(body, conWidth, out lines))
                                {
                                    X = (scrWidth - conWidth) / 2 + 1,
                                    Y = height,
                                    Width = conWidth,
                                    ColorScheme = BodyScheme,
                                });
                                height += lines + 1;

                                break;
                            }
                        }
                    }

                    Match preMatch = new Regex(@"^```(.*)").Match(line);
                    if (preMatch.Success)
                    {
                        pre = !pre;
                        continue;
                    }

                    if (pre)
                    {
                        MainView.Add(new Label(line + " ")
                        {
                            X = (scrWidth - conWidth) / 2 + 1,
                            Y = height,
                            Width = conWidth,
                            ColorScheme = PreScheme,
                        });

                        height += lines;
                        continue;
                    }

                    Match titleMatch = new Regex(@"^(#{1,3})\s*(.+)").Match(line);
                    if (titleMatch.Success)
                    {
                        int ti = titleMatch.Groups[1].Length;
                        string value = titleMatch.Groups[2].Value;

                        int pos = height - 1;

                        if (ti == 1)
                        {
                            MainView.Add(new Label(WrapString(value, conWidth, out lines) + "\n" + new string('─', Math.Min(value.Length + 2, conWidth)))
                            {
                                X = (scrWidth - conWidth) / 2 + 1,
                                Y = height,
                                Width = conWidth,
                                TextAlignment = TextAlignment.Centered,
                            });
                            height += lines + 1;
                            if (string.IsNullOrWhiteSpace(tabTitle))
                            {
                                tab.Title = tabTitle = value;
                            }
                        }
                        else
                        {
                            MainView.Add(new Label(new string('#', ti))
                            {
                                X = (scrWidth - conWidth) / 2 - ti,
                                Y = height,
                                ColorScheme = BodyScheme,
                            });
                            MainView.Add(new Label(WrapString(value, conWidth, out lines))
                            {
                                X = (scrWidth - conWidth) / 2 + 1,
                                Y = height,
                                Width = conWidth,
                            });
                            height += lines;
                        }

                        if (ShowSidebar)
                        {
                            PlainButton pb;
                            Sidebar.Add(pb = new PlainButton(value)
                            {
                                X = ti * 2 - 1,
                                Y = sheight,
                                Width = sbWidth - ti * 2 - 1,
                                TextAlignment = TextAlignment.Left,
                                Data = pos,
                            });
                            int spos = sheight + 0;
                            pb.Enter += (e) =>
                            {
                                int hi = pb.Frame.Height;
                                int shi = Sidebar.Frame.Height;
                                if (spos < -Sidebar.ContentOffset.Y)
                                    Sidebar.ContentOffset = new Point(Sidebar.ContentOffset.X, -spos);
                                if (spos + hi > -Sidebar.ContentOffset.Y + shi)
                                    Sidebar.ContentOffset = new Point(Sidebar.ContentOffset.X, -spos - hi + shi);
                                MainView.ContentOffset = new Point(MainView.ContentOffset.X, (int)pb.Data);
                            };
                            pb.KeyDown += (e) =>
                            {
                                if (e.KeyEvent.Key == Key.Enter) MainView.SetFocus();
                            };

                            sheight++;
                        }

                        continue;
                    }


                    Match bulletMatch = new Regex(@"^\*[\s]*(.+)").Match(line);
                    if (bulletMatch.Success)
                    {
                        string title = bulletMatch.Groups[1].Value;
                        MainView.Add(new Label("•")
                        {
                            X = (scrWidth - conWidth) / 2 + 1,
                            Y = height,
                            ColorScheme = BodyScheme,
                        });
                        MainView.Add(new Label(WrapString(title, conWidth, out lines))
                        {
                            X = (scrWidth - conWidth) / 2 + 3,
                            Y = height,
                            Width = conWidth - 2,
                            ColorScheme = BodyScheme,
                        });
                        height += lines;
                        continue;
                    }

                    Match linkMatch = new Regex(@"^=>\s*([^\s]+)\s*(.*)").Match(line);
                    if (linkMatch.Success)
                    {
                        string url = linkMatch.Groups[1].Value;
                        Uri realUri = new Uri(Tabs[CurrentTab].Uri, url);
                        string tag = linkMatch.Groups[2].Value;
                        MakeLinkButton(realUri, string.IsNullOrWhiteSpace(tag) ? url : tag, out lines, scrWidth, conWidth, height);
                        height += lines;
                        continue;
                    }
                    {
                        MainView.Add(new Label(WrapString(line, conWidth, out lines))
                        {
                            X = (scrWidth - conWidth) / 2 + 1,
                            Y = height,
                            Width = conWidth,
                            ColorScheme = BodyScheme,
                        });

                        height += lines;
                    }
                }
            }
            else if (tab.Type == DocumentType.Gopher)
            {
                height++;
                foreach (string line in data.Split("\n"))
                {
                    int lines = 1;

                    if (line.Length == 0)
                    {
                        height++;
                        continue;
                    }

                    char type = line[0];
                    string[] d = line.Remove(0, 1).Split('\t');

                    if (type == 'i')
                    {
                        MainView.Add(new Label(d[0])
                        {
                            X = (scrWidth - conWidth) / 2 + 1,
                            Y = height,
                            Width = conWidth,
                            ColorScheme = BodyScheme,
                        });
                    }
                    else if (type == 'h')
                    {
                        string url = d[1].StartsWith("URL:") ? d[1].Remove(0, 4) : d[1];
                        Uri realUri;
                        if (Uri.TryCreate(new Uri("https://" + tab.Uri.Host), url, out realUri))
                        {
                            MakeLinkButton(realUri, d[0], out lines, scrWidth, conWidth, height);
                        } 
                        else
                        {
                            Console.Write(url);
                        }
                    }
                    else
                    {
                        if (d.Length < 4)
                        {
                            height++;
                            continue;
                        }
                        string url = "gopher://" + d[2] + ":" + d[3] + "/" + type + d[1];
                        Uri realUri;
                        if (Uri.TryCreate(url, UriKind.Absolute, out realUri))
                        {
                            MakeLinkButton(realUri, d[0], out lines, scrWidth, conWidth, height);
                        }
                    }

                    height += lines;
                }
            }
            else if (tab.Type == DocumentType.System)
            {
                height++;
                foreach (string line in data.Split("\n"))
                {
                    int lines = 1;

                    MainView.Add(new Label(WrapString(line, conWidth, out lines))
                    {
                        X = (scrWidth - conWidth) / 2 + 1,
                        Y = height,
                        Width = conWidth,
                        ColorScheme = BodyScheme,
                    });

                    height += lines;
                }
            }
            else if (tab.Type == DocumentType.Text)
            {
                height++;
                foreach (string line in data.Split("\n"))
                {
                    int lines = 1;

                    MainView.Add(new Label(WrapString(line, conWidth, out lines))
                    {
                        X = (scrWidth - conWidth) / 2 + 1,
                        Y = height,
                        Width = conWidth,
                        ColorScheme = BodyScheme,
                    });

                    height += lines;
                }
            }
            MainView.ContentSize = new Size(scrWidth, height + 1);
            Sidebar.ContentSize = new Size(sbWidth, sheight + 1);
        }

        static void MakeHeader(string text, out int height, int scrWidth, int conWidth, int Y)
        {
            MainView.Add(new Label(WrapString(text, conWidth, out height))
            {
                X = (scrWidth - conWidth) / 2 + 1,
                Y = Y,
                Width = conWidth,
            });
        }

        static void MakeLinkButton(Uri realUri, string text, out int height, int scrWidth, int conWidth, int Y)
        {
            bool supported = Array.IndexOf(new[] { "gopher", "gemini" }, realUri.Scheme) >= 0;
            int spos = Y;

            MainView.Add(new Label(supported ? "═►" : "╔►")
            {
                X = (scrWidth - conWidth) / 2 - 2,
                Y = Y,
                ColorScheme = BodyScheme,
            });
            PlainButton item = new PlainButton(WrapString(text, conWidth, out height))
            {
                X = (scrWidth - conWidth) / 2 + 1,
                Y = Y,
                Width = conWidth,
                TextAlignment = TextAlignment.Left,
                ColorScheme = LinkScheme,
            };
            item.Enter += (a) =>
            {
                if (KeyboardInteracted)
                {
                    int hi = MainView.Focused?.Focused?.Frame.Height ?? 0;
                    int shi = MainView.Frame.Height;
                    Console.WriteLine(spos + " " + hi);
                    if (spos < -MainView.ContentOffset.Y)
                        MainView.ContentOffset = new Point(MainView.ContentOffset.X, -spos);
                    if (spos + hi > -MainView.ContentOffset.Y + shi)
                        MainView.ContentOffset = new Point(MainView.ContentOffset.X, -spos - hi + shi);
                }
                KeyboardInteracted = false;
                StatusText = realUri.AbsoluteUri;
            };
            item.KeyDown += (e) =>
            {
                KeyboardInteracted = true;
            };
            item.Leave += (a) =>
            {
                StatusText = "";
            };
            item.Clicked += () =>
            {
                if (supported)
                {
                    StatusText = "";
                    Tabs[CurrentTab].Navigate(realUri.AbsoluteUri);
                    Tabs[CurrentTab].Output = "\nConnecting to " + realUri.Host + " . . .";
                }
                else
                {
                    Tab.OpenExternalUri(realUri.AbsoluteUri);
                }
            };
            MainView.Add(item);
        }

        static NStack.ustring WrapString(string str, int width, out int lines)
        {
            NStack.ustring result = "";
            List<NStack.ustring> line = TextFormatter.WordWrap(str, width);
            lines = Math.Max(line.Count, 1);
            foreach (NStack.ustring li in line) result += (result == "" ? "" : "\n") + li;
            return result;
        }

        static void ResizeConsole(int width, int height)
        {
            try { Console.SetBufferSize(width, height); } catch (Exception) { }
            try { Console.SetWindowSize(width, height); } catch (Exception) { }
        }
    }
}
