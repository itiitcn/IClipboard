using Controls;
using MaterialSkin.Animations;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialSingleLineTextField : Control, IMaterialControl
	{
		private class BaseTextBox : TextBox
		{
			private const int EM_SETCUEBANNER = 5377;

			private const char EmptyChar = '\0';

			private const char VisualStylePasswordChar = '●';

			private const char NonVisualStylePasswordChar = '*';

			private string hint = string.Empty;

			private char _passwordChar = '\0';

			private char _useSystemPasswordChar = '\0';

			public string Hint
			{
				get
				{
					return this.hint;
				}
				set
				{
					this.hint = value;
					BaseTextBox.SendMessage(base.Handle, 5377, (int)IntPtr.Zero, this.Hint);
				}
			}

			public new char PasswordChar
			{
				get
				{
					return this._passwordChar;
				}
				set
				{
					this._passwordChar = value;
					this.SetBasePasswordChar();
				}
			}

			public new bool UseSystemPasswordChar
			{
				get
				{
					return this._useSystemPasswordChar != '\0';
				}
				set
				{
					if (value)
					{
						this._useSystemPasswordChar = (char)(Application.RenderWithVisualStyles ? 9679 : 42);
					}
					else
					{
						this._useSystemPasswordChar = '\0';
					}
					this.SetBasePasswordChar();
				}
			}

			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

			public new void SelectAll()
			{
				base.BeginInvoke((MethodInvoker)delegate
				{
					base.Focus();
					base.SelectAll();
				});
			}

			public new void Focus()
			{
				base.BeginInvoke((MethodInvoker)delegate
				{
					base.Focus();
				});
			}

			private void SetBasePasswordChar()
			{
				base.PasswordChar = (this.UseSystemPasswordChar ? this._useSystemPasswordChar : this._passwordChar);
			}

			public BaseTextBox()
			{
				this.ContextMenuStrip = new TextBoxContextMenuStrip();
                //ContextMenuStrip.Opening += this.ContextMenuStripOnOpening;
                //ContextMenuStrip.ItemClicked +=  this.ContextMenuStripOnItemClickStart;

            }
            

            private void ContextMenuStripOnItemClickStart(object sender, ToolStripItemClickedEventArgs toolStripItemClickedEventArgs)
			{
				string text = toolStripItemClickedEventArgs.ClickedItem.Text;
				switch (text)
				{
				default:
					if (text == "全选")
					{
						this.SelectAll();
					}
					break;
				case "撤销":
					base.Undo();
					break;
				case "剪切":
					base.Cut();
					break;
				case "复制":
					base.Copy();
					break;
				case "粘贴":
					base.Paste();
					break;
				case "删除":
					this.SelectedText = string.Empty;
					break;
				}
			}

			private void ContextMenuStripOnOpening(object sender, CancelEventArgs cancelEventArgs)
			{
				TextBoxContextMenuStrip textBoxContextMenuStrip = sender as TextBoxContextMenuStrip;
				if (textBoxContextMenuStrip != null)
				{
					textBoxContextMenuStrip.Undo.Enabled = base.CanUndo;
					textBoxContextMenuStrip.Cut.Enabled = !string.IsNullOrEmpty(this.SelectedText);
					textBoxContextMenuStrip.Copy.Enabled = !string.IsNullOrEmpty(this.SelectedText);
					textBoxContextMenuStrip.Paste.Enabled = Clipboard.ContainsText();
					textBoxContextMenuStrip.Delete.Enabled = !string.IsNullOrEmpty(this.SelectedText);
					textBoxContextMenuStrip.SelectAll.Enabled = !string.IsNullOrEmpty(this.Text);
				}
			}
		}

		

		private readonly BaseTextBox _baseTextBox;

		private readonly AnimationManager _animationManager;

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

		public override string Text
		{
			get
			{
				return this._baseTextBox.Text;
			}
			set
			{
				this._baseTextBox.Text = value;
			}
		}

		public new object Tag
		{
			get
			{
				return this._baseTextBox.Tag;
			}
			set
			{
				this._baseTextBox.Tag = value;
			}
		}

		public int MaxLength
		{
			get
			{
				return this._baseTextBox.MaxLength;
			}
			set
			{
				this._baseTextBox.MaxLength = value;
			}
		}

		public string SelectedText
		{
			get
			{
				return this._baseTextBox.SelectedText;
			}
			set
			{
				this._baseTextBox.SelectedText = value;
			}
		}

		public string Hint
		{
			get
			{
				return this._baseTextBox.Hint;
			}
			set
			{
				this._baseTextBox.Hint = value;
			}
		}

		public int SelectionStart
		{
			get
			{
				return this._baseTextBox.SelectionStart;
			}
			set
			{
				this._baseTextBox.SelectionStart = value;
			}
		}

		public int SelectionLength
		{
			get
			{
				return this._baseTextBox.SelectionLength;
			}
			set
			{
				this._baseTextBox.SelectionLength = value;
			}
		}

		public int TextLength
		{
			get
			{
				return this._baseTextBox.TextLength;
			}
		}

		public bool UseSystemPasswordChar
		{
			get
			{
				return this._baseTextBox.UseSystemPasswordChar;
			}
			set
			{
				this._baseTextBox.UseSystemPasswordChar = value;
			}
		}

		public char PasswordChar
		{
			get
			{
				return this._baseTextBox.PasswordChar;
			}
			set
			{
				this._baseTextBox.PasswordChar = value;
			}
		}

		public event EventHandler AcceptsTabChanged
		{
			add
			{
				this._baseTextBox.AcceptsTabChanged += value;
			}
			remove
			{
				this._baseTextBox.AcceptsTabChanged -= value;
			}
		}

		public new event EventHandler AutoSizeChanged
		{
			add
			{
				this._baseTextBox.AutoSizeChanged += value;
			}
			remove
			{
				this._baseTextBox.AutoSizeChanged -= value;
			}
		}

		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				this._baseTextBox.BackgroundImageChanged += value;
			}
			remove
			{
				this._baseTextBox.BackgroundImageChanged -= value;
			}
		}

		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				this._baseTextBox.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				this._baseTextBox.BackgroundImageLayoutChanged -= value;
			}
		}

		public new event EventHandler BindingContextChanged
		{
			add
			{
				this._baseTextBox.BindingContextChanged += value;
			}
			remove
			{
				this._baseTextBox.BindingContextChanged -= value;
			}
		}

		public event EventHandler BorderStyleChanged
		{
			add
			{
				this._baseTextBox.BorderStyleChanged += value;
			}
			remove
			{
				this._baseTextBox.BorderStyleChanged -= value;
			}
		}

		public new event EventHandler CausesValidationChanged
		{
			add
			{
				this._baseTextBox.CausesValidationChanged += value;
			}
			remove
			{
				this._baseTextBox.CausesValidationChanged -= value;
			}
		}

		public new event UICuesEventHandler ChangeUICues
		{
			add
			{
				this._baseTextBox.ChangeUICues += value;
			}
			remove
			{
				this._baseTextBox.ChangeUICues -= value;
			}
		}

		public new event EventHandler Click
		{
			add
			{
				this._baseTextBox.Click += value;
			}
			remove
			{
				this._baseTextBox.Click -= value;
			}
		}

		public new event EventHandler ClientSizeChanged
		{
			add
			{
				this._baseTextBox.ClientSizeChanged += value;
			}
			remove
			{
				this._baseTextBox.ClientSizeChanged -= value;
			}
		}

		public new event EventHandler ContextMenuChanged
		{
			add
			{
				this._baseTextBox.ContextMenuChanged += value;
			}
			remove
			{
				this._baseTextBox.ContextMenuChanged -= value;
			}
		}

		public new event EventHandler ContextMenuStripChanged
		{
			add
			{
				this._baseTextBox.ContextMenuStripChanged += value;
			}
			remove
			{
				this._baseTextBox.ContextMenuStripChanged -= value;
			}
		}

		public new event ControlEventHandler ControlAdded
		{
			add
			{
				this._baseTextBox.ControlAdded += value;
			}
			remove
			{
				this._baseTextBox.ControlAdded -= value;
			}
		}

		public new event ControlEventHandler ControlRemoved
		{
			add
			{
				this._baseTextBox.ControlRemoved += value;
			}
			remove
			{
				this._baseTextBox.ControlRemoved -= value;
			}
		}

		public new event EventHandler CursorChanged
		{
			add
			{
				this._baseTextBox.CursorChanged += value;
			}
			remove
			{
				this._baseTextBox.CursorChanged -= value;
			}
		}

		public new event EventHandler Disposed
		{
			add
			{
				this._baseTextBox.Disposed += value;
			}
			remove
			{
				this._baseTextBox.Disposed -= value;
			}
		}

		public new event EventHandler DockChanged
		{
			add
			{
				this._baseTextBox.DockChanged += value;
			}
			remove
			{
				this._baseTextBox.DockChanged -= value;
			}
		}

		public new event EventHandler DoubleClick
		{
			add
			{
				this._baseTextBox.DoubleClick += value;
			}
			remove
			{
				this._baseTextBox.DoubleClick -= value;
			}
		}

		public new event DragEventHandler DragDrop
		{
			add
			{
				this._baseTextBox.DragDrop += value;
			}
			remove
			{
				this._baseTextBox.DragDrop -= value;
			}
		}

		public new event DragEventHandler DragEnter
		{
			add
			{
				this._baseTextBox.DragEnter += value;
			}
			remove
			{
				this._baseTextBox.DragEnter -= value;
			}
		}

		public new event EventHandler DragLeave
		{
			add
			{
				this._baseTextBox.DragLeave += value;
			}
			remove
			{
				this._baseTextBox.DragLeave -= value;
			}
		}

		public new event DragEventHandler DragOver
		{
			add
			{
				this._baseTextBox.DragOver += value;
			}
			remove
			{
				this._baseTextBox.DragOver -= value;
			}
		}

		public new event EventHandler EnabledChanged
		{
			add
			{
				this._baseTextBox.EnabledChanged += value;
			}
			remove
			{
				this._baseTextBox.EnabledChanged -= value;
			}
		}

		public new event EventHandler Enter
		{
			add
			{
				this._baseTextBox.Enter += value;
			}
			remove
			{
				this._baseTextBox.Enter -= value;
			}
		}

		public new event EventHandler FontChanged
		{
			add
			{
				this._baseTextBox.FontChanged += value;
			}
			remove
			{
				this._baseTextBox.FontChanged -= value;
			}
		}

		public new event EventHandler ForeColorChanged
		{
			add
			{
				this._baseTextBox.ForeColorChanged += value;
			}
			remove
			{
				this._baseTextBox.ForeColorChanged -= value;
			}
		}

		public new event GiveFeedbackEventHandler GiveFeedback
		{
			add
			{
				this._baseTextBox.GiveFeedback += value;
			}
			remove
			{
				this._baseTextBox.GiveFeedback -= value;
			}
		}

		public new event EventHandler GotFocus
		{
			add
			{
				this._baseTextBox.GotFocus += value;
			}
			remove
			{
				this._baseTextBox.GotFocus -= value;
			}
		}

		public new event EventHandler HandleCreated
		{
			add
			{
				this._baseTextBox.HandleCreated += value;
			}
			remove
			{
				this._baseTextBox.HandleCreated -= value;
			}
		}

		public new event EventHandler HandleDestroyed
		{
			add
			{
				this._baseTextBox.HandleDestroyed += value;
			}
			remove
			{
				this._baseTextBox.HandleDestroyed -= value;
			}
		}

		public new event HelpEventHandler HelpRequested
		{
			add
			{
				this._baseTextBox.HelpRequested += value;
			}
			remove
			{
				this._baseTextBox.HelpRequested -= value;
			}
		}

		public event EventHandler HideSelectionChanged
		{
			add
			{
				this._baseTextBox.HideSelectionChanged += value;
			}
			remove
			{
				this._baseTextBox.HideSelectionChanged -= value;
			}
		}

		public new event EventHandler ImeModeChanged
		{
			add
			{
				this._baseTextBox.ImeModeChanged += value;
			}
			remove
			{
				this._baseTextBox.ImeModeChanged -= value;
			}
		}

		public new event InvalidateEventHandler Invalidated
		{
			add
			{
				this._baseTextBox.Invalidated += value;
			}
			remove
			{
				this._baseTextBox.Invalidated -= value;
			}
		}

		public new event KeyEventHandler KeyDown
		{
			add
			{
				this._baseTextBox.KeyDown += value;
			}
			remove
			{
				this._baseTextBox.KeyDown -= value;
			}
		}

		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				this._baseTextBox.KeyPress += value;
			}
			remove
			{
				this._baseTextBox.KeyPress -= value;
			}
		}

		public new event KeyEventHandler KeyUp
		{
			add
			{
				this._baseTextBox.KeyUp += value;
			}
			remove
			{
				this._baseTextBox.KeyUp -= value;
			}
		}

		public new event LayoutEventHandler Layout
		{
			add
			{
				this._baseTextBox.Layout += value;
			}
			remove
			{
				this._baseTextBox.Layout -= value;
			}
		}

		public new event EventHandler Leave
		{
			add
			{
				this._baseTextBox.Leave += value;
			}
			remove
			{
				this._baseTextBox.Leave -= value;
			}
		}

		public new event EventHandler LocationChanged
		{
			add
			{
				this._baseTextBox.LocationChanged += value;
			}
			remove
			{
				this._baseTextBox.LocationChanged -= value;
			}
		}

		public new event EventHandler LostFocus
		{
			add
			{
				this._baseTextBox.LostFocus += value;
			}
			remove
			{
				this._baseTextBox.LostFocus -= value;
			}
		}

		public new event EventHandler MarginChanged
		{
			add
			{
				this._baseTextBox.MarginChanged += value;
			}
			remove
			{
				this._baseTextBox.MarginChanged -= value;
			}
		}

		public event EventHandler ModifiedChanged
		{
			add
			{
				this._baseTextBox.ModifiedChanged += value;
			}
			remove
			{
				this._baseTextBox.ModifiedChanged -= value;
			}
		}

		public new event EventHandler MouseCaptureChanged
		{
			add
			{
				this._baseTextBox.MouseCaptureChanged += value;
			}
			remove
			{
				this._baseTextBox.MouseCaptureChanged -= value;
			}
		}

		public new event MouseEventHandler MouseClick
		{
			add
			{
				this._baseTextBox.MouseClick += value;
			}
			remove
			{
				this._baseTextBox.MouseClick -= value;
			}
		}

		public new event MouseEventHandler MouseDoubleClick
		{
			add
			{
				this._baseTextBox.MouseDoubleClick += value;
			}
			remove
			{
				this._baseTextBox.MouseDoubleClick -= value;
			}
		}

		public new event MouseEventHandler MouseDown
		{
			add
			{
				this._baseTextBox.MouseDown += value;
			}
			remove
			{
				this._baseTextBox.MouseDown -= value;
			}
		}

		public new event EventHandler MouseEnter
		{
			add
			{
				this._baseTextBox.MouseEnter += value;
			}
			remove
			{
				this._baseTextBox.MouseEnter -= value;
			}
		}

		public new event EventHandler MouseHover
		{
			add
			{
				this._baseTextBox.MouseHover += value;
			}
			remove
			{
				this._baseTextBox.MouseHover -= value;
			}
		}

		public new event EventHandler MouseLeave
		{
			add
			{
				this._baseTextBox.MouseLeave += value;
			}
			remove
			{
				this._baseTextBox.MouseLeave -= value;
			}
		}

		public new event MouseEventHandler MouseMove
		{
			add
			{
				this._baseTextBox.MouseMove += value;
			}
			remove
			{
				this._baseTextBox.MouseMove -= value;
			}
		}

		public new event MouseEventHandler MouseUp
		{
			add
			{
				this._baseTextBox.MouseUp += value;
			}
			remove
			{
				this._baseTextBox.MouseUp -= value;
			}
		}

		public new event MouseEventHandler MouseWheel
		{
			add
			{
				this._baseTextBox.MouseWheel += value;
			}
			remove
			{
				this._baseTextBox.MouseWheel -= value;
			}
		}

		public new event EventHandler Move
		{
			add
			{
				this._baseTextBox.Move += value;
			}
			remove
			{
				this._baseTextBox.Move -= value;
			}
		}

		public event EventHandler MultilineChanged
		{
			add
			{
				this._baseTextBox.MultilineChanged += value;
			}
			remove
			{
				this._baseTextBox.MultilineChanged -= value;
			}
		}

		public new event EventHandler PaddingChanged
		{
			add
			{
				this._baseTextBox.PaddingChanged += value;
			}
			remove
			{
				this._baseTextBox.PaddingChanged -= value;
			}
		}

		public new event PaintEventHandler Paint
		{
			add
			{
				this._baseTextBox.Paint += value;
			}
			remove
			{
				this._baseTextBox.Paint -= value;
			}
		}

		public new event EventHandler ParentChanged
		{
			add
			{
				this._baseTextBox.ParentChanged += value;
			}
			remove
			{
				this._baseTextBox.ParentChanged -= value;
			}
		}

		public new event PreviewKeyDownEventHandler PreviewKeyDown
		{
			add
			{
				this._baseTextBox.PreviewKeyDown += value;
			}
			remove
			{
				this._baseTextBox.PreviewKeyDown -= value;
			}
		}

		public new event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
		{
			add
			{
				this._baseTextBox.QueryAccessibilityHelp += value;
			}
			remove
			{
				this._baseTextBox.QueryAccessibilityHelp -= value;
			}
		}

		public new event QueryContinueDragEventHandler QueryContinueDrag
		{
			add
			{
				this._baseTextBox.QueryContinueDrag += value;
			}
			remove
			{
				this._baseTextBox.QueryContinueDrag -= value;
			}
		}

		public event EventHandler ReadOnlyChanged
		{
			add
			{
				this._baseTextBox.ReadOnlyChanged += value;
			}
			remove
			{
				this._baseTextBox.ReadOnlyChanged -= value;
			}
		}

		public new event EventHandler RegionChanged
		{
			add
			{
				this._baseTextBox.RegionChanged += value;
			}
			remove
			{
				this._baseTextBox.RegionChanged -= value;
			}
		}

		public new event EventHandler Resize
		{
			add
			{
				this._baseTextBox.Resize += value;
			}
			remove
			{
				this._baseTextBox.Resize -= value;
			}
		}

		public new event EventHandler RightToLeftChanged
		{
			add
			{
				this._baseTextBox.RightToLeftChanged += value;
			}
			remove
			{
				this._baseTextBox.RightToLeftChanged -= value;
			}
		}

		public new event EventHandler SizeChanged
		{
			add
			{
				this._baseTextBox.SizeChanged += value;
			}
			remove
			{
				this._baseTextBox.SizeChanged -= value;
			}
		}

		public new event EventHandler StyleChanged
		{
			add
			{
				this._baseTextBox.StyleChanged += value;
			}
			remove
			{
				this._baseTextBox.StyleChanged -= value;
			}
		}

		public new event EventHandler SystemColorsChanged
		{
			add
			{
				this._baseTextBox.SystemColorsChanged += value;
			}
			remove
			{
				this._baseTextBox.SystemColorsChanged -= value;
			}
		}

		public new event EventHandler TabIndexChanged
		{
			add
			{
				this._baseTextBox.TabIndexChanged += value;
			}
			remove
			{
				this._baseTextBox.TabIndexChanged -= value;
			}
		}

		public new event EventHandler TabStopChanged
		{
			add
			{
				this._baseTextBox.TabStopChanged += value;
			}
			remove
			{
				this._baseTextBox.TabStopChanged -= value;
			}
		}

		public event EventHandler TextAlignChanged
		{
			add
			{
				this._baseTextBox.TextAlignChanged += value;
			}
			remove
			{
				this._baseTextBox.TextAlignChanged -= value;
			}
		}

		public new event EventHandler TextChanged
		{
			add
			{
				this._baseTextBox.TextChanged += value;
			}
			remove
			{
				this._baseTextBox.TextChanged -= value;
			}
		}

		public new event EventHandler Validated
		{
			add
			{
				this._baseTextBox.Validated += value;
			}
			remove
			{
				this._baseTextBox.Validated -= value;
			}
		}

		public new event CancelEventHandler Validating
		{
			add
			{
				this._baseTextBox.Validating += value;
			}
			remove
			{
				this._baseTextBox.Validating -= value;
			}
		}

		public new event EventHandler VisibleChanged
		{
			add
			{
				this._baseTextBox.VisibleChanged += value;
			}
			remove
			{
				this._baseTextBox.VisibleChanged -= value;
			}
		}

		public void SelectAll()
		{
			this._baseTextBox.SelectAll();
		}

		public void Clear()
		{
			this._baseTextBox.Clear();
		}

		public new void Focus()
		{
			this._baseTextBox.Focus();
		}

		public MaterialSingleLineTextField()
		{
			base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
			this._animationManager = new AnimationManager(true)
			{
				Increment = 0.06,
				AnimationType = AnimationType.EaseInOut,
				InterruptAnimation = false
			};
			this._animationManager.OnAnimationProgress += delegate
			{
				base.Invalidate();
			};
			this._baseTextBox = new BaseTextBox
			{
				BorderStyle = BorderStyle.None,
				Font = this.SkinManager.ROBOTO_REGULAR_11,
				ForeColor = this.SkinManager.GetPrimaryTextColor(),
				Location = new Point(0, 0),
				Width = base.Width,
				Height = base.Height - 5
			};
			if (!base.Controls.Contains(this._baseTextBox) && !base.DesignMode)
			{
				base.Controls.Add(this._baseTextBox);
			}
			this._baseTextBox.GotFocus += delegate
			{
				this._animationManager.StartNewAnimation(AnimationDirection.In, null);
			};
			this._baseTextBox.LostFocus += delegate
			{
				this._animationManager.StartNewAnimation(AnimationDirection.Out, null);
			};
			base.BackColorChanged += delegate
			{
				this._baseTextBox.BackColor = this.BackColor;
				this._baseTextBox.ForeColor = this.SkinManager.GetPrimaryTextColor();
			};
			this._baseTextBox.TabStop = true;
			base.TabStop = false;
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			Graphics graphics = pevent.Graphics;
			graphics.Clear(base.Parent.BackColor);
			int y = this._baseTextBox.Bottom + 3;
			Point location;
			if (!this._animationManager.IsAnimating())
			{
				Graphics graphics2 = graphics;
				Brush brush = this._baseTextBox.Focused ? this.SkinManager.ColorScheme.PrimaryBrush : this.SkinManager.GetDividersBrush();
				location = this._baseTextBox.Location;
				graphics2.FillRectangle(brush, location.X, y, this._baseTextBox.Width, (!this._baseTextBox.Focused) ? 1 : 2);
			}
			else
			{
				int num = (int)((double)this._baseTextBox.Width * this._animationManager.GetProgress());
				int num2 = num / 2;
				location = this._baseTextBox.Location;
				int num3 = location.X + this._baseTextBox.Width / 2;
				Graphics graphics3 = graphics;
				Brush dividersBrush = this.SkinManager.GetDividersBrush();
				location = this._baseTextBox.Location;
				graphics3.FillRectangle(dividersBrush, location.X, y, this._baseTextBox.Width, 1);
				graphics.FillRectangle(this.SkinManager.ColorScheme.PrimaryBrush, num3 - num2, y, num, 2);
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this._baseTextBox.Location = new Point(0, 0);
			this._baseTextBox.Width = base.Width;
			base.Height = this._baseTextBox.Height + 5;
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			this._baseTextBox.BackColor = base.Parent.BackColor;
			this._baseTextBox.ForeColor = this.SkinManager.GetPrimaryTextColor();
		}
	}
}
