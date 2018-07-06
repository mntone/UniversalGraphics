using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;

namespace UniversalGraphics.GdiPlus
{
	public sealed class UGRadialGradientBrush : IUGRadialGradientBrush, IUGBrushInternal
	{
		object IUGObject.Native => Native;
		Brush IUGBrushInternal.Native => Native;
		public PathGradientBrush Native
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

		private readonly PathGradientBrush _native;

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
			Radius = radius;

			var ugStops = gradientStops.Reverse().ToArray();
			if (ugStops.Length < 2)
			{
				throw new ArgumentException(nameof(gradientStops));
			}
			for (var i = 0; i < ugStops.Length; ++i)
			{
				ugStops[i].Offset = 1F - ugStops[i].Offset;
			}
			
			var size = radius * 4F;
			var offset = .5F * size;
			using (var path = new GraphicsPath())
			{
				path.AddEllipse(center.X - offset, center.Y - offset, size, size);

				var native = new PathGradientBrush(path);
				native.CenterPoint = center.ToGDIPointF();
				native.WrapMode = edgeBehavior.ToGDIWrapMode();
				native.InterpolationColors = ugStops.ToGDIColorBlendWithEdgeBehavior(edgeBehavior);
				_native = native;
			}
		}

		internal UGRadialGradientBrush(PathGradientBrush native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		public Vector2 Center => Native.CenterPoint.ToVector();
		public float Radius { get; }
		public UGEdgeBehavior EdgeBehavior => Native.WrapMode.ToUGEdgeBehavior();
		public UGGradientStop[] Stops => Native.InterpolationColors.ToUGGradientStop();

		public static implicit operator PathGradientBrush(UGRadialGradientBrush d)
			=> d.Native;
	}
}
