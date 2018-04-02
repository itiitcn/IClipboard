using MaterialSkin.Animations;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialRadioButton : RadioButton, IMaterialControl
	{
		private bool ripple;

		private readonly AnimationManager _animationManager;

		private readonly AnimationManager _rippleAnimationManager;

		private Rectangle _radioButtonBounds;

		private int _boxOffset;

		private const int RADIOBUTTON_SIZE = 19;

		private const int RADIOBUTTON_SIZE_HALF = 9;

		private const int RADIOBUTTON_OUTER_CIRCLE_WIDTH = 2;

		private const int RADIOBUTTON_INNER_CIRCLE_SIZE = 15;

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
				return this.ripple;
			}
			set
			{
				this.ripple = value;
				this.AutoSize = this.AutoSize;
				if (value)
				{
					base.Margin = new Padding(0);
				}
				base.Invalidate();
			}
		}

		public MaterialRadioButton()
		{
			base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
			this._animationManager = new AnimationManager(true)
			{
				AnimationType = AnimationType.EaseInOut,
				Increment = 0.06
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
			base.SizeChanged += this.OnSizeChanged;
			this.Ripple = true;
			this.MouseLocation = new Point(-1, -1);
		}

		private void OnSizeChanged(object sender, EventArgs eventArgs)
		{
			this._boxOffset = base.Height / 2 - (int)Math.Ceiling(9.5);
			this._radioButtonBounds = new Rectangle(this._boxOffset, this._boxOffset, 19, 19);
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			int width = this._boxOffset + 20 + (int)base.CreateGraphics().MeasureString(this.Text, this.SkinManager.ROBOTO_MEDIUM_10).Width;
			return this.Ripple ? new Size(width, 30) : new Size(width, 20);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			Graphics graphics = pevent.Graphics;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			graphics.Clear(base.Parent.BackColor);
			int num = this._boxOffset + 9;
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
			float num5 = (float)(progress * 8.0);
			float num6 = num5 / 2f;
			num5 = (float)(progress * 9.0);
			SolidBrush solidBrush = new SolidBrush(Color.FromArgb(alpha, base.Enabled ? this.SkinManager.ColorScheme.AccentColor : this.SkinManager.GetCheckBoxOffDisabledColor()));
			Pen pen = new Pen(solidBrush.Color);
			if (this.Ripple && this._rippleAnimationManager.IsAnimating())
			{
				for (int i = 0; i < this._rippleAnimationManager.GetAnimationCount(); i++)
				{
					double progress2 = this._rippleAnimationManager.GetProgress(i);
					Point point = new Point(num, num);
					SolidBrush solidBrush2 = new SolidBrush(Color.FromArgb((int)(progress2 * 40.0), ((bool)this._rippleAnimationManager.GetData(i)[0]) ? Color.Black : solidBrush.Color));
					int num7 = (base.Height % 2 == 0) ? (base.Height - 3) : (base.Height - 2);
					int num8 = (this._rippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn) ? ((int)((double)num7 * (0.8 + 0.2 * progress2))) : num7;
					using (GraphicsPath path = DrawHelper.CreateRoundRect((float)(point.X - num8 / 2), (float)(point.Y - num8 / 2), (float)num8, (float)num8, (float)(num8 / 2)))
					{
						graphics.FillPath(solidBrush2, path);
					}
					solidBrush2.Dispose();
				}
			}
			Color color2 = DrawHelper.BlendColor(base.Parent.BackColor, base.Enabled ? this.SkinManager.GetCheckboxOffColor() : this.SkinManager.GetCheckBoxOffDisabledColor(), (double)num4);
			using (GraphicsPath path2 = DrawHelper.CreateRoundRect((float)this._boxOffset, (float)this._boxOffset, 19f, 19f, 9f))
			{
				graphics.FillPath(new SolidBrush(color2), path2);
				if (base.Enabled)
				{
					graphics.FillPath(solidBrush, path2);
				}
			}
			graphics.FillEllipse(new SolidBrush(base.Parent.BackColor), 2 + this._boxOffset, 2 + this._boxOffset, 15, 15);
			if (base.Checked)
			{
				using (GraphicsPath path3 = DrawHelper.CreateRoundRect((float)num - num6, (float)num - num6, num5, num5, 4f))
				{
					graphics.FillPath(solidBrush, path3);
				}
			}
			SizeF sizeF = graphics.MeasureString(this.Text, this.SkinManager.ROBOTO_MEDIUM_10);
			graphics.DrawString(this.Text, this.SkinManager.ROBOTO_MEDIUM_10, base.Enabled ? this.SkinManager.GetPrimaryTextBrush() : this.SkinManager.GetDisabledOrHintBrush(), (float)(this._boxOffset + 22), (float)(base.Height / 2) - sizeF.Height / 2f);
			solidBrush.Dispose();
			pen.Dispose();
		}

		private bool IsMouseInCheckArea()
		{
			return this._radioButtonBounds.Contains(this.MouseLocation);
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
