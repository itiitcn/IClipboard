using MaterialSkin.Animations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	internal class MaterialToolStripRender : ToolStripProfessionalRenderer, IMaterialControl
	{
		public int Depth
		{
			get;
			set;
		}

		public MaterialSkinManager SkinManager
		{
			get
			{
				return MaterialSkinManager.Instance;
			}
		}

		public MouseState MouseState
		{
			get;
			set;
		}

		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			Rectangle itemRect = this.GetItemRect(e.Item);
			Rectangle r = new Rectangle(24, itemRect.Y, itemRect.Width - 40, itemRect.Height);
			graphics.DrawString(e.Text, this.SkinManager.ROBOTO_MEDIUM_10, e.Item.Enabled ? this.SkinManager.GetPrimaryTextBrush() : this.SkinManager.GetDisabledOrHintBrush(), r, new StringFormat
			{
				LineAlignment = StringAlignment.Center
			});
		}

		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			graphics.Clear(this.SkinManager.GetApplicationBackgroundColor());
			Rectangle itemRect = this.GetItemRect(e.Item);
			graphics.FillRectangle((e.Item.Selected && e.Item.Enabled) ? this.SkinManager.GetCmsSelectedItemBrush() : new SolidBrush(this.SkinManager.GetApplicationBackgroundColor()), itemRect);
			MaterialContextMenuStrip materialContextMenuStrip = e.ToolStrip as MaterialContextMenuStrip;
			if (materialContextMenuStrip != null)
			{
				AnimationManager animationManager = materialContextMenuStrip.AnimationManager;
				Point animationSource = materialContextMenuStrip.AnimationSource;
				if (materialContextMenuStrip.AnimationManager.IsAnimating() && e.Item.Bounds.Contains(animationSource))
				{
					for (int i = 0; i < animationManager.GetAnimationCount(); i++)
					{
						double progress = animationManager.GetProgress(i);
						SolidBrush brush = new SolidBrush(Color.FromArgb((int)(51.0 - progress * 50.0), Color.Black));
						int num = (int)(progress * (double)itemRect.Width * 2.5);
						graphics.FillEllipse(brush, new Rectangle(animationSource.X - num / 2, itemRect.Y - itemRect.Height, num, itemRect.Height * 3));
					}
				}
			}
		}

		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
		}

		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			graphics.FillRectangle(new SolidBrush(this.SkinManager.GetApplicationBackgroundColor()), e.Item.Bounds);
			Graphics graphics2 = graphics;
			Pen pen = new Pen(this.SkinManager.GetDividersColor());
			Rectangle bounds = e.Item.Bounds;
			int left = bounds.Left;
			bounds = e.Item.Bounds;
			Point pt = new Point(left, bounds.Height / 2);
			bounds = e.Item.Bounds;
			int right = bounds.Right;
			bounds = e.Item.Bounds;
			graphics2.DrawLine(pen, pt, new Point(right, bounds.Height / 2));
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Graphics graphics2 = graphics;
			Pen pen = new Pen(this.SkinManager.GetDividersColor());
			Rectangle affectedBounds = e.AffectedBounds;
			int x = affectedBounds.X;
			affectedBounds = e.AffectedBounds;
			int y = affectedBounds.Y;
			affectedBounds = e.AffectedBounds;
			int width = affectedBounds.Width - 1;
			affectedBounds = e.AffectedBounds;
			graphics2.DrawRectangle(pen, new Rectangle(x, y, width, affectedBounds.Height - 1));
		}

		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Rectangle arrowRectangle = e.ArrowRectangle;
			int x = arrowRectangle.X;
			arrowRectangle = e.ArrowRectangle;
			int x2 = x + arrowRectangle.Width / 2;
			arrowRectangle = e.ArrowRectangle;
			int y = arrowRectangle.Y;
			arrowRectangle = e.ArrowRectangle;
			Point point = new Point(x2, y + arrowRectangle.Height / 2);
			Brush brush = e.Item.Enabled ? this.SkinManager.GetPrimaryTextBrush() : this.SkinManager.GetDisabledOrHintBrush();
			using (GraphicsPath graphicsPath = new GraphicsPath())
			{
				graphicsPath.AddLines(new Point[3]
				{
					new Point(point.X - 4, point.Y - 4),
					new Point(point.X, point.Y),
					new Point(point.X - 4, point.Y + 4)
				});
				graphicsPath.CloseFigure();
				graphics.FillPath(brush, graphicsPath);
			}
		}

		private Rectangle GetItemRect(ToolStripItem item)
		{
			Rectangle contentRectangle = item.ContentRectangle;
			int y = contentRectangle.Y;
			contentRectangle = item.ContentRectangle;
			int width = contentRectangle.Width + 4;
			contentRectangle = item.ContentRectangle;
			return new Rectangle(0, y, width, contentRectangle.Height);
		}
	}
}
