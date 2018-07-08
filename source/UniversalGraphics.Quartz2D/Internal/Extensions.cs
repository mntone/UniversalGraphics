using CoreGraphics;
using System;
using System.Linq;
using System.Numerics;

#if __IOS__ || __TVOS__ || __WATCHOS__
using UIKit;
#elif __MACOS__
using AppKit;
#endif

namespace UniversalGraphics.Quartz2D
{
	internal static class CGAffineTransformHelper
	{
		public static CGAffineTransform CreateScale(float scaleX, float scaleY, float centerX, float centerY)
		{
			var tx = centerX * (1F - scaleX);
			var ty = centerY * (1F - scaleY);

			CGAffineTransform result;
			result.xx = scaleX;
			result.xy = 0F;
			result.yx = 0F;
			result.yy = scaleY;
			result.x0 = tx;
			result.y0 = ty;
			return result;
		}

		public static CGAffineTransform CreateSkew(float radiansX, float radiansY)
		{
			var skewX = (float)Math.Tan(radiansX);
			var skewY = (float)Math.Tan(radiansY);

			CGAffineTransform result;
			result.xx = 1F;
			result.xy = skewX;
			result.yx = skewY;
			result.yy = 1F;
			result.x0 = 0F;
			result.y0 = 0F;
			return result;
		}

		public static CGAffineTransform CreateSkew(float radiansX, float radiansY, float centerX, float centerY)
		{
			var skewX = (float)Math.Tan(radiansX);
			var skewY = (float)Math.Tan(radiansY);
			var tx = -centerY * skewX;
			var ty = -centerX * skewY;

			CGAffineTransform result;
			result.xx = 1F;
			result.xy = skewX;
			result.yx = skewY;
			result.yy = 1F;
			result.x0 = tx;
			result.y0 = ty;
			return result;
		}

		public static CGAffineTransform CreateRotation(float radians, float centerX, float centerY)
		{
			radians = (float)Math.IEEERemainder(radians, 2.0 * Math.PI);

			float cos, sin;
			const float epsilon = 0.001F * (float)Math.PI / 180F;
			if (radians > -epsilon && radians < epsilon)
			{
				cos = 1F;
				sin = 0F;
			}
			else if (radians > Math.PI / 2 - epsilon && radians < Math.PI / 2 + epsilon)
			{
				cos = 0F;
				sin = 1F;
			}
			else if (radians < -Math.PI + epsilon || radians > Math.PI - epsilon)
			{
				cos = -1F;
				sin = 0F;
			}
			else if (radians > -Math.PI / 2 - epsilon && radians < -Math.PI / 2 + epsilon)
			{
				cos = 0F;
				sin = -1F;
			}
			else
			{
				cos = (float)Math.Cos(radians);
				sin = (float)Math.Sin(radians);
			}

			var tx = centerX * (1F - cos) + centerY * sin;
			var ty = centerY * (1F - cos) - centerX * sin;

			CGAffineTransform result;
			result.xx = cos;
			result.xy = -sin;
			result.yx = sin;
			result.yy = cos;
			result.x0 = tx;
			result.y0 = ty;
			return result;
		}
	}

	public static class Vector2Extensions
	{
		public static CGPoint ToCGPoint(this Vector2 vector)
			=> new CGPoint(vector.X, vector.Y);

		public static Vector2 ToVector(this CGPoint point)
			=> new Vector2((float)point.X, (float)point.Y);
	}

	public static class UGColorExtensions
	{
		public static nfloat[] ToColorComponents(this UGColor color)
			=> new[] { (nfloat)color.R / 255, (nfloat)color.G / 255, (nfloat)color.B / 255, (nfloat)color.A / 255 };

		public static CGColor ToCGColor(this UGColor color)
		{
			using (var colorSpace = CGColorSpace.CreateSrgb())
			{
				return new CGColor(colorSpace, color.ToColorComponents());
			}
		}

#if __MACOS__
		public static NSColor ToNSColor(this UGColor color)
			=> NSColor.FromRgba(color.R, color.G, color.B, color.A);
#else
		public static UIColor ToUIColor(this UGColor color)
			=> UIColor.FromRGBA(color.R, color.G, color.B, color.A);
#endif

		public static UGColor ToUGColor(this CGColor color)
		{
			return new UGColor(
				(byte)(255F * color.Components[3] + .5F),
				(byte)(255F * color.Components[0] + .5F),
				(byte)(255F * color.Components[1] + .5F),
				(byte)(255F * color.Components[2] + .5F));
		}
	}

	public static class UGSizeExtensions
	{
		public static CGSize ToCGSize(this UGSize size)
			=> new CGSize(size.Width, size.Height);

		public static UGSize ToUGSize(this CGSize size)
			=> new UGSize((float)size.Width, (float)size.Height);
	}

	public static class UGRectExtensions
	{
		public static CGRect ToCGRect(this UGRect rect)
			=> new CGRect(rect.X, rect.Y, rect.Width, rect.Height);

		public static UGRect ToUGRect(this CGRect rect)
			=> new UGRect((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
	}

	internal static class UGStrokeStyleExtensions
	{
		public static CGLineCap ToCGLineCap(this UGCapStyle capStyle)
		{
			switch (capStyle)
			{
				case UGCapStyle.Flat:
					return CGLineCap.Butt;

				case UGCapStyle.Square:
					return CGLineCap.Square;

				case UGCapStyle.Round:
					return CGLineCap.Round;

				case UGCapStyle.Triangle:
				default:
					throw new NotSupportedException();
			}
		}

		public static CGLineJoin ToCGLineJoin(this UGLineJoin lineJoin)
		{
			switch (lineJoin)
			{
				case UGLineJoin.Miter:
					return CGLineJoin.Miter;

				case UGLineJoin.Bevel:
					return CGLineJoin.Bevel;

				case UGLineJoin.Round:
					return CGLineJoin.Round;

				case UGLineJoin.MiterOrBevel:
				default:
					throw new NotSupportedException();
			}
		}

		public static void SetStrokeStyle(this CGContext context, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			context.SetLineCap(strokeStyle.LineCap.ToCGLineCap());
			context.SetLineJoin(strokeStyle.LineJoin.ToCGLineJoin());
			context.SetMiterLimit(strokeStyle.MiterLimit);
			if (strokeStyle.DashStyle.Value != null)
			{
				context.SetLineDash(strokeStyle.DashOffset, strokeStyle.DashStyle.Value.Select(v => (nfloat)v * strokeWidth).ToArray());
			}
		}
	}
}
