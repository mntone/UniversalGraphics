using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Windows.Media;

namespace UniversalGraphics.Wpf
{
	public sealed class UGRadialGradientBrush : IUGRadialGradientBrush, IUGBrushInternal
	{
		object IUGObject.Native => Native;
		Brush IUGBrushInternal.Native => Native;
		public RadialGradientBrush Native
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

		private readonly RadialGradientBrush _native;

		private bool _disposed = false;

		public UGRadialGradientBrush(IUGContext context, UGColor startColor, UGColor endColor)
			: this(context, startColor, endColor, UGEdgeBehavior.Clamp)
		{ }

		public UGRadialGradientBrush(IUGContext context, UGColor startColor, UGColor endColor, UGEdgeBehavior edgeBehavior)
			: this(context, new Vector2(.5F, .5F), .5F, new[] { new UGGradientStop(startColor, 0F), new UGGradientStop(endColor, 1F) }, edgeBehavior)
		{ }

		public UGRadialGradientBrush(IUGContext context, IEnumerable<UGGradientStop> gradientStops)
			: this(context, gradientStops, UGEdgeBehavior.Clamp)
		{ }

		public UGRadialGradientBrush(IUGContext context, IEnumerable<UGGradientStop> gradientStops, UGEdgeBehavior edgeBehavior)
			: this(context, new Vector2(.5F, .5F), .5F, gradientStops, edgeBehavior)
		{ }

		public UGRadialGradientBrush(IUGContext context, Vector2 center, float radius, IEnumerable<UGGradientStop> gradientStops)
			: this(context, center, radius, gradientStops, UGEdgeBehavior.Clamp)
		{ }

		public UGRadialGradientBrush(IUGContext context, Vector2 center, float radius, IEnumerable<UGGradientStop> gradientStops, UGEdgeBehavior edgeBehavior)
		{
			var count = gradientStops.Count();
			if (count < 2)
			{
				throw new ArgumentException(nameof(gradientStops));
			}

			var wpfStops = gradientStops.ToWPFGradientStopCollection(count);
			var native = new RadialGradientBrush(wpfStops);
			var wpfCenter = center.ToWPFPoint();
			native.Center = wpfCenter;
			native.GradientOrigin = wpfCenter;
			native.RadiusX = radius;
			native.RadiusY = radius;
			native.SpreadMethod = edgeBehavior.ToWPFSpreadMethod();
			native.Freeze();
			_native = native;
		}

		internal UGRadialGradientBrush(RadialGradientBrush native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			GC.SuppressFinalize(this);
		}

		public Vector2 Center => Native.Center.ToVector();
		public float Radius => (float)Native.RadiusX;
		public UGEdgeBehavior EdgeBehavior => Native.SpreadMethod.ToUGEdgeBehavior();
		public UGGradientStop[] Stops => Native.GradientStops.ToUGGradientStop();

		public static implicit operator RadialGradientBrush(UGRadialGradientBrush d)
			=> d.Native;
	}
}
