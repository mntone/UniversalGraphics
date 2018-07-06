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
	public class GradientCanvasViewDelegate : UGCanvasViewDelegate, IDisposable
	{
		private UGLinearGradientBrush _linear0, _linear45, _linear90;
		private UGRadialGradientBrush _radialClamp, _radialWrap, _radialMirror;
		private UGRadialGradientBrush _radialClamp2, _radialWrap2, _radialMirror2;

		public override void InitializeResources(IUGContext context)
		{
			_linear0 = new UGLinearGradientBrush(
				context,
				new Vector2(0F, 0F),
				new Vector2(1F, 0F),
				new[] {
					new UGGradientStop(UGColor.FromHSL(0F / 3F, 1F, .5F), .25F),
					new UGGradientStop(UGColor.FromHSL(2F / 3F, 1F, .5F), .75F),
				},
				UGEdgeBehavior.Clamp);
			_linear45 = new UGLinearGradientBrush(
				context,
				new Vector2(0F, 0F),
				new Vector2(1F, 1F),
				new[] {
					new UGGradientStop(UGColor.FromHSL(0F / 3F, 1F, .5F), .25F),
					new UGGradientStop(UGColor.FromHSL(2F / 3F, 1F, .5F), .75F),
				},
				UGEdgeBehavior.Clamp);
			_linear90 = new UGLinearGradientBrush(
				context,
				new Vector2(0F, 0F),
				new Vector2(0F, 1F),
				new[] {
					new UGGradientStop(UGColor.FromHSL(0F / 3F, 1F, .5F), .25F),
					new UGGradientStop(UGColor.FromHSL(2F / 3F, 1F, .5F), .75F),
				},
				UGEdgeBehavior.Clamp);

			_radialClamp = new UGRadialGradientBrush(
				context,
				new[] {
					new UGGradientStop(UGColor.FromHSL(0F / 3F, 1F, .5F), 0F),
					new UGGradientStop(UGColor.FromHSL(1F / 3F, 1F, .5F), .5F),
					new UGGradientStop(UGColor.FromHSL(2F / 3F, 1F, .5F), 1F),
				},
				UGEdgeBehavior.Clamp);
			_radialWrap = new UGRadialGradientBrush(
				context,
				new[] {
					new UGGradientStop(UGColor.FromHSL(0F / 3F, 1F, .5F), 0F),
					new UGGradientStop(UGColor.FromHSL(1F / 3F, 1F, .5F), .5F),
					new UGGradientStop(UGColor.FromHSL(2F / 3F, 1F, .5F), 1F),
				},
				UGEdgeBehavior.Wrap);
			_radialMirror = new UGRadialGradientBrush(
				context,
				new[] {
					new UGGradientStop(UGColor.FromHSL(0F / 3F, 1F, .5F), 0F),
					new UGGradientStop(UGColor.FromHSL(1F / 3F, 1F, .5F), .5F),
					new UGGradientStop(UGColor.FromHSL(2F / 3F, 1F, .5F), 1F),
				},
				UGEdgeBehavior.Mirror);

			_radialClamp2 = new UGRadialGradientBrush(
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
				UGEdgeBehavior.Clamp);
			_radialWrap2 = new UGRadialGradientBrush(
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
			_radialMirror2 = new UGRadialGradientBrush(
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
				UGEdgeBehavior.Mirror);
		}

		public void Dispose()
		{
			_linear0.Dispose();
			_linear45.Dispose();
			_linear90.Dispose();
			_radialClamp.Dispose();
			_radialWrap.Dispose();
			_radialMirror.Dispose();
			_radialClamp2.Dispose();
			_radialWrap2.Dispose();
			_radialMirror2.Dispose();
			GC.SuppressFinalize(this);
		}

		public override void OnDraw(IUGContext context)
		{
			base.OnDraw(context);

			var size = new UGSize((context.CanvasSize.Width - 40F) / 3, (context.CanvasSize.Height - 40F) / 3F);
			context.FillRectangle(0, 0, size.Width, size.Height, _linear0);
			context.FillRectangle(20F + size.Width, 0, size.Width, size.Height, _linear45);
			context.FillRectangle(40F + 2F * size.Width, 0, size.Width, size.Height, _linear90);

			context.FillRectangle(0, 20F + size.Height, size.Width, size.Height, _radialClamp);
			context.FillRectangle(20F + size.Width, 20F + size.Height, size.Width, size.Height, _radialWrap);
			context.FillRectangle(40F + 2F * size.Width, 20F + size.Height, size.Width, size.Height, _radialMirror);

			context.FillRectangle(0, 40F + 2F * size.Height, size.Width, size.Height, _radialClamp2);
			context.FillRectangle(20F + size.Width, 40F + 2F * size.Height, size.Width, size.Height, _radialWrap2);
			context.FillRectangle(40F + 2F * size.Width, 40F + 2F * size.Height, size.Width, size.Height, _radialMirror2);
		}
	}
}
