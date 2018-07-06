using System.Numerics;

namespace UniversalGraphics.Test.Delegates
{
	public class ColorPalletCanvasViewDelegate : IUGCanvasViewDelegate
	{
		private const int xStep = 36;
		private const int yStep = 32;

		private readonly UGColor white = new UGColor(255, 255, 255);

		public void OnDraw(IUGContext context)
		{
			context.ClearColor(white);

			var pos = new Vector2();
			var size = new UGSize(context.CanvasSize.Width / xStep, context.CanvasSize.Height / yStep);
			for (var h = 0; h < xStep; ++h)
			{
				pos.Y = 0F;
				for (var l = 0; l < yStep; ++l)
				{
					var color = UGColor.FromHSL((float)h / xStep, 1F, (float)l / yStep);
					context.FillRectangle(pos, size, color);
					pos.Y += size.Height;
				}
				pos.X += size.Width;
			}
		}
	}
}
