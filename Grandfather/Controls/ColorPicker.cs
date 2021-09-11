
using System;
using Grandfather.Data;
using NStack;
using Terminal.Gui;

namespace Grandfather.Controls
{
	public class ColorPicker : View
	{
		ColorPallete color;

		public ColorPicker() : this(color: new ColorPallete()) { }

		public ColorPicker(ColorPallete color) : base()
		{
			Init(color);
		}

		public ColorPicker(int x, int y, ColorPallete color) : this(x, y, color, false) { }

		public ColorPicker(int x, int y, ColorPallete color, bool is_default)
			: base(new Rect(x, y, 2, 1))
		{
			Init(color);
		}

		void Init(ColorPallete color)
		{
			Text = "Color■";
			Width = 6;
			Height = 1;
			CanFocus = true;
			this.color = color;
			Update();
		}

		/// <summary>
		///   The color displayed by this <see cref="ColorPicker"/>.
		/// </summary>
		public ColorPallete Color
		{
			get
			{
				return color;
			}

			set
			{
				color = value;
				Update();
			}
		}

		internal void Update()
		{
			ColorScheme = new ColorScheme()
			{
				Normal = new Terminal.Gui.Attribute(Color.Foreground, Color.Background),
				HotNormal = new Terminal.Gui.Attribute(Color.Foreground, Color.Background),
				Focus = new Terminal.Gui.Attribute(Color.Foreground, Color.Background),
				HotFocus = new Terminal.Gui.Attribute(Color.Foreground, Color.Background),
				Disabled = new Terminal.Gui.Attribute(Color.Foreground, Color.Background),
			};
			SetWidth(6, out int _);
			Width = 6;
			Frame = new Rect(Frame.Location, new Size(2, 1));
			SetNeedsDisplay();
		}

		bool CheckKey(KeyEvent key)
		{
			if (key.Key == (Key.AltMask | HotKey))
			{
				SetFocus();
				return true;
			}
			return false;
		}

		///<inheritdoc/>
		public override bool ProcessHotKey(KeyEvent kb)
		{
			if (kb.IsAlt)
				return CheckKey(kb);

			return false;
		}

		///<inheritdoc/>
		public override bool ProcessColdKey(KeyEvent kb)
		{
			return CheckKey(kb);
		}

		///<inheritdoc/>
		public override bool ProcessKey(KeyEvent kb)
		{
			var c = kb.KeyValue;
			if (c == '\n' || c == ' ' || kb.Key == HotKey)
			{
				return true;
			}
			return base.ProcessKey(kb);
		}

		///<inheritdoc/>
		public override bool MouseEvent(MouseEvent me)
		{
			if (me.Flags == MouseFlags.Button1Clicked || me.Flags == MouseFlags.Button1DoubleClicked ||
				me.Flags == MouseFlags.Button1TripleClicked)
			{
				if (CanFocus)
				{
					if (!HasFocus)
					{
						SetFocus();
						SetNeedsDisplay();
					}
				}

				return true;
			}
			return false;
		}

		///<inheritdoc/>
		public override bool OnEnter(View view)
		{
			Application.Driver.SetCursorVisibility(CursorVisibility.Invisible);

			return base.OnEnter(view);
		}
	}
}