using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace UniversalGraphics.Quartz2D
{
	public sealed class UGLinearGradientBrush : IUGLinearGradientBrush, IUGBrushInternal
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
			EdgeBehavior = edgeBehavior;
			Stops = gradientStops.ToArray();

			if (Stops.Length < 2)
			{
				throw new ArgumentException(nameof(gradientStops));
			}

			unsafe
			{
				var baseFunction = CGFunctionsHelper.GetCGFunction(edgeBehavior);
				CGFunction.CGFunctionEvaluate function = (data, outData) => baseFunction(Stops, data, outData);
				_function = new CGFunction(new nfloat[] { 0, 1 }, new nfloat[] { 0, 1, 0, 1, 0, 1, 0, 1 }, function);
			}

			var cgStartPoint = StartPoint.ToCGPoint();
			var cgEndPoint = EndPoint.ToCGPoint();
			using (var colorSpace = CGColorSpace.CreateSrgb())
			{
				_native = CGShading.CreateAxial(colorSpace, cgStartPoint, cgEndPoint, _function, false, false);
			}
		}

		internal UGLinearGradientBrush(CGShading native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			Native.Dispose();
			GC.SuppressFinalize(this);
		}

		public Vector2 StartPoint { get; }
		public Vector2 EndPoint { get; }
		public UGEdgeBehavior EdgeBehavior { get; }
		public UGGradientStop[] Stops { get; }

		public static implicit operator CGShading(UGLinearGradientBrush d)
			=> d.Native;
	}
}
