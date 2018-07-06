using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace UniversalGraphics.Droid2D
{
	public sealed class UGLinearGradientBrush : IUGLinearGradientBrush, IUGBrushInternal
	{
		object IUGObject.Native => Native;
		Shader IUGBrushInternal.Native => Native;
		public LinearGradient Native
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

		private readonly LinearGradient _native;

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

			var colors = gradientStops.Select(s => s.Color.ColorAsInt).ToArray();
			var offsets = gradientStops.Select(s => s.Offset).ToArray();

			_native = new LinearGradient(
				StartPoint.X, StartPoint.Y,
				EndPoint.X, EndPoint.Y,
				colors,
				offsets,
				edgeBehavior.ToAGShaderTileMode());
		}

		internal UGLinearGradientBrush(LinearGradient native)
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
		public UGEdgeBehavior EdgeBehavior { get; }
		public UGGradientStop[] Stops { get; }

		public static implicit operator LinearGradient(UGLinearGradientBrush d)
			=> d.Native;
	}
}
