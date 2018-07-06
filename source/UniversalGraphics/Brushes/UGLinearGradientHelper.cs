using System;
using System.Numerics;

namespace UniversalGraphics
{
	internal static class UGLinearGradientHelper
	{
		public static Vector2 EndPointFromAngle(float angle)
		{
			var radians = MathHelper.DegreesToRadians(angle);
			return new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
		}
	}
}
