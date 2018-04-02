using System.Drawing;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MouseMessageFilter : IMessageFilter
	{
		private const int WM_MOUSEMOVE = 512;

		public static event MouseEventHandler MouseMove;

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == 512 && MouseMessageFilter.MouseMove != null)
			{
				Point mousePosition = Control.MousePosition;
				int x = mousePosition.X;
				mousePosition = Control.MousePosition;
				int y = mousePosition.Y;
				MouseMessageFilter.MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
			}
			return false;
		}
	}
}
