using System.ComponentModel;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public sealed class MaterialDivider : Control, IMaterialControl
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

		public MaterialDivider()
		{
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			base.Height = 1;
			this.BackColor = this.SkinManager.GetDividersColor();
		}
	}
}
