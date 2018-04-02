using MaterialSkin.Animations;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialTabSelector : Control, IMaterialControl
	{
		private MaterialTabControl _baseTabControl;

		private int _previousSelectedTabIndex;

		private Point _animationSource;

		private readonly AnimationManager _animationManager;

		private List<Rectangle> _tabRects;

		private const int TAB_HEADER_PADDING = 24;

		private const int TAB_INDICATOR_HEIGHT = 2;

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

		public MaterialTabControl BaseTabControl
		{
			get
			{
				return this._baseTabControl;
			}
			set
			{
				this._baseTabControl = value;
				if (this._baseTabControl != null)
				{
					this._previousSelectedTabIndex = this._baseTabControl.SelectedIndex;
					this._baseTabControl.Deselected += delegate
					{
						this._previousSelectedTabIndex = this._baseTabControl.SelectedIndex;
					};
					this._baseTabControl.SelectedIndexChanged += delegate
					{
						this._animationManager.SetProgress(0.0);
						this._animationManager.StartNewAnimation(AnimationDirection.In, null);
					};
					this._baseTabControl.ControlAdded += delegate
					{
						base.Invalidate();
					};
					this._baseTabControl.ControlRemoved += delegate
					{
						base.Invalidate();
					};
				}
			}
		}

		public MaterialTabSelector()
		{
			base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
			base.Height = 48;
			this._animationManager = new AnimationManager(true)
			{
				AnimationType = AnimationType.EaseOut,
				Increment = 0.04
			};
			this._animationManager.OnAnimationProgress += delegate
			{
				base.Invalidate();
			};
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			graphics.Clear(this.SkinManager.ColorScheme.PrimaryColor);
			if (this._baseTabControl != null)
			{
				if (!this._animationManager.IsAnimating() || this._tabRects == null || this._tabRects.Count != this._baseTabControl.TabCount)
				{
					this.UpdateTabRects();
				}
				double progress = this._animationManager.GetProgress();
				if (this._animationManager.IsAnimating())
				{
					SolidBrush solidBrush = new SolidBrush(Color.FromArgb((int)(51.0 - progress * 50.0), Color.White));
					int num = (int)(progress * (double)this._tabRects[this._baseTabControl.SelectedIndex].Width * 1.75);
					graphics.SetClip(this._tabRects[this._baseTabControl.SelectedIndex]);
					graphics.FillEllipse(solidBrush, new Rectangle(this._animationSource.X - num / 2, this._animationSource.Y - num / 2, num, num));
					graphics.ResetClip();
					solidBrush.Dispose();
				}
				foreach (TabPage tabPage in this._baseTabControl.TabPages)
				{
					int num2 = this._baseTabControl.TabPages.IndexOf(tabPage);
					Brush brush = new SolidBrush(Color.FromArgb(this.CalculateTextAlpha(num2, progress), this.SkinManager.ColorScheme.TextColor));
					graphics.DrawString(tabPage.Text.ToUpper(), this.SkinManager.ROBOTO_MEDIUM_10, brush, this._tabRects[num2], new StringFormat
					{
						Alignment = StringAlignment.Center,
						LineAlignment = StringAlignment.Center
					});
					brush.Dispose();
				}
				int index = (this._previousSelectedTabIndex == -1) ? this._baseTabControl.SelectedIndex : this._previousSelectedTabIndex;
				Rectangle rectangle = this._tabRects[index];
				Rectangle rectangle2 = this._tabRects[this._baseTabControl.SelectedIndex];
				int y = rectangle2.Bottom - 2;
				int x = rectangle.X + (int)((double)(rectangle2.X - rectangle.X) * progress);
				int width = rectangle.Width + (int)((double)(rectangle2.Width - rectangle.Width) * progress);
				graphics.FillRectangle(this.SkinManager.ColorScheme.AccentBrush, x, y, width, 2);
			}
		}

		private int CalculateTextAlpha(int tabIndex, double animationProgress)
		{
			Color color = this.SkinManager.ACTION_BAR_TEXT;
			int a = color.A;
			color = this.SkinManager.ACTION_BAR_TEXT_SECONDARY;
			int a2 = color.A;
			if (tabIndex == this._baseTabControl.SelectedIndex && !this._animationManager.IsAnimating())
			{
				return a;
			}
			if (tabIndex != this._previousSelectedTabIndex && tabIndex != this._baseTabControl.SelectedIndex)
			{
				return a2;
			}
			if (tabIndex == this._previousSelectedTabIndex)
			{
				return a - (int)((double)(a - a2) * animationProgress);
			}
			return a2 + (int)((double)(a - a2) * animationProgress);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (this._tabRects == null)
			{
				this.UpdateTabRects();
			}
			for (int i = 0; i < this._tabRects.Count; i++)
			{
				if (this._tabRects[i].Contains(e.Location))
				{
					this._baseTabControl.SelectedIndex = i;
				}
			}
			this._animationSource = e.Location;
		}

		private void UpdateTabRects()
		{
			this._tabRects = new List<Rectangle>();
			if (this._baseTabControl != null && this._baseTabControl.TabCount != 0)
			{
				using (Bitmap image = new Bitmap(1, 1))
				{
					using (Graphics graphics = Graphics.FromImage(image))
					{
						List<Rectangle> tabRects = this._tabRects;
						int fORM_PADDING = this.SkinManager.FORM_PADDING;
						SizeF sizeF = graphics.MeasureString(this._baseTabControl.TabPages[0].Text, this.SkinManager.ROBOTO_MEDIUM_10);
						tabRects.Add(new Rectangle(fORM_PADDING, 0, 48 + (int)sizeF.Width, base.Height));
						for (int i = 1; i < this._baseTabControl.TabPages.Count; i++)
						{
							List<Rectangle> tabRects2 = this._tabRects;
							int right = this._tabRects[i - 1].Right;
							sizeF = graphics.MeasureString(this._baseTabControl.TabPages[i].Text, this.SkinManager.ROBOTO_MEDIUM_10);
							tabRects2.Add(new Rectangle(right, 0, 48 + (int)sizeF.Width, base.Height));
						}
					}
				}
			}
		}
	}
}
