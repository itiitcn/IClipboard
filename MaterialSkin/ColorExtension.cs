using System.Drawing;

namespace MaterialSkin
{
	public static class ColorExtension
	{
		public static Color ToColor(this int argb)
		{
			return Color.FromArgb((argb & 0xFF0000) >> 16, (argb & 0xFF00) >> 8, argb & 0xFF);
		}

		public static Color RemoveAlpha(this Color color)
		{
			return Color.FromArgb(color.R, color.G, color.B);
		}

		public static int PercentageToColorComponent(this int percentage)
		{
			return (int)((double)percentage / 100.0 * 255.0);
		}
	}
}
