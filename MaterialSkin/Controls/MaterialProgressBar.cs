using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialProgressBar : ProgressBar, IMaterialControl
	{
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

		public MaterialProgressBar()
		{
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, width, 5, specified);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle clipRectangle = e.ClipRectangle;
			int num = (int)((double)clipRectangle.Width * ((double)base.Value / (double)base.Maximum));
			Graphics graphics = e.Graphics;
			Brush primaryBrush = this.SkinManager.ColorScheme.PrimaryBrush;
			int width = num;
			clipRectangle = e.ClipRectangle;
			graphics.FillRectangle(primaryBrush, 0, 0, width, clipRectangle.Height);
			Graphics graphics2 = e.Graphics;
			Brush disabledOrHintBrush = this.SkinManager.GetDisabledOrHintBrush();
			int x = num;
			clipRectangle = e.ClipRectangle;
			int width2 = clipRectangle.Width;
			clipRectangle = e.ClipRectangle;
			graphics2.FillRectangle(disabledOrHintBrush, x, 0, width2, clipRectangle.Height);
		}
	}
}
