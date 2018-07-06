namespace UniversalGraphics.Test.Delegates
{
	public class MyCanvasViewDelegate : IUGCanvasViewDelegate
	{
		public void OnDraw(IUGContext context)
		{
			var rect = new UGRect(10, 20, 100, 200);
			var paint = new UGColor();
			paint.A = 255;
			paint.R = 255;
			var paint2 = new UGColor();
			paint2.A = 255;
			paint2.B = 255;
			context.FillRoundedRectangle(rect, 40, 80, paint);
			context.DrawRoundedRectangle(rect, 40, 80, paint2);

			var rect2 = new UGRect();
			rect2.X = 4;
			rect2.Y = 131;
			rect2.Width = 10;
			rect2.Height = 40;
			var style = new UGStrokeStyle();
			style.DashStyle = new UGDashStyle(UGDashStyle.Type.Dash);
			context.DrawRectangle(rect2, paint2, 2, style);
		}
	}
}
