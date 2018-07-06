using System;

namespace UniversalGraphics
{
	internal static class MathHelper
	{
		public const float PI = (float)Math.PI;
		public const double PI2D = 2.0 * Math.PI;

		private const float R2D = 180F / PI;
		private const float D2R = PI / 180F;

		public static float RadiansToDegrees(float radians)
			=> R2D * radians;

		public static float DegreesToRadians(float degrees)
			=> D2R * degrees;
	}
}
