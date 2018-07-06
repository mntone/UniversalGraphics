using System;

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
	public sealed class OffScreenCanvasViewDelegate : UGCanvasViewDelegate, IDisposable
	{
		private readonly UGColor WHITE = new UGColor(255, 255, 255);
		private readonly UGColor BLUE = new UGColor(0, 0, 255);
		
		private UGCanvasRenderTarget _offScreenTarget;

		public void Dispose()
		{
			if (_offScreenTarget != null)
			{
				_offScreenTarget.Dispose();
				_offScreenTarget = null;
			}
			GC.SuppressFinalize(this);
		}

		public override void InitializeResources(IUGContext context)
		{
			var size = new UGSize(64F, 64F);
			_offScreenTarget = new UGCanvasRenderTarget(size, 2F);
			using (var session = _offScreenTarget.CreateDrawingSession())
			{
				session.ClearColor(WHITE);
				session.FillEllipse(32F, 32F, 24F, 32F, BLUE);
			}
		}

		public override void OnDraw(IUGContext context)
		{
			base.OnDraw(context);

			context.DrawImage(_offScreenTarget, 10F, 20F, 256F, 256F);
		}
	}
}
