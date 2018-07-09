using System;
using System.Collections.Generic;
using System.Numerics;


#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
namespace UniversalGraphics.Win2D
#elif WINDOWS_WPF
namespace UniversalGraphics.Wpf
#elif __ANDROID__
namespace UniversalGraphics.Droid2D
#elif __MACOS__ || __IOS__ || __TVOS__ || __WATCHOS__
namespace UniversalGraphics.Quartz2D
#else
namespace UniversalGraphics.GdiPlus
#endif
{
	internal sealed class UGFactory : IUGFactory
	{
		private readonly UGContext _context;

		public UGFactory(UGContext context) => _context = context;

		public IUGPath CreatePath() => new UGPath(_context);

		public IUGTextFormat CreateTextFormat() => new UGTextFormat();

		public IUGTextLayout CreateTextLayout(string textString, IUGTextFormat textFormat)
			=> new UGTextLayout(_context, textString, textFormat);

		public IUGTextLayout CreateTextLayout(string textString, IUGTextFormat textFormat, UGSize requestedSize)
			=> new UGTextLayout(_context, textString, textFormat, requestedSize);

		public IUGSolidColorBrush CreateSolidColorBrush(UGColor color) => new UGSolidColorBrush(_context, color);

		public IUGLinearGradientBrush CreateLinearGradientBrush(UGColor startColor, UGColor endColor, float angle)
			=> new UGLinearGradientBrush(_context, startColor, endColor, angle);

		public IUGLinearGradientBrush CreateLinearGradientBrush(UGColor startColor, UGColor endColor, float angle, UGEdgeBehavior edgeBehavior)
			=> new UGLinearGradientBrush(_context, startColor, endColor, angle, edgeBehavior);

		public IUGLinearGradientBrush CreateLinearGradientBrush(Vector2 startPoint, Vector2 endPoint, UGColor startColor, UGColor endColor)
			=> new UGLinearGradientBrush(_context, startPoint, endPoint, startColor, endColor);

		public IUGLinearGradientBrush CreateLinearGradientBrush(Vector2 startPoint, Vector2 endPoint, UGColor startColor, UGColor endColor, UGEdgeBehavior edgeBehavior)
			=> new UGLinearGradientBrush(_context, startPoint, endPoint, startColor, endColor, edgeBehavior);

		public IUGLinearGradientBrush CreateLinearGradientBrush(Vector2 startPoint, Vector2 endPoint, IEnumerable<UGGradientStop> gradientStops)
			=> new UGLinearGradientBrush(_context, startPoint, endPoint, gradientStops);

		public IUGLinearGradientBrush CreateLinearGradientBrush(Vector2 startPoint, Vector2 endPoint, IEnumerable<UGGradientStop> gradientStops, UGEdgeBehavior edgeBehavior)
			=> new UGLinearGradientBrush(_context, startPoint, endPoint, gradientStops, edgeBehavior);

		public IUGRadialGradientBrush CreateRadialGradientBrush(UGColor startColor, UGColor endColor)
			=> new UGRadialGradientBrush(_context, startColor, endColor);

		public IUGRadialGradientBrush CreateRadialGradientBrush(UGColor startColor, UGColor endColor, UGEdgeBehavior edgeBehavior)
			=> new UGRadialGradientBrush(_context, startColor, endColor, edgeBehavior);

		public IUGRadialGradientBrush CreateRadialGradientBrush(IEnumerable<UGGradientStop> gradientStops)
			=> new UGRadialGradientBrush(_context, gradientStops);

		public IUGRadialGradientBrush CreateRadialGradientBrush(IEnumerable<UGGradientStop> gradientStops, UGEdgeBehavior edgeBehavior)
			=> new UGRadialGradientBrush(_context, gradientStops, edgeBehavior);

		public IUGRadialGradientBrush CreateRadialGradientBrush(Vector2 center, float radius, IEnumerable<UGGradientStop> gradientStops)
			=> new UGRadialGradientBrush(_context, center, radius, gradientStops);

		public IUGRadialGradientBrush CreateRadialGradientBrush(Vector2 center, float radius, IEnumerable<UGGradientStop> gradientStops, UGEdgeBehavior edgeBehavior)
			=> new UGRadialGradientBrush(_context, center, radius, gradientStops, edgeBehavior);
	}
}
