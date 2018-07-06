using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;

namespace UniversalGraphics.GdiPlus
{
	public sealed class UGLinearGradientBrush : IUGLinearGradientBrush, IUGBrushInternal
	{
		object IUGObject.Native => Native;
		Brush IUGBrushInternal.Native => Native;
		public LinearGradientBrush Native
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(nameof(Native));
				}

				return _native;
			}
		}

		private readonly LinearGradientBrush _native;

		private bool _disposed = false;

		public UGLinearGradientBrush(IUGContext context, UGColor startColor, UGColor endColor, float angle)
			: this(context, Vector2.Zero, UGLinearGradientHelper.EndPointFromAngle(angle), startColor, endColor, UGEdgeBehavior.Clamp)
		{ }

		public UGLinearGradientBrush(IUGContext context, UGColor startColor, UGColor endColor, float angle, UGEdgeBehavior edgeBehavior)
			: this(context, Vector2.Zero, UGLinearGradientHelper.EndPointFromAngle(angle), startColor, endColor, edgeBehavior)
		{ }

		public UGLinearGradientBrush(IUGContext context, Vector2 startPoint, Vector2 endPoint, UGColor startColor, UGColor endColor)
			: this(context, startPoint, endPoint, new[] { new UGGradientStop(startColor, 0F), new UGGradientStop(endColor, 1F) }, UGEdgeBehavior.Clamp)
		{ }

		public UGLinearGradientBrush(IUGContext context, Vector2 startPoint, Vector2 endPoint, UGColor startColor, UGColor endColor, UGEdgeBehavior edgeBehavior)
			: this(context, startPoint, endPoint, new[] { new UGGradientStop(startColor, 0F), new UGGradientStop(endColor, 1F) }, edgeBehavior)
		{ }

		public UGLinearGradientBrush(IUGContext context, Vector2 startPoint, Vector2 endPoint, IEnumerable<UGGradientStop> gradientStops)
			: this(context, startPoint, endPoint, gradientStops, UGEdgeBehavior.Clamp)
		{ }

		public UGLinearGradientBrush(IUGContext context, Vector2 startPoint, Vector2 endPoint, IEnumerable<UGGradientStop> gradientStops, UGEdgeBehavior edgeBehavior)
		{
			StartPoint = startPoint;
			EndPoint = endPoint;

			var ugStops = gradientStops.ToArray();
			if (ugStops.Length < 2)
			{
				throw new ArgumentException(nameof(gradientStops));
			}

			var diff = endPoint - startPoint;
			var angle = MathHelper.RadiansToDegrees((float)Math.Atan2(diff.Y, diff.X));
			var native = new LinearGradientBrush(
				new RectangleF(0F, 0F, 1F, 1F),
				Color.Transparent, Color.Transparent,
				angle,
				false);
			native.InterpolationColors = ugStops.ToGDIColorBlend();
			_native = native;
		}

		internal UGLinearGradientBrush(LinearGradientBrush native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		public Vector2 StartPoint { get; }
		public Vector2 EndPoint { get; }
		public UGEdgeBehavior EdgeBehavior => Native.WrapMode.ToUGEdgeBehavior();
		public UGGradientStop[] Stops => Native.InterpolationColors.ToUGGradientStop();

		public static implicit operator LinearGradientBrush(UGLinearGradientBrush d)
			=> d.Native;
	}
}
