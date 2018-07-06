using System.Numerics;

namespace UniversalGraphics
{
	public interface IUGBrush : IUGObject
	{
	}

	public interface IUGSolidColorBrush : IUGBrush
	{
		UGColor Color { get; }
	}

	public interface IUGLinearGradientBrush : IUGBrush
	{
		Vector2 StartPoint { get; }
		Vector2 EndPoint { get; }
		UGEdgeBehavior EdgeBehavior { get; }
		UGGradientStop[] Stops { get; }
	}

	public interface IUGRadialGradientBrush : IUGBrush
	{
		Vector2 Center { get; }
		float Radius { get; }
		UGEdgeBehavior EdgeBehavior { get; }
		UGGradientStop[] Stops { get; }
	}
}
