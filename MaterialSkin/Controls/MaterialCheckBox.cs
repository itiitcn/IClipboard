using MaterialSkin.Animations;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialCheckBox : CheckBox, IMaterialControl
	{
		private bool _ripple;

		private readonly AnimationManager _animationManager;

		private readonly AnimationManager _rippleAnimationManager;

		private const int CHECKBOX_SIZE = 18;

		private const int CHECKBOX_SIZE_HALF = 9;

		private const int CHECKBOX_INNER_BOX_SIZE = 14;

		private int _boxOffset;

		private Rectangle _boxRectangle;

		private static readonly Point[] CheckmarkLine = new Point[3]
		{
			new Point(3, 8),
			new Point(7, 12),
			new Point(14, 5)
		};

		private const int TEXT_OFFSET = 22;

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

		[Category("Behavior")]
		public bool Ripple
		{
			get
			{
				return this._ripple;
			}
			set
			{
				this._ripple = value;
				this.AutoSize = this.AutoSize;
				if (value)
				{
					base.Margin = new Padding(0);
				}
				base.Invalidate();
			}
		}

		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
				if (value)
				{
					base.Size = new Size(10, 10);
				}
			}
		}

		public MaterialCheckBox()
		{
			this._animationManager = new AnimationManager(true)
			{
				AnimationType = AnimationType.EaseInOut,
				Increment = 0.05
			};
			this._rippleAnimationManager = new AnimationManager(false)
			{
				AnimationType = AnimationType.Linear,
				Increment = 0.1,
				SecondaryIncrement = 0.08
			};
			this._animationManager.OnAnimationProgress += delegate
			{
				base.Invalidate();
			};
			this._rippleAnimationManager.OnAnimationProgress += delegate
			{
				base.Invalidate();
			};
			base.CheckedChanged += delegate
			{
				this._animationManager.StartNewAnimation((AnimationDirection)((!base.Checked) ? 1 : 0), null);
			};
			this.Ripple = true;
			this.MouseLocation = new Point(-1, -1);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this._boxOffset = base.Height / 2 - 9;
			this._boxRectangle = new Rectangle(this._boxOffset, this._boxOffset, 17, 17);
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			int width = this._boxOffset + 18 + 2 + (int)base.CreateGraphics().MeasureString(this.Text, this.SkinManager.ROBOTO_MEDIUM_10).Width;
			return this.Ripple ? new Size(width, 30) : new Size(width, 20);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			Graphics graphics = pevent.Graphics;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			graphics.Clear(base.Parent.BackColor);
			int num = this._boxOffset + 9 - 1;
			double progress = this._animationManager.GetProgress();
			Color color;
			int num2;
			if (!base.Enabled)
			{
				color = this.SkinManager.GetCheckBoxOffDisabledColor();
				num2 = color.A;
			}
			else
			{
				num2 = (int)(progress * 255.0);
			}
			int alpha = num2;
			int num3;
			if (!base.Enabled)
			{
				color = this.SkinManager.GetCheckBoxOffDisabledColor();
				num3 = color.A;
			}
			else
			{
				color = this.SkinManager.GetCheckboxOffColor();
				num3 = (int)((double)(int)color.A * (1.0 - progress));
			}
			int num4 = num3;
			SolidBrush solidBrush = new SolidBrush(Color.FromArgb(alpha, base.Enabled ? this.SkinManager.ColorScheme.AccentColor : this.SkinManager.GetCheckBoxOffDisabledColor()));
			SolidBrush solidBrush2 = new SolidBrush(base.Enabled ? this.SkinManager.ColorScheme.AccentColor : this.SkinManager.GetCheckBoxOffDisabledColor());
			Pen pen = new Pen(solidBrush.Color);
			if (this.Ripple && this._rippleAnimationManager.IsAnimating())
			{
				for (int i = 0; i < this._rippleAnimationManager.GetAnimationCount(); i++)
				{
					double progress2 = this._rippleAnimationManager.GetProgress(i);
					Point point = new Point(num, num);
					SolidBrush solidBrush3 = new SolidBrush(Color.FromArgb((int)(progress2 * 40.0), ((bool)this._rippleAnimationManager.GetData(i)[0]) ? Color.Black : solidBrush.Color));
					int num5 = (base.Height % 2 == 0) ? (base.Height - 3) : (base.Height - 2);
					int num6 = (this._rippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn) ? ((int)((double)num5 * (0.8 + 0.2 * progress2))) : num5;
					using (GraphicsPath path = DrawHelper.CreateRoundRect((float)(point.X - num6 / 2), (float)(point.Y - num6 / 2), (float)num6, (float)num6, (float)(num6 / 2)))
					{
						graphics.FillPath(solidBrush3, path);
					}
					solidBrush3.Dispose();
				}
			}
			solidBrush2.Dispose();
			Rectangle rect = new Rectangle(this._boxOffset, this._boxOffset, (int)(17.0 * progress), 17);
			using (GraphicsPath path2 = DrawHelper.CreateRoundRect((float)this._boxOffset, (float)this._boxOffset, 17f, 17f, 1f))
			{
				SolidBrush solidBrush4 = new SolidBrush(DrawHelper.BlendColor(base.Parent.BackColor, base.Enabled ? this.SkinManager.GetCheckboxOffColor() : this.SkinManager.GetCheckBoxOffDisabledColor(), (double)num4));
				Pen pen2 = new Pen(solidBrush4.Color);
				graphics.FillPath(solidBrush4, path2);
				graphics.DrawPath(pen2, path2);
				graphics.FillRectangle(new SolidBrush(base.Parent.BackColor), this._boxOffset + 2, this._boxOffset + 2, 13, 13);
				graphics.DrawRectangle(new Pen(base.Parent.BackColor), this._boxOffset + 2, this._boxOffset + 2, 13, 13);
				solidBrush4.Dispose();
				pen2.Dispose();
				if (base.Enabled)
				{
					graphics.FillPath(solidBrush, path2);
					graphics.DrawPath(pen, path2);
				}
				else if (base.Checked)
				{
					graphics.SmoothingMode = SmoothingMode.None;
					graphics.FillRectangle(solidBrush, this._boxOffset + 2, this._boxOffset + 2, 14, 14);
					graphics.SmoothingMode = SmoothingMode.AntiAlias;
				}
				graphics.DrawImageUnscaledAndClipped(this.DrawCheckMarkBitmap(), rect);
			}
			SizeF sizeF = graphics.MeasureString(this.Text, this.SkinManager.ROBOTO_MEDIUM_10);
			graphics.DrawString(this.Text, this.SkinManager.ROBOTO_MEDIUM_10, base.Enabled ? this.SkinManager.GetPrimaryTextBrush() : this.SkinManager.GetDisabledOrHintBrush(), (float)(this._boxOffset + 22), (float)(base.Height / 2) - sizeF.Height / 2f);
			pen.Dispose();
			solidBrush.Dispose();
		}

		private Bitmap DrawCheckMarkBitmap()
		{
			Bitmap bitmap = new Bitmap(18, 18);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(Color.Transparent);
			using (Pen pen = new Pen(base.Parent.BackColor, 2f))
			{
				graphics.DrawLines(pen, MaterialCheckBox.CheckmarkLine);
			}
			return bitmap;
		}

		private bool IsMouseInCheckArea()
		{
			return this._boxRectangle.Contains(this.MouseLocation);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			this.Font = this.SkinManager.ROBOTO_MEDIUM_10;
			if (!base.DesignMode)
			{
				this.MouseState = MouseState.OUT;
				base.MouseEnter += delegate
				{
					this.MouseState = MouseState.HOVER;
				};
				base.MouseLeave += delegate
				{
					this.MouseLocation = new Point(-1, -1);
					this.MouseState = MouseState.OUT;
				};
				base.MouseDown += delegate(object sender, MouseEventArgs args)
				{
					this.MouseState = MouseState.DOWN;
					if (this.Ripple && args.Button == MouseButtons.Left && this.IsMouseInCheckArea())
					{
						this._rippleAnimationManager.SecondaryIncrement = 0.0;
						this._rippleAnimationManager.StartNewAnimation(AnimationDirection.InOutIn, new object[1]
						{
							base.Checked
						});
					}
				};
				base.MouseUp += delegate
				{
					this.MouseState = MouseState.HOVER;
					this._rippleAnimationManager.SecondaryIncrement = 0.08;
				};
				base.MouseMove += delegate(object sender, MouseEventArgs args)
				{
					this.MouseLocation = args.Location;
					this.Cursor = (this.IsMouseInCheckArea() ? Cursors.Hand : Cursors.Default);
				};
			}
		}
	}
}
