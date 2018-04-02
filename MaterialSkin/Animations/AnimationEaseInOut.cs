using System;

namespace MaterialSkin.Animations
{
	internal static class AnimationEaseInOut
	{
		public static double PI = 3.1415926535897931;

		public static double PI_HALF = 1.5707963267948966;

		public static double CalculateProgress(double progress)
		{
			return AnimationEaseInOut.EaseInOut(progress);
		}

		private static double EaseInOut(double s)
		{
			return s - Math.Sin(s * 2.0 * AnimationEaseInOut.PI) / (2.0 * AnimationEaseInOut.PI);
		}
	}
}
