using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace UniversalGraphics.Wpf
{
	public static class Vector2Extensions
	{
		public static Point ToWPFPoint(this Vector2 vector)
			=> new Point(vector.X, vector.Y);

		public static Vector2 ToVector(this Point point)
			=> new Vector2((float)point.X, (float)point.Y);
	}

	public static class UGColorExtensions
	{
		public static Color ToWPFColor(this UGColor color)
			=> Color.FromArgb(color.A, color.R, color.G, color.B);

		public static UGColor ToUGColor(this Color color)
			=> new UGColor(color.A, color.R, color.G, color.B);
	}

	public static class UGRectExtensions
	{
		public static Rect ToWPFRect(this UGRect rect)
			=> new Rect(rect.X, rect.Y, rect.Width, rect.Height);

		public static UGRect ToUGRect(this Rect rect)
			=> new UGRect((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
	}

	internal static class UGStrokeStyleExtensions
	{
		public static PenLineCap ToWPFLineCap(this UGCapStyle capStyle)
		{
			switch (capStyle)
			{
				case UGCapStyle.Flat:
					return PenLineCap.Flat;

				case UGCapStyle.Square:
					return PenLineCap.Square;

				case UGCapStyle.Round:
					return PenLineCap.Round;

				case UGCapStyle.Triangle:
					return PenLineCap.Triangle;

				default:
					throw new NotSupportedException();
			}
		}

		public static PenLineJoin ToWPFLineJoin(this UGLineJoin lineJoin)
		{
			switch (lineJoin)
			{
				case UGLineJoin.Miter:
					return PenLineJoin.Miter;

				case UGLineJoin.Bevel:
					return PenLineJoin.Bevel;

				case UGLineJoin.Round:
					return PenLineJoin.Round;

				case UGLineJoin.MiterOrBevel:
				default:
					throw new NotSupportedException();
			}
		}

		public static void SetStrokeStyle(this Pen pen, UGStrokeStyle strokeStyle)
		{
			var lineCap = strokeStyle.LineCap.ToWPFLineCap();
			pen.StartLineCap = lineCap;
			pen.EndLineCap = lineCap;
			pen.LineJoin = strokeStyle.LineJoin.ToWPFLineJoin();
			pen.MiterLimit = strokeStyle.MiterLimit;
			if (strokeStyle.DashStyle.Value != null)
			{
				pen.DashStyle = new DashStyle(strokeStyle.DashStyle.Value.Select(v => (double)v), strokeStyle.DashOffset);
				pen.DashCap = lineCap;
			}
		}
	}

	internal static class UGEdgeBehaviorExtensions
	{
		public static GradientSpreadMethod ToWPFSpreadMethod(this UGEdgeBehavior edgeBehavior)
		{
			switch (edgeBehavior)
			{
				case UGEdgeBehavior.Clamp:
					return GradientSpreadMethod.Pad;

				case UGEdgeBehavior.Wrap:
					return GradientSpreadMethod.Repeat;

				case UGEdgeBehavior.Mirror:
					return GradientSpreadMethod.Reflect;

				default:
					throw new NotSupportedException();
			}
		}

		public static UGEdgeBehavior ToUGEdgeBehavior(this GradientSpreadMethod edgeBehavior)
		{
			switch (edgeBehavior)
			{
				case GradientSpreadMethod.Pad:
					return UGEdgeBehavior.Clamp;

				case GradientSpreadMethod.Repeat:
					return UGEdgeBehavior.Wrap;

				case GradientSpreadMethod.Reflect:
					return UGEdgeBehavior.Mirror;

				default:
					throw new NotSupportedException();
			}
		}
	}

	internal static class UGGradientStopExtensions
	{
		public static GradientStopCollection ToWPFGradientStopCollection(this IEnumerable<UGGradientStop> gradientStops, int count)
		{
			var wpfStops = new GradientStopCollection(count);
			foreach (var ugStop in gradientStops)
			{
				var wpfStop = new GradientStop(ugStop.Color.ToWPFColor(), ugStop.Offset);
				wpfStops.Add(wpfStop);
			}
			return wpfStops;
		}

		public static UGGradientStop[] ToUGGradientStop(this GradientStopCollection wpfStops)
		{
			var count = wpfStops.Count;
			var ugStops = new UGGradientStop[count];

			var i = 0;
			foreach (var wpfStop in wpfStops)
			{
				ugStops[i++] = new UGGradientStop(wpfStop.Color.ToUGColor(), (float)wpfStop.Offset);
			}
			return ugStops;
		}
	}
}
