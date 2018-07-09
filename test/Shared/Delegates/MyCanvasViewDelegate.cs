using System.Linq;
using System.Numerics;

namespace UniversalGraphics.Test.Delegates
{
	public class MyCanvasViewDelegate : IUGCanvasViewDelegate
	{
		private readonly UGColor RED = new UGColor(255, 0, 0);
		private readonly UGColor BLUE = new UGColor(0, 0, 255);
		private readonly UGStrokeStyle STYLE = new UGStrokeStyle() { DashStyle = new UGDashStyle(UGDashStyle.Type.Dash) };

		public void OnDraw(IUGContext context)
		{
			var rect = new UGRect(10, 20, 100, 200);
			context.FillRoundedRectangle(rect, 40, 80, RED);
			context.DrawRoundedRectangle(rect, 40, 80, BLUE);

			var rect2 = new UGRect();
			rect2.X = 4;
			rect2.Y = 131;
			rect2.Width = 10;
			rect2.Height = 40;
			context.DrawRectangle(rect2, BLUE, 2, STYLE);

			var points = new[]
			{
				new Vector2() { X = 10F, Y = 20F },
				new Vector2() { X = 20F, Y = 10F },
				new Vector2() { X = 30F, Y = 20F },
				new Vector2() { X = 40F, Y = 10F },
				new Vector2() { X = 50F, Y = 20F },
				new Vector2() { X = 60F, Y = 10F },
			};
			context.DrawLines(points, BLUE);

			var points2 = points.Select(p => { p.Y += 30F; return p; }).ToArray();
			context.DrawLines(points2, BLUE, 2F);

			var points3 = points2.Select(p => { p.Y += 30F; return p; }).ToArray();
			context.DrawLines(points3, BLUE, 3F, STYLE);
		}
	}
}
