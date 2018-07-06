using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Windows.Media;

namespace UniversalGraphics.Wpf
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
			var count = gradientStops.Count();
			if (count < 2)
			{
				throw new ArgumentException(nameof(gradientStops));
			}

			var wpfStops = gradientStops.ToWPFGradientStopCollection(count);
			var native = new LinearGradientBrush(wpfStops, startPoint.ToWPFPoint(), endPoint.ToWPFPoint());
			native.SpreadMethod = edgeBehavior.ToWPFSpreadMethod();
			native.Freeze();
			_native = native;
		}

		internal UGLinearGradientBrush(LinearGradientBrush native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			GC.SuppressFinalize(this);
		}

		public Vector2 StartPoint => Native.StartPoint.ToVector();
		public Vector2 EndPoint => Native.EndPoint.ToVector();
		public UGEdgeBehavior EdgeBehavior => Native.SpreadMethod.ToUGEdgeBehavior();
		public UGGradientStop[] Stops => Native.GradientStops.ToUGGradientStop();

		public static implicit operator LinearGradientBrush(UGLinearGradientBrush d)
			=> d.Native;
	}
}
