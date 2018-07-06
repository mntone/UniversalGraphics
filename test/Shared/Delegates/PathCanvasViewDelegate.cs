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
	public class PathCanvasViewDelegate : UGCanvasViewDelegate
	{
		private UGPath _path;

		public override void InitializeResources(IUGContext context)
		{
			_path = new UGPath(context);
			_path.AddEllipse(24, 32, 10, 20);
			_path.MoveTo(100, 100);
			_path.LineTo(200, 200);
			_path.LineTo(200, 100);
			_path.Close();
		}

		public override void OnDraw(IUGContext context)
		{
			base.OnDraw(context);

			context.FillPath(_path, new UGColor(0, 0, 255));
			context.DrawPath(_path, new UGColor(255, 0, 0), 4F);
		}
	}
}
