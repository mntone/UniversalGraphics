using System.Collections.Generic;
using System.Numerics;

namespace UniversalGraphics
{
	public interface IUGBrushFactory
    {
		IUGSolidColorBrush CreateSolidColorBrush(UGColor color);

		IUGLinearGradientBrush CreateLinearGradientBrush(UGColor startColor, UGColor endColor, float angle);
		IUGLinearGradientBrush CreateLinearGradientBrush(UGColor startColor, UGColor endColor, float angle, UGEdgeBehavior edgeBehavior);
		IUGLinearGradientBrush CreateLinearGradientBrush(Vector2 startPoint, Vector2 endPoint, UGColor startColor, UGColor endColor);
		IUGLinearGradientBrush CreateLinearGradientBrush(Vector2 startPoint, Vector2 endPoint, UGColor startColor, UGColor endColor, UGEdgeBehavior edgeBehavior);
		IUGLinearGradientBrush CreateLinearGradientBrush(Vector2 startPoint, Vector2 endPoint, IEnumerable<UGGradientStop> gradientStops);
		IUGLinearGradientBrush CreateLinearGradientBrush(Vector2 startPoint, Vector2 endPoint, IEnumerable<UGGradientStop> gradientStops, UGEdgeBehavior edgeBehavior);

		IUGRadialGradientBrush CreateRadialGradientBrush(UGColor startColor, UGColor endColor);
		IUGRadialGradientBrush CreateRadialGradientBrush(UGColor startColor, UGColor endColor, UGEdgeBehavior edgeBehavior);
		IUGRadialGradientBrush CreateRadialGradientBrush(IEnumerable<UGGradientStop> gradientStops);
		IUGRadialGradientBrush CreateRadialGradientBrush(IEnumerable<UGGradientStop> gradientStops, UGEdgeBehavior edgeBehavior);
		IUGRadialGradientBrush CreateRadialGradientBrush(Vector2 center, float radius, IEnumerable<UGGradientStop> gradientStops);
		IUGRadialGradientBrush CreateRadialGradientBrush(Vector2 center, float radius, IEnumerable<UGGradientStop> gradientStops, UGEdgeBehavior edgeBehavior);
	}
}
