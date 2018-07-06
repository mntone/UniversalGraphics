namespace UniversalGraphics
{
	public interface IUGCanvasRenderTarget : IUGCanvasImage
	{
		IUGContext CreateDrawingSession();
	}
}
