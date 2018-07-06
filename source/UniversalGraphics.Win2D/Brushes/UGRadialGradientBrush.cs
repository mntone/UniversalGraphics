using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace UniversalGraphics.Win2D
{
	public sealed class UGRadialGradientBrush : IUGRadialGradientBrush, IUGBrushInternal
	{
		object IUGObject.Native => Native;
		ICanvasBrush IUGBrushInternal.Native => Native;
		public CanvasRadialGradientBrush Native
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

		private readonly CanvasRadialGradientBrush _native;

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

			var device = ((UGContext)context).Device;
			var winrtStops = gradientStops.ToWinRTGradientStops();
			var native = new CanvasRadialGradientBrush(device, winrtStops, edgeBehavior.ToWinRTEdgeBehavior(), CanvasAlphaMode.Premultiplied);
			native.Center = center;
			native.RadiusX = radius;
			native.RadiusY = radius;
			_native = native;
		}

		internal UGRadialGradientBrush(CanvasRadialGradientBrush native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		public Vector2 Center => Native.Center;
		public float Radius => Native.RadiusX;
		public UGEdgeBehavior EdgeBehavior => Native.EdgeBehavior.ToUGEdgeBehavior();
		public UGGradientStop[] Stops => Native.Stops.ToUGGradientStop();

		public static implicit operator CanvasRadialGradientBrush(UGRadialGradientBrush d)
			=> d.Native;
	}
}
