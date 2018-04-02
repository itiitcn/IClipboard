using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialListView : ListView, IMaterialControl
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LogFont
		{
			public int lfHeight = 0;

			public int lfWidth = 0;

			public int lfEscapement = 0;

			public int lfOrientation = 0;

			public int lfWeight = 0;

			public byte lfItalic = 0;

			public byte lfUnderline = 0;

			public byte lfStrikeOut = 0;

			public byte lfCharSet = 0;

			public byte lfOutPrecision = 0;

			public byte lfClipPrecision = 0;

			public byte lfQuality = 0;

			public byte lfPitchAndFamily = 0;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName = string.Empty;
		}

		private const int ITEM_PADDING = 12;

		[Browsable(false)]
		public int Depth
		{
			get;
			set;
		}

		[Browsable(false)]
		public MaterialSkinManager SkinManager
		{
			get
			{
				return MaterialSkinManager.Instance;
			}
		}

		[Browsable(false)]
		public MouseState MouseState
		{
			get;
			set;
		}

		[Browsable(false)]
		public Point MouseLocation
		{
			get;
			set;
		}

		[Browsable(false)]
		private ListViewItem HoveredItem
		{
			get;
			set;
		}

		public MaterialListView()
		{
			base.GridLines = false;
			base.FullRowSelect = true;
			base.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			base.View = View.Details;
			base.OwnerDraw = true;
			base.ResizeRedraw = true;
			base.BorderStyle = BorderStyle.None;
			base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
			this.MouseLocation = new Point(-1, -1);
			this.MouseState = MouseState.OUT;
			base.MouseEnter += delegate
			{
				this.MouseState = MouseState.HOVER;
			};
			base.MouseLeave += delegate
			{
				this.MouseState = MouseState.OUT;
				this.MouseLocation = new Point(-1, -1);
				this.HoveredItem = null;
				base.Invalidate();
			};
			base.MouseDown += delegate
			{
				this.MouseState = MouseState.DOWN;
			};
			base.MouseUp += delegate
			{
				this.MouseState = MouseState.HOVER;
			};
			base.MouseMove += delegate(object sender, MouseEventArgs args)
			{
				this.MouseLocation = args.Location;
				Point mouseLocation = this.MouseLocation;
				int x = mouseLocation.X;
				mouseLocation = this.MouseLocation;
				ListViewItem itemAt = base.GetItemAt(x, mouseLocation.Y);
				if (this.HoveredItem != itemAt)
				{
					this.HoveredItem = itemAt;
					base.Invalidate();
				}
			};
		}

		protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			SolidBrush brush = new SolidBrush(this.SkinManager.GetApplicationBackgroundColor());
			Rectangle bounds = e.Bounds;
			int x = bounds.X;
			bounds = e.Bounds;
			int y = bounds.Y;
			int width = base.Width;
			bounds = e.Bounds;
			graphics.FillRectangle(brush, new Rectangle(x, y, width, bounds.Height));
			Graphics graphics2 = e.Graphics;
			string text = e.Header.Text;
			Font rOBOTO_MEDIUM_ = this.SkinManager.ROBOTO_MEDIUM_10;
			Brush secondaryTextBrush = this.SkinManager.GetSecondaryTextBrush();
			bounds = e.Bounds;
			int x2 = bounds.X + 12;
			bounds = e.Bounds;
			int y2 = bounds.Y + 12;
			bounds = e.Bounds;
			int width2 = bounds.Width - 24;
			bounds = e.Bounds;
			graphics2.DrawString(text, rOBOTO_MEDIUM_, secondaryTextBrush, new Rectangle(x2, y2, width2, bounds.Height - 24), this.getStringFormat());
		}

		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			Rectangle bounds = e.Item.Bounds;
			int width = bounds.Width;
			bounds = e.Item.Bounds;
			Bitmap bitmap = new Bitmap(width, bounds.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			Graphics graphics2 = graphics;
			SolidBrush brush = new SolidBrush(this.SkinManager.GetApplicationBackgroundColor());
			bounds = e.Bounds;
			Point location = new Point(bounds.X, 0);
			bounds = e.Bounds;
			graphics2.FillRectangle(brush, new Rectangle(location, bounds.Size));
			if (((Enum)(object)e.State).HasFlag((Enum)(object)ListViewItemStates.Selected))
			{
				Graphics graphics3 = graphics;
				Brush flatButtonPressedBackgroundBrush = this.SkinManager.GetFlatButtonPressedBackgroundBrush();
				bounds = e.Bounds;
				Point location2 = new Point(bounds.X, 0);
				bounds = e.Bounds;
				graphics3.FillRectangle(flatButtonPressedBackgroundBrush, new Rectangle(location2, bounds.Size));
			}
			else
			{
				bounds = e.Bounds;
				if (bounds.Contains(this.MouseLocation) && this.MouseState == MouseState.HOVER)
				{
					Graphics graphics4 = graphics;
					Brush flatButtonHoverBackgroundBrush = this.SkinManager.GetFlatButtonHoverBackgroundBrush();
					bounds = e.Bounds;
					Point location3 = new Point(bounds.X, 0);
					bounds = e.Bounds;
					graphics4.FillRectangle(flatButtonHoverBackgroundBrush, new Rectangle(location3, bounds.Size));
				}
			}
			Graphics graphics5 = graphics;
			Pen pen = new Pen(this.SkinManager.GetDividersColor());
			bounds = e.Bounds;
			int left = bounds.Left;
			bounds = e.Bounds;
			graphics5.DrawLine(pen, left, 0, bounds.Right, 0);
			foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
			{
				Graphics graphics6 = graphics;
				string text = subItem.Text;
				Font rOBOTO_MEDIUM_ = this.SkinManager.ROBOTO_MEDIUM_10;
				Brush primaryTextBrush = this.SkinManager.GetPrimaryTextBrush();
				bounds = subItem.Bounds;
				int x = bounds.X + 12;
				bounds = subItem.Bounds;
				int width2 = bounds.Width - 24;
				bounds = subItem.Bounds;
				graphics6.DrawString(text, rOBOTO_MEDIUM_, primaryTextBrush, new Rectangle(x, 12, width2, bounds.Height - 24), this.getStringFormat());
			}
			Graphics graphics7 = e.Graphics;
			Image image = (Image)bitmap.Clone();
			bounds = e.Item.Bounds;
			graphics7.DrawImage(image, new Point(0, bounds.Location.Y));
			graphics.Dispose();
			bitmap.Dispose();
		}

		private StringFormat getStringFormat()
		{
			return new StringFormat
			{
				FormatFlags = StringFormatFlags.LineLimit,
				Trimming = StringTrimming.EllipsisCharacter,
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Center
			};
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			Font font = new Font(this.SkinManager.ROBOTO_MEDIUM_12.FontFamily, 24f);
			LogFont logFont = new LogFont();
			font.ToLogFont(logFont);
			try
			{
				this.Font = Font.FromLogFont(logFont);
			}
			catch (ArgumentException)
			{
				this.Font = new Font(FontFamily.GenericSansSerif, 24f);
			}
		}
	}
}
