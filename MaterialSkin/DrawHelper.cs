using System.Drawing;
using System.Drawing.Drawing2D;

namespace MaterialSkin
{
	internal static class DrawHelper
	{
		public static GraphicsPath CreateRoundRect(float x, float y, float width, float height, float radius)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.AddLine(x + radius, y, x + width - radius * 2f, y);
			graphicsPath.AddArc(x + width - radius * 2f, y, radius * 2f, radius * 2f, 270f, 90f);
			graphicsPath.AddLine(x + width, y + radius, x + width, y + height - radius * 2f);
			graphicsPath.AddArc(x + width - radius * 2f, y + height - radius * 2f, radius * 2f, radius * 2f, 0f, 90f);
			graphicsPath.AddLine(x + width - radius * 2f, y + height, x + radius, y + height);
			graphicsPath.AddArc(x, y + height - radius * 2f, radius * 2f, radius * 2f, 90f, 90f);
			graphicsPath.AddLine(x, y + height - radius * 2f, x, y + radius);
			graphicsPath.AddArc(x, y, radius * 2f, radius * 2f, 180f, 90f);
			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		public static GraphicsPath CreateRoundRect(Rectangle rect, float radius)
		{
			return DrawHelper.CreateRoundRect((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, radius);
		}

		public static Color BlendColor(Color backgroundColor, Color frontColor, double blend)
		{
			double num = blend / 255.0;
			double num2 = 1.0 - num;
			int red = (int)((double)(int)backgroundColor.R * num2 + (double)(int)frontColor.R * num);
			int green = (int)((double)(int)backgroundColor.G * num2 + (double)(int)frontColor.G * num);
			int blue = (int)((double)(int)backgroundColor.B * num2 + (double)(int)frontColor.B * num);
			return Color.FromArgb(red, green, blue);
		}

		public static Color BlendColor(Color backgroundColor, Color frontColor)
		{
			return DrawHelper.BlendColor(backgroundColor, frontColor, (double)(int)frontColor.A);
		}
	}
}
