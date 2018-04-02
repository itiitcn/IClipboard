using MaterialSkin.Animations;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialFlatButton : Button, IMaterialControl
	{
		private readonly AnimationManager _animationManager;

		private readonly AnimationManager _hoverAnimationManager;

		private SizeF _textSize;

		private Image _icon;

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

		public bool Primary
		{
			get;
			set;
		}

		public Image Icon
		{
			get
			{
				return this._icon;
			}
			set
			{
				this._icon = value;
				if (this.AutoSize)
				{
					base.Size = this.GetPreferredSize();
				}
				base.Invalidate();
			}
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
				this._textSize = base.CreateGraphics().MeasureString(value.ToUpper(), this.SkinManager.ROBOTO_MEDIUM_10);
				if (this.AutoSize)
				{
					base.Size = this.GetPreferredSize();
				}
				base.Invalidate();
			}
		}

		public MaterialFlatButton()
		{
			this.Primary = false;
			this._animationManager = new AnimationManager(false)
			{
				Increment = 0.03,
				AnimationType = AnimationType.EaseOut
			};
			this._hoverAnimationManager = new AnimationManager(true)
			{
				Increment = 0.07,
				AnimationType = AnimationType.Linear
			};
			this._hoverAnimationManager.OnAnimationProgress += delegate
			{
				base.Invalidate();
			};
			this._animationManager.OnAnimationProgress += delegate
			{
				base.Invalidate();
			};
			base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.AutoSize = true;
			base.Margin = new Padding(4, 6, 4, 6);
			base.Padding = new Padding(0);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			Graphics graphics = pevent.Graphics;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			graphics.Clear(base.Parent.BackColor);
			Color flatButtonHoverBackgroundColor = this.SkinManager.GetFlatButtonHoverBackgroundColor();
			using (Brush brush = new SolidBrush(Color.FromArgb((int)(this._hoverAnimationManager.GetProgress() * (double)(int)flatButtonHoverBackgroundColor.A), flatButtonHoverBackgroundColor.RemoveAlpha())))
			{
				graphics.FillRectangle(brush, base.ClientRectangle);
			}
			if (this._animationManager.IsAnimating())
			{
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				for (int i = 0; i < this._animationManager.GetAnimationCount(); i++)
				{
					double progress = this._animationManager.GetProgress(i);
					Point source = this._animationManager.GetSource(i);
					using (Brush brush2 = new SolidBrush(Color.FromArgb((int)(101.0 - progress * 100.0), Color.Black)))
					{
						int num = (int)(progress * (double)base.Width * 2.0);
						graphics.FillEllipse(brush2, new Rectangle(source.X - num / 2, source.Y - num / 2, num, num));
					}
				}
				graphics.SmoothingMode = SmoothingMode.None;
			}
			Rectangle rect = new Rectangle(8, 6, 24, 24);
			if (string.IsNullOrEmpty(this.Text))
			{
				rect.X += 2;
			}
			if (this.Icon != null)
			{
				graphics.DrawImage(this.Icon, rect);
			}
			Rectangle clientRectangle = base.ClientRectangle;
			if (this.Icon != null)
			{
				clientRectangle.Width -= 44;
				clientRectangle.X += 36;
			}
			graphics.DrawString(this.Text.ToUpper(), this.SkinManager.ROBOTO_MEDIUM_10, base.Enabled ? (this.Primary ? this.SkinManager.ColorScheme.PrimaryBrush : this.SkinManager.GetPrimaryTextBrush()) : this.SkinManager.GetFlatButtonDisabledTextBrush(), clientRectangle, new StringFormat
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center
			});
		}

		private Size GetPreferredSize()
		{
			return this.GetPreferredSize(new Size(0, 0));
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			int num = 16;
			if (this.Icon != null)
			{
				num += 28;
			}
			return new Size((int)Math.Ceiling((double)this._textSize.Width) + num, 36);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			if (!base.DesignMode)
			{
				this.MouseState = MouseState.OUT;
				base.MouseEnter += delegate
				{
					this.MouseState = MouseState.HOVER;
					this._hoverAnimationManager.StartNewAnimation(AnimationDirection.In, null);
					base.Invalidate();
				};
				base.MouseLeave += delegate
				{
					this.MouseState = MouseState.OUT;
					this._hoverAnimationManager.StartNewAnimation(AnimationDirection.Out, null);
					base.Invalidate();
				};
				base.MouseDown += delegate(object sender, MouseEventArgs args)
				{
					if (args.Button == MouseButtons.Left)
					{
						this.MouseState = MouseState.DOWN;
						this._animationManager.StartNewAnimation(AnimationDirection.In, args.Location, null);
						base.Invalidate();
					}
				};
				base.MouseUp += delegate
				{
					this.MouseState = MouseState.HOVER;
					base.Invalidate();
				};
			}
		}
	}
}
