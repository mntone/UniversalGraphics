using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace UniversalGraphics.Quartz2D
{
	public sealed class UGRadialGradientBrush : IUGRadialGradientBrush, IUGBrushInternal
	{
		object IUGObject.Native => Native;
		CGShading IUGBrushInternal.Native => Native;
		public CGShading Native
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

		private readonly CGShading _native;
		private readonly CGFunction _function;

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

			unsafe
			{
				var baseFunction = CGFunctionsHelper.GetCGFunction(EdgeBehavior);
				CGFunction.CGFunctionEvaluate function = (data, outData) => baseFunction(Stops, data, outData);
				var domain = new nfloat[] { 0, edgeBehavior != UGEdgeBehavior.Clamp ? 1.5F : 1 };
				_function = new CGFunction(domain, new nfloat[] { 0, 1, 0, 1, 0, 1, 0, 1 }, function);
			}

			var cgCenter = Center.ToCGPoint();
			var cgRadius = EdgeBehavior != UGEdgeBehavior.Clamp ? 1.5F * Radius : Radius;
			using (var colorSpace = CGColorSpace.CreateSrgb())
			{
				_native = CGShading.CreateRadial(colorSpace, cgCenter, 0F, cgCenter, cgRadius, _function, true, true);
			}
		}

		internal UGRadialGradientBrush(CGShading native)
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

		public static implicit operator CGShading(UGRadialGradientBrush d)
			=> d.Native;
	}
}
