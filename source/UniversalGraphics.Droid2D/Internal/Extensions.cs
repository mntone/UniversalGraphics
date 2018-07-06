using Android.Graphics;
using System;
using System.Linq;

namespace UniversalGraphics.Droid2D
{
	public static class UGColorExtensions
    {
		public static Color ToAGColor(this UGColor color)
			=> new Color(color.ColorAsInt);

		public static UGColor ToUGColor(this Color color)
			=> new UGColor(color.A, color.R, color.G, color.B);
	}

	public static class UGRectExtensions
	{
		public static Rect ToAGRect(this UGRect rect)
			=> new Rect(
				(int)(rect.X + 0.5F),
				(int)(rect.Y + 0.5F),
				(int)(rect.Width + 0.5F),
				(int)(rect.Height + 0.5F));

		public static RectF ToAGRectF(this UGRect rect)
			=> new RectF(rect.X, rect.Y, rect.Width, rect.Height);
	}

	internal static class UGStrokeStyleExtensions
	{
		public static Paint.Cap ToAGPaintCap(this UGCapStyle capStyle)
		{
			switch (capStyle)
			{
				case UGCapStyle.Flat:
					return Paint.Cap.Butt;

				case UGCapStyle.Square:
					return Paint.Cap.Square;

				case UGCapStyle.Round:
					return Paint.Cap.Round;

				case UGCapStyle.Triangle:
				default:
					throw new NotSupportedException();
			}
		}

		public static Paint.Join ToAGPaintJoin(this UGLineJoin lineJoin)
		{
			switch (lineJoin)
			{
				case UGLineJoin.Miter:
					return Paint.Join.Miter;

				case UGLineJoin.Bevel:
					return Paint.Join.Bevel;
					
				case UGLineJoin.Round:
					return Paint.Join.Round;

				case UGLineJoin.MiterOrBevel:
				default:
					throw new NotSupportedException();
			}
		}

		public static void SetStrokeStyle(this Paint paint, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			paint.StrokeCap = strokeStyle.LineCap.ToAGPaintCap();
			paint.StrokeJoin = strokeStyle.LineJoin.ToAGPaintJoin();
			paint.StrokeMiter = strokeStyle.MiterLimit;
			if (strokeStyle.DashStyle.Value != null)
			{
				using (var effect = new DashPathEffect(
					strokeStyle.DashStyle.Value.Select(v => v * strokeWidth).ToArray(),
					strokeStyle.DashOffset))
				{
					paint.SetPathEffect(effect);
				}
			}
		}
	}

	internal static class UGEdgeBehaviorExtensions
	{
		public static Shader.TileMode ToAGShaderTileMode(this UGEdgeBehavior edgeBehavior)
		{
			switch (edgeBehavior)
			{
				case UGEdgeBehavior.Clamp:
					return Shader.TileMode.Clamp;

				case UGEdgeBehavior.Wrap:
					return Shader.TileMode.Repeat;

				case UGEdgeBehavior.Mirror:
					return Shader.TileMode.Mirror;

				default:
					throw new NotSupportedException();
			}
		}
	}
}
