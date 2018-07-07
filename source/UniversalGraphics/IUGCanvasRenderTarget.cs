namespace UniversalGraphics
{
	public interface IUGCanvasRenderTarget : IUGCanvasImage
	{
		float Scale { get; }

		IUGContext CreateDrawingSession();
	}
}
