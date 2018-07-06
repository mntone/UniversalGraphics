using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace UniversalGraphics.Droid2D
{
	public sealed class UGRadialGradientBrush : IUGRadialGradientBrush, IUGBrushInternal
	{
		object IUGObject.Native => Native;
		Shader IUGBrushInternal.Native => Native;
		public RadialGradient Native
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

		private readonly RadialGradient _native;

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
			Center = center;
			Radius = radius;
			EdgeBehavior = edgeBehavior;
			Stops = gradientStops.ToArray();

			if (Stops.Length < 2)
			{
				throw new ArgumentException(nameof(gradientStops));
			}

			var colors = gradientStops.Select(s => s.Color.ColorAsInt).ToArray();
			var offsets = gradientStops.Select(s => s.Offset).ToArray();

			_native = new RadialGradient(
				Center.X, Center.Y,
				Radius,
				colors,
				offsets,
				edgeBehavior.ToAGShaderTileMode());
		}

		internal UGRadialGradientBrush(RadialGradient native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		public Vector2 Center { get; }
		public float Radius { get; }
		public UGEdgeBehavior EdgeBehavior { get; }
		public UGGradientStop[] Stops { get; }

		public static implicit operator RadialGradient(UGRadialGradientBrush d)
			=> d.Native;
	}
}
