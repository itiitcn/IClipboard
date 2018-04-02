namespace MaterialSkin.Animations
{
	public static class AnimationEaseOut
	{
		public static double CalculateProgress(double progress)
		{
			return -1.0 * progress * (progress - 2.0);
		}
	}
}
