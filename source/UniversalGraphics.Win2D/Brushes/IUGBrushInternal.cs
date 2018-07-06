using Microsoft.Graphics.Canvas.Brushes;

namespace UniversalGraphics.Win2D
{
	internal interface IUGBrushInternal
	{
		ICanvasBrush Native { get; }
	}
}
