using System.Drawing;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialToolStripMenuItem : ToolStripMenuItem
	{
		public MaterialToolStripMenuItem()
		{
			base.AutoSize = false;
			this.Size = new Size(120, 30);
		}

		protected override ToolStripDropDown CreateDefaultDropDown()
		{
			ToolStripDropDown toolStripDropDown = base.CreateDefaultDropDown();
			if (base.DesignMode)
			{
				return toolStripDropDown;
			}
			MaterialContextMenuStrip materialContextMenuStrip = new MaterialContextMenuStrip();
			materialContextMenuStrip.Items.AddRange(toolStripDropDown.Items);
			return materialContextMenuStrip;
		}
	}
}
