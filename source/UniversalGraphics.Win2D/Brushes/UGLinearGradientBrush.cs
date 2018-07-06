using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace UniversalGraphics.Win2D
{
	public sealed class UGLinearGradientBrush : IUGLinearGradientBrush, IUGBrushInternal
	{
		object IUGObject.Native => Native;
		ICanvasBrush IUGBrushInternal.Native => Native;
		public CanvasLinearGradientBrush Native
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

		private readonly CanvasLinearGradientBrush _native;

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
			var count = gradientStops.Count();
			if (count < 2)
			{
				throw new ArgumentException(nameof(gradientStops));
			}

			var device = ((UGContext)context).Device;
			var winrtStops = gradientStops.ToWinRTGradientStops();
			var native = new CanvasLinearGradientBrush(device, winrtStops, edgeBehavior.ToWinRTEdgeBehavior(), CanvasAlphaMode.Premultiplied);
			native.StartPoint = startPoint;
			native.EndPoint = endPoint;
			_native = native;
		}

		internal UGLinearGradientBrush(CanvasLinearGradientBrush native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		public Vector2 StartPoint => Native.StartPoint;
		public Vector2 EndPoint => Native.EndPoint;
		public UGEdgeBehavior EdgeBehavior => Native.EdgeBehavior.ToUGEdgeBehavior();
		public UGGradientStop[] Stops => Native.Stops.ToUGGradientStop();

		public static implicit operator CanvasLinearGradientBrush(UGLinearGradientBrush d)
			=> d.Native;
	}
}
