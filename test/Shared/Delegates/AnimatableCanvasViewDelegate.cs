using System;
using System.Reactive.Linq;
using System.Threading;

#if WINDOWS_APP || WINDOWS_PHONE_APP
using Microsoft.Graphics.Canvas.Numerics;
#else
using System.Numerics;
#endif

namespace UniversalGraphics.Test.Delegates
{
	public sealed class AnimatableCanvasViewDelegate : UGAnimatableCanvasViewDelegate, IDisposable
	{
		private const float MARGIN = 10F;
		private const float PI_H = (float)(.5F * Math.PI);
		private const float PI2 = (float)(2F * Math.PI);

		private readonly UGColor WHITE = new UGColor(255, 255, 255);
		private readonly UGColor GRAY = new UGColor(127, 127, 127);
		private readonly UGColor BLACK = new UGColor(0, 0, 0);
		private readonly UGColor RED = new UGColor(255, 0, 0);

		private IDisposable _timer = null;

		public override void InitializeResources(IUGContext context)
		{
			var synchronizationContext = SynchronizationContext.Current;
			_timer = Observable.Interval(TimeSpan.FromMilliseconds(500))
				.ObserveOn(synchronizationContext)
				.Subscribe(OnUpdate);
		}

		public void Dispose()
		{
			if (_timer != null)
			{
				_timer.Dispose();
				_timer = null;
			}
		}

		private void OnUpdate(long _) => InvalidateVisual();

		public override void OnDraw(IUGContext context)
		{
			base.OnDraw(context);

			context.ClearColor(WHITE);

			var halfWidth = context.CanvasSize.Width / 2F;
			var halfHeight = context.CanvasSize.Height / 2F;
			var radius = Math.Min(halfWidth, halfHeight);
			var center = new Vector2() { X = halfWidth, Y = halfHeight };
			context.DrawCircle(center, radius, BLACK, 4F);

			var current = DateTime.Now;
			{
				var hourDegrees = PI2 * (current.Hour % 12) / 12F - PI_H;
				var hourLengths = .5F * radius;
				var hourLengthsX = (float)(hourLengths * Math.Cos(hourDegrees));
				var hourLengthsY = (float)(hourLengths * Math.Sin(hourDegrees));
				context.DrawLine(center, new Vector2() { X = center.X + hourLengthsX, Y = center.Y + hourLengthsY }, GRAY, 4F);
			}
			{
				var minDegrees = PI2 * current.Minute / 60F - PI_H;
				var minLengths = .7F * radius;
				var minLengthsX = (float)(minLengths * Math.Cos(minDegrees));
				var minLengthsY = (float)(minLengths * Math.Sin(minDegrees));
				context.DrawLine(center, new Vector2() { X = center.X + minLengthsX, Y = center.Y + minLengthsY }, GRAY, 4F);
			}
			{
				var secDegrees = PI2 * current.Second / 60F - PI_H;
				var secLengths = .9F * radius;
				var secLengthsX = (float)(secLengths * Math.Cos(secDegrees));
				var secLengthsY = (float)(secLengths * Math.Sin(secDegrees));
				context.DrawLine(center, new Vector2() { X = center.X + secLengthsX, Y = center.Y + secLengthsY }, RED, 4F);
			}
		}
	}
}
