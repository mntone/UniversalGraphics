namespace UniversalGraphics
{
	public interface IUGTextLayout : IUGObject
    {
		UGHorizontalAlignment HorizontalAlignment { get; set; }
		UGRect LayoutBounds { get; }
		UGVerticalAlignment VerticalAlignment { get; set; }
	}
}
