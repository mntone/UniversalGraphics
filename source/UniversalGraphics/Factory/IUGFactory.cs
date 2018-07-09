namespace UniversalGraphics
{
	public interface IUGFactory : IUGBrushFactory
    {
		IUGPath CreatePath();

		IUGTextFormat CreateTextFormat();
		IUGTextLayout CreateTextLayout(string textString, IUGTextFormat textFormat);
		IUGTextLayout CreateTextLayout(string textString, IUGTextFormat textFormat, UGSize requestedSize);
    }
}
