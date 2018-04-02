using System.ComponentModel;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialLabel : Label, IMaterialControl
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

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			this.ForeColor = this.SkinManager.GetPrimaryTextColor();
			this.Font = this.SkinManager.ROBOTO_REGULAR_11;
			base.BackColorChanged += delegate
			{
				this.ForeColor = this.SkinManager.GetPrimaryTextColor();
			};
		}
	}
}
