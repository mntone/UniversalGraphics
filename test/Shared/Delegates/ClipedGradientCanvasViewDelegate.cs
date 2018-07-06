using System;
using System.Numerics;

#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
using UniversalGraphics.Win2D;
#elif WINDOWS_WPF
using UniversalGraphics.Wpf;
#elif __ANDROID__
using UniversalGraphics.Droid2D;
#elif __MACOS__ || __IOS__ || __TVOS__ || __WATCHOS__
using UniversalGraphics.Quartz2D;
#else
using UniversalGraphics.GdiPlus;
#endif

namespace UniversalGraphics.Test.Delegates
{
	public class ClipedGradientCanvasViewDelegate : UGCanvasViewDelegate, IDisposable
	{
		private const int COLUMN = 4;
		private const int ROW = 2;
		private const float SPACE = 20F;

		private UGLinearGradientBrush _linear;
		private UGRadialGradientBrush _radial;

		public override void InitializeResources(IUGContext context)
		{
			_linear = new UGLinearGradientBrush(
				context,
				new Vector2(0F, 0F),
				new Vector2(1F, 1F),
				new[] {
					new UGGradientStop(UGColor.FromHSL(0F / 3F, 1F, .5F), .25F),
					new UGGradientStop(UGColor.FromHSL(2F / 3F, 1F, .5F), .75F),
				},
				UGEdgeBehavior.Clamp);

			_radial = new UGRadialGradientBrush(
				context,
				new Vector2(.2F, .2F),
				.75F,
				new[] {
					new UGGradientStop(UGColor.FromHSL(0F / 3F, 1F, .5F), 0F),
					new UGGradientStop(UGColor.FromHSL(.5F / 3F, 1F, .5F), .25F),
					new UGGradientStop(UGColor.FromHSL(1F / 3F, 1F, .5F), .5F),
					new UGGradientStop(UGColor.FromHSL(1.5F / 3F, 1F, .5F), .75F),
					new UGGradientStop(UGColor.FromHSL(2F / 3F, 1F, .5F), 1F),
				},
				UGEdgeBehavior.Wrap);
		}

		public void Dispose()
		{
			_linear.Dispose();
			_radial.Dispose();
			GC.SuppressFinalize(this);
		}

		public override void OnDraw(IUGContext context)
		{
			base.OnDraw(context);
			
			var size = new UGSize((context.CanvasSize.Width - SPACE * (COLUMN - 1)) / COLUMN, (context.CanvasSize.Height - SPACE * (ROW - 1)) / ROW);
			var pos = new Vector2();
			context.FillCircleInRectangle(pos, Math.Min(size.Width, size.Height), _linear);
			pos.X += SPACE + size.Width;
			context.FillEllipseInRectangle(pos, size.Width, size.Height, _linear);
			pos.X += SPACE + size.Width;
			context.FillRectangle(pos, size.Width, size.Height, _linear);
			pos.X += SPACE + size.Width;
			context.FillRoundedRectangle(pos, size.Width, size.Height, 10F, 20F, _linear);
			pos.X = 0F;

			pos.Y = SPACE + size.Height;
			context.FillCircleInRectangle(pos, Math.Min(size.Width, size.Height), _radial);
			pos.X += SPACE + size.Width;
			context.FillEllipseInRectangle(pos, size.Width, size.Height, _radial);
			pos.X += SPACE + size.Width;
			context.FillRectangle(pos, size.Width, size.Height, _radial);
			pos.X += SPACE + size.Width;
			context.FillRoundedRectangle(pos, size.Width, size.Height, 10F, 20F, _radial);
			pos.X = 0F;
		}
	}
}
