using System.Numerics;

namespace UniversalGraphics.Test.Delegates
{
	public sealed class BorderCanvasViewDelegate : IUGCanvasViewDelegate
	{
		private const float MARGIN = 10F;
		private const float STROKE_WIDTH = 4F;

		private readonly UGColor WHITE = new UGColor(255, 255, 255);
		private readonly UGColor BLUE = new UGColor(0, 0, 255);
		private readonly UGCapStyle[] caps = new[] { UGCapStyle.Flat, UGCapStyle.Square, UGCapStyle.Round };
		private readonly UGDashStyle.Type[] styles = new[] { UGDashStyle.Type.Solid, UGDashStyle.Type.Dash, UGDashStyle.Type.Dot, UGDashStyle.Type.DashDot, UGDashStyle.Type.DashDotDot };
		private readonly int count;

		public BorderCanvasViewDelegate()
		{
			count = caps.Length * styles.Length;
		}

		public void OnDraw(IUGContext context)
		{
			context.ClearColor(WHITE);

			var offset = (context.CanvasSize.Height - 2F * MARGIN) / (count - 1);
			var start = new Vector2() { X = MARGIN, Y = MARGIN };
			var end = new Vector2() { X = context.CanvasSize.Width - MARGIN, Y = MARGIN };
			var style = new UGStrokeStyle();
			foreach (var cap in caps)
			{
				style.LineCap = cap;

				foreach (var dashStyle in styles)
				{
					style.DashStyle = new UGDashStyle(dashStyle);
					context.DrawLine(start, end, BLUE, STROKE_WIDTH, style);
					start.Y += offset;
					end.Y += offset;
				}
			}
		}
	}
}
