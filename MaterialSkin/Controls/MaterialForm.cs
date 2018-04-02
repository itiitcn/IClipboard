using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialForm : Form, IMaterialControl
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		public class MONITORINFOEX
		{
			public int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));

			public RECT rcMonitor = default(RECT);

			public RECT rcWork = default(RECT);

			public int dwFlags = 0;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public char[] szDevice = new char[32];
		}

		public struct RECT
		{
			public int left;

			public int top;

			public int right;

			public int bottom;

			public int Width()
			{
				return this.right - this.left;
			}

			public int Height()
			{
				return this.bottom - this.top;
			}
		}

		private enum ResizeDirection
		{
			BottomLeft,
			Left,
			Right,
			BottomRight,
			Bottom,
			None
		}

		private enum ButtonState
		{
			XOver,
			MaxOver,
			MinOver,
			XDown,
			MaxDown,
			MinDown,
			None
		}

		public const int WM_NCLBUTTONDOWN = 161;

		public const int HT_CAPTION = 2;

		public const int WM_MOUSEMOVE = 512;

		public const int WM_LBUTTONDOWN = 513;

		public const int WM_LBUTTONUP = 514;

		public const int WM_LBUTTONDBLCLK = 515;

		public const int WM_RBUTTONDOWN = 516;

		private const int HTBOTTOMLEFT = 16;

		private const int HTBOTTOMRIGHT = 17;

		private const int HTLEFT = 10;

		private const int HTRIGHT = 11;

		private const int HTBOTTOM = 15;

        private const int TITLEHEIGHT = 32;


        private const int HTTOP = 12;

		private const int HTTOPLEFT = 13;

		private const int HTTOPRIGHT = 14;

		private const int BORDER_WIDTH = 7;

		private ResizeDirection _resizeDir;

		private ButtonState _buttonState = ButtonState.None;

		private const int WMSZ_TOP = 3;

		private const int WMSZ_TOPLEFT = 4;

		private const int WMSZ_TOPRIGHT = 5;

		private const int WMSZ_LEFT = 1;

		private const int WMSZ_RIGHT = 2;

		private const int WMSZ_BOTTOM = 6;

		private const int WMSZ_BOTTOMLEFT = 7;

		private const int WMSZ_BOTTOMRIGHT = 8;

		private readonly Dictionary<int, int> _resizingLocationsToCmd = new Dictionary<int, int>
		{
			{
				12,
				3
			},
			{
				13,
				4
			},
			{
				14,
				5
			},
			{
				10,
				1
			},
			{
				11,
				2
			},
			{
				15,
				6
			},
			{
				16,
				7
			},
			{
				17,
				8
			}
		};

		private const int STATUS_BAR_BUTTON_WIDTH = 24;

		private const int STATUS_BAR_HEIGHT = 24;

		private const int ACTION_BAR_HEIGHT = 40;

		private const uint TPM_LEFTALIGN = 0u;

		private const uint TPM_RETURNCMD = 256u;

		private const int WM_SYSCOMMAND = 274;

		private const int WS_MINIMIZEBOX = 131072;

		private const int WS_SYSMENU = 524288;

		private const int MONITOR_DEFAULTTONEAREST = 2;

		private readonly Cursor[] _resizeCursors = new Cursor[5]
		{
			Cursors.SizeNESW,
			Cursors.SizeWE,
			Cursors.SizeNWSE,
			Cursors.SizeWE,
			Cursors.SizeNS
		};

		private Rectangle _minButtonBounds;

		private Rectangle _maxButtonBounds;

		private Rectangle _xButtonBounds;

		private Rectangle _statusBarBounds;

		private bool _maximized;

		private Size _previousSize;

		private Point _previousLocation;

		private bool _headerMouseDown;

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

		public new FormBorderStyle FormBorderStyle
		{
			get
			{
				return base.FormBorderStyle;
			}
			set
			{
				base.FormBorderStyle = value;
			}
		}

		public bool Sizable
		{
			get;
			set;
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style = (createParams.Style | 0x20000 | 0x80000);
				return createParams;
			}
		}

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		[DllImport("user32.dll")]
		public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

		[DllImport("user32.dll")]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll")]
		public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetMonitorInfo(HandleRef hmonitor, [In] [Out] MONITORINFOEX info);

		public MaterialForm()
		{
			this.FormBorderStyle = FormBorderStyle.None;
			this.Sizable = true;
			this.DoubleBuffered = true;
			base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			Application.AddMessageFilter(new MouseMessageFilter());
			MouseMessageFilter.MouseMove += this.OnGlobalMouseMove;
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (!base.DesignMode && !base.IsDisposed)
			{
				Point position;
				if (m.Msg == 515)
				{
					this.MaximizeWindow(!this._maximized);
				}
				else if (m.Msg == 512 && this._maximized && (this._statusBarBounds.Contains(base.PointToClient(Cursor.Position))) && !this._minButtonBounds.Contains(base.PointToClient(Cursor.Position)) && !this._maxButtonBounds.Contains(base.PointToClient(Cursor.Position)) && !this._xButtonBounds.Contains(base.PointToClient(Cursor.Position)))
				{
					if (this._headerMouseDown)
					{
						this._maximized = false;
						this._headerMouseDown = false;
						Point point = base.PointToClient(Cursor.Position);
						if (point.X < base.Width / 2)
						{
							object location;
							if (point.X >= this._previousSize.Width / 2)
							{
								position = Cursor.Position;
								int x = position.X - this._previousSize.Width / 2;
								position = Cursor.Position;
								location = new Point(x, position.Y - point.Y);
							}
							else
							{
								position = Cursor.Position;
								int x2 = position.X - point.X;
								position = Cursor.Position;
								location = new Point(x2, position.Y - point.Y);
							}
							base.Location = (Point)location;
						}
						else
						{
							object location2;
							if (base.Width - point.X >= this._previousSize.Width / 2)
							{
								position = Cursor.Position;
								int x3 = position.X - this._previousSize.Width / 2;
								position = Cursor.Position;
								location2 = new Point(x3, position.Y - point.Y);
							}
							else
							{
								position = Cursor.Position;
								int x4 = position.X - this._previousSize.Width + base.Width - point.X;
								position = Cursor.Position;
								location2 = new Point(x4, position.Y - point.Y);
							}
							base.Location = (Point)location2;
						}
						base.Size = this._previousSize;
						MaterialForm.ReleaseCapture();
						MaterialForm.SendMessage(base.Handle, 161, 2, 0);
					}
				}
				else if (m.Msg == 513 && (this._statusBarBounds.Contains(base.PointToClient(Cursor.Position)) ) && !this._minButtonBounds.Contains(base.PointToClient(Cursor.Position)) && !this._maxButtonBounds.Contains(base.PointToClient(Cursor.Position)) && !this._xButtonBounds.Contains(base.PointToClient(Cursor.Position)))
				{
					if (!this._maximized)
					{
						MaterialForm.ReleaseCapture();
						MaterialForm.SendMessage(base.Handle, 161, 2, 0);
					}
					else
					{
						this._headerMouseDown = true;
					}
				}
				else if (m.Msg == 516)
				{
					Point pt = base.PointToClient(Cursor.Position);
					if (this._statusBarBounds.Contains(pt) && !this._minButtonBounds.Contains(pt) && !this._maxButtonBounds.Contains(pt) && !this._xButtonBounds.Contains(pt))
					{
						IntPtr systemMenu = MaterialForm.GetSystemMenu(base.Handle, false);
						position = Cursor.Position;
						int x5 = position.X;
						position = Cursor.Position;
						int wParam = MaterialForm.TrackPopupMenuEx(systemMenu, 256u, x5, position.Y, base.Handle, IntPtr.Zero);
						MaterialForm.SendMessage(base.Handle, 274, wParam, 0);
					}
				}
				else if (m.Msg == 161)
				{
					if (this.Sizable)
					{
						byte b = 0;
						if (this._resizingLocationsToCmd.ContainsKey((int)m.WParam))
						{
							b = (byte)this._resizingLocationsToCmd[(int)m.WParam];
						}
						if (b != 0)
						{
							MaterialForm.SendMessage(base.Handle, 274, 0xF000 | b, (int)m.LParam);
						}
					}
				}
				else if (m.Msg == 514)
				{
					this._headerMouseDown = false;
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!base.DesignMode)
			{
				this.UpdateButtons(e, false);
				if (e.Button == MouseButtons.Left && !this._maximized)
				{
					this.ResizeForm(this._resizeDir);
				}
				base.OnMouseDown(e);
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (!base.DesignMode)
			{
				this._buttonState = ButtonState.None;
				base.Invalidate();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			bool flag;
			Point location;
			int num;
			if (!base.DesignMode)
			{
				if (this.Sizable)
				{
					flag = (base.GetChildAtPoint(e.Location) != null);
					location = e.Location;
					if (location.X < 7)
					{
						location = e.Location;
						if (location.Y > base.Height - 7 && !flag)
						{
							num = ((!this._maximized) ? 1 : 0);
							goto IL_0070;
						}
					}
					num = 0;
					goto IL_0070;
				}
				goto IL_01e9;
			}
			return;
			IL_01e9:
			this.UpdateButtons(e, false);
			return;
			IL_0110:
			int num2;
			if (num2 != 0)
			{
				this._resizeDir = ResizeDirection.BottomRight;
				this.Cursor = Cursors.SizeNWSE;
			}
			else
			{
				location = e.Location;
				if (location.X > base.Width - 7 && !flag && !this._maximized)
				{
					this._resizeDir = ResizeDirection.Right;
					this.Cursor = Cursors.SizeWE;
				}
				else
				{
					location = e.Location;
					if (location.Y > base.Height - 7 && !flag && !this._maximized)
					{
						this._resizeDir = ResizeDirection.Bottom;
						this.Cursor = Cursors.SizeNS;
					}
					else
					{
						this._resizeDir = ResizeDirection.None;
						if (this._resizeCursors.Contains(this.Cursor))
						{
							this.Cursor = Cursors.Default;
						}
					}
				}
			}
			goto IL_01e9;
			IL_0070:
			if (num != 0)
			{
				this._resizeDir = ResizeDirection.BottomLeft;
				this.Cursor = Cursors.SizeNESW;
				goto IL_01e9;
			}
			location = e.Location;
			if (location.X < 7 && !flag && !this._maximized)
			{
				this._resizeDir = ResizeDirection.Left;
				this.Cursor = Cursors.SizeWE;
				goto IL_01e9;
			}
			location = e.Location;
			if (location.X > base.Width - 7)
			{
				location = e.Location;
				if (location.Y > base.Height - 7 && !flag)
				{
					num2 = ((!this._maximized) ? 1 : 0);
					goto IL_0110;
				}
			}
			num2 = 0;
			goto IL_0110;
		}

		protected void OnGlobalMouseMove(object sender, MouseEventArgs e)
		{
			if (!base.IsDisposed)
			{
				Point point = base.PointToClient(e.Location);
				MouseEventArgs e2 = new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0);
				this.OnMouseMove(e2);
			}
		}

		private void UpdateButtons(MouseEventArgs e, bool up = false)
		{
			if (!base.DesignMode)
			{
				ButtonState buttonState = this._buttonState;
				bool flag = base.MinimizeBox && base.ControlBox;
				bool flag2 = base.MaximizeBox && base.ControlBox;
				if (e.Button == MouseButtons.Left && !up)
				{
					if (flag && !flag2 && this._maxButtonBounds.Contains(e.Location))
					{
						this._buttonState = ButtonState.MinDown;
					}
					else if ((flag & flag2) && this._minButtonBounds.Contains(e.Location))
					{
						this._buttonState = ButtonState.MinDown;
					}
					else if (flag2 && this._maxButtonBounds.Contains(e.Location))
					{
						this._buttonState = ButtonState.MaxDown;
					}
					else if (base.ControlBox && this._xButtonBounds.Contains(e.Location))
					{
						this._buttonState = ButtonState.XDown;
					}
					else
					{
						this._buttonState = ButtonState.None;
					}
				}
				else if (flag && !flag2 && this._maxButtonBounds.Contains(e.Location))
				{
					this._buttonState = ButtonState.MinOver;
					if (buttonState == ButtonState.MinDown & up)
					{
						base.WindowState = FormWindowState.Minimized;
					}
				}
				else if ((flag & flag2) && this._minButtonBounds.Contains(e.Location))
				{
					this._buttonState = ButtonState.MinOver;
					if (buttonState == ButtonState.MinDown & up)
					{
						base.WindowState = FormWindowState.Minimized;
					}
				}
				else if (base.MaximizeBox && base.ControlBox && this._maxButtonBounds.Contains(e.Location))
				{
					this._buttonState = ButtonState.MaxOver;
					if (buttonState == ButtonState.MaxDown & up)
					{
						this.MaximizeWindow(!this._maximized);
					}
				}
				else if (base.ControlBox && this._xButtonBounds.Contains(e.Location))
				{
					this._buttonState = ButtonState.XOver;
					if (buttonState == ButtonState.XDown & up)
					{
						base.Close();
					}
				}
				else
				{
					this._buttonState = ButtonState.None;
				}
				if (buttonState != this._buttonState)
				{
					base.Invalidate();
				}
			}
		}

		private void MaximizeWindow(bool maximize)
		{
			if (base.MaximizeBox && base.ControlBox)
			{
				this._maximized = maximize;
				if (maximize)
				{
					IntPtr handle = MaterialForm.MonitorFromWindow(base.Handle, 2u);
					MONITORINFOEX mONITORINFOEX = new MONITORINFOEX();
					MaterialForm.GetMonitorInfo(new HandleRef(null, handle), mONITORINFOEX);
					this._previousSize = base.Size;
					this._previousLocation = base.Location;
					base.Size = new Size(mONITORINFOEX.rcWork.Width(), mONITORINFOEX.rcWork.Height());
					base.Location = new Point(mONITORINFOEX.rcWork.left, mONITORINFOEX.rcWork.top);
				}
				else
				{
					base.Size = this._previousSize;
					base.Location = this._previousLocation;
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (!base.DesignMode)
			{
				this.UpdateButtons(e, true);
				base.OnMouseUp(e);
				MaterialForm.ReleaseCapture();
			}
		}

		private void ResizeForm(ResizeDirection direction)
		{
			if (!base.DesignMode)
			{
				int num = -1;
				switch (direction)
				{
				case ResizeDirection.BottomLeft:
					num = 16;
					break;
				case ResizeDirection.Left:
					num = 10;
					break;
				case ResizeDirection.Right:
					num = 11;
					break;
				case ResizeDirection.BottomRight:
					num = 17;
					break;
				case ResizeDirection.Bottom:
					num = 15;
					break;
				}
				MaterialForm.ReleaseCapture();
				if (num != -1)
				{
					MaterialForm.SendMessage(base.Handle, 161, num, 0);
				}
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this._minButtonBounds = new Rectangle(base.Width - this.SkinManager.FORM_PADDING / 2 - TITLEHEIGHT*3, 0, TITLEHEIGHT, TITLEHEIGHT);
			this._maxButtonBounds = new Rectangle(base.Width - this.SkinManager.FORM_PADDING / 2 - TITLEHEIGHT*2, 0, TITLEHEIGHT, TITLEHEIGHT);
			this._xButtonBounds = new Rectangle(base.Width - this.SkinManager.FORM_PADDING / 2 - TITLEHEIGHT, 0, TITLEHEIGHT, TITLEHEIGHT);
			this._statusBarBounds = new Rectangle(0, 0, base.Width, TITLEHEIGHT);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			graphics.Clear(this.SkinManager.GetApplicationBackgroundColor());
			graphics.FillRectangle(this.SkinManager.ColorScheme.DarkPrimaryBrush, this._statusBarBounds);
			using (Pen pen = new Pen(this.SkinManager.GetDividersColor(), 1f))
			{
				graphics.DrawLine(pen, new Point(0, base.Height - 1), new Point(base.Width - 1, base.Height - 1));
			}
			bool flag = base.MinimizeBox && base.ControlBox;
			bool flag2 = base.MaximizeBox && base.ControlBox;
			Brush flatButtonHoverBackgroundBrush = this.SkinManager.GetFlatButtonHoverBackgroundBrush();
			Brush flatButtonPressedBackgroundBrush = this.SkinManager.GetFlatButtonPressedBackgroundBrush();
			if (this._buttonState == ButtonState.MinOver & flag)
			{
				graphics.FillRectangle(flatButtonHoverBackgroundBrush, flag2 ? this._minButtonBounds : this._maxButtonBounds);
			}
			if (this._buttonState == ButtonState.MinDown & flag)
			{
				graphics.FillRectangle(flatButtonPressedBackgroundBrush, flag2 ? this._minButtonBounds : this._maxButtonBounds);
			}
			if (this._buttonState == ButtonState.MaxOver & flag2)
			{
				graphics.FillRectangle(flatButtonHoverBackgroundBrush, this._maxButtonBounds);
			}
			if (this._buttonState == ButtonState.MaxDown & flag2)
			{
				graphics.FillRectangle(flatButtonPressedBackgroundBrush, this._maxButtonBounds);
			}
			if (this._buttonState == ButtonState.XOver && base.ControlBox)
			{
				graphics.FillRectangle(flatButtonHoverBackgroundBrush, this._xButtonBounds);
			}
			if (this._buttonState == ButtonState.XDown && base.ControlBox)
			{
				graphics.FillRectangle(flatButtonPressedBackgroundBrush, this._xButtonBounds);
			}
			using (Pen pen2 = new Pen(this.SkinManager.ACTION_BAR_TEXT_SECONDARY, 2f))
			{
				if (flag)
				{
					int num = flag2 ? this._minButtonBounds.X : this._maxButtonBounds.X;
					int num2 = flag2 ? this._minButtonBounds.Y : this._maxButtonBounds.Y;
					graphics.DrawLine(pen2, num + (int)((double)this._minButtonBounds.Width * 0.33), num2 + (int)((double)this._minButtonBounds.Height * 0.66), num + (int)((double)this._minButtonBounds.Width * 0.66), num2 + (int)((double)this._minButtonBounds.Height * 0.66));
				}
				if (flag2)
				{
					graphics.DrawRectangle(pen2, this._maxButtonBounds.X + (int)((double)this._maxButtonBounds.Width * 0.33), this._maxButtonBounds.Y + (int)((double)this._maxButtonBounds.Height * 0.36), (int)((double)this._maxButtonBounds.Width * 0.39), (int)((double)this._maxButtonBounds.Height * 0.31));
				}
				if (base.ControlBox)
				{
					graphics.DrawLine(pen2, this._xButtonBounds.X + (int)((double)this._xButtonBounds.Width * 0.33), this._xButtonBounds.Y + (int)((double)this._xButtonBounds.Height * 0.33), this._xButtonBounds.X + (int)((double)this._xButtonBounds.Width * 0.66), this._xButtonBounds.Y + (int)((double)this._xButtonBounds.Height * 0.66));
					graphics.DrawLine(pen2, this._xButtonBounds.X + (int)((double)this._xButtonBounds.Width * 0.66), this._xButtonBounds.Y + (int)((double)this._xButtonBounds.Height * 0.33), this._xButtonBounds.X + (int)((double)this._xButtonBounds.Width * 0.33), this._xButtonBounds.Y + (int)((double)this._xButtonBounds.Height * 0.66));
				}
			}
			graphics.DrawString(this.Text, this.SkinManager.ROBOTO_MEDIUM_12, this.SkinManager.ColorScheme.TextBrush, new Rectangle(this.SkinManager.FORM_PADDING+10, 2, base.Width, 32), new StringFormat
			{
				LineAlignment = StringAlignment.Center
			});
            graphics.DrawIcon(this.Icon, new Rectangle(8, 8, 16, 16));
        }

        protected override void OnLoad(EventArgs e)
        {

            //
            string file = Application.StartupPath + "\\logo.ico";
            if (File.Exists(file))
            {
                StreamReader sr = new StreamReader(file);
                if (sr != null)
                    this.Icon = new Icon(sr.BaseStream);
            }

            base.OnLoad(e);
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.Invalidate();
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialForm));
            this.SuspendLayout();
            // 
            // MaterialForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MaterialForm";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
    }
}
