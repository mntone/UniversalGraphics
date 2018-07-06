using System;
using System.Numerics;

namespace UniversalGraphics.Test.Delegates
{
	public sealed class TransformCanvasViewDelegate : IUGCanvasViewDelegate
	{
		private const int SPLIT_X = 4;
		private const int SPLIT_Y = 4;
		private const int SPLIT_LENGTH = SPLIT_X * SPLIT_Y;
		private const float MARGIN = 10F;

		private readonly UGColor WHITE = new UGColor(255, 255, 255);
		private readonly UGColor BLACK = new UGColor(0, 0, 0);
		private readonly UGColor BLUE = new UGColor(0, 0, 255);
		private readonly UGStrokeStyle STYLE = new UGStrokeStyle() { DashStyle = new UGDashStyle(UGDashStyle.Type.Dash) };

		public void OnDraw(IUGContext context)
		{
			context.ClearColor(WHITE);

			var translate = new Vector2();
			var center = new Vector2() { X = context.CanvasSize.Width / SPLIT_X / 2F, Y = context.CanvasSize.Height / SPLIT_Y / 2F };
			var size = new UGSize(context.CanvasSize.Width / SPLIT_X, context.CanvasSize.Height / SPLIT_Y);
			for (var y = 0; y < SPLIT_Y; ++y, translate.Y += size.Height)
			{
				translate.X = 0F;
				for (var x = 0; x < SPLIT_X; ++x, translate.X += size.Width)
				{
					var d = SPLIT_Y * y + x;
					var color = UGColor.FromHSL((float)d / SPLIT_LENGTH, 1F, .5F);
					color.A = 128;
					using (context.CreateLayer())
					{
						context.Translate(translate);
						using (context.CreateLayer())
						{
							switch (d)
							{
								case 1:
									context.ScaleX(.5F);
									break;

								case 2:
									context.ScaleY(.5F);
									break;

								case 3:
									context.Scale(.25F, .5F);
									break;
									
								case 4:
									context.Rotate(25F);
									break;

								case 5:
									context.Rotate(45F);
									break;

								case 6:
									context.SkewX(20F);
									break;

								case 7:
									context.SkewY(40F);
									break;

								case 8:
									context.Skew(5F, 10F);
									break;

								case 9:
									context.TranslateX(10F);
									break;

								case 10:
									context.TranslateY(10F);
									break;

								case 11:
									context.Translate(5F, 10F);
									break;

								case 12:
									context.Rotate(20F, center);
									break;

								case 13:
									context.SkewX(20F, center);
									break;

								case 14:
									context.Scale(.25F, .5F, center);
									break;

								case 15:
									context.Transform(new Matrix3x2()
									{
										M11 = .25F,
										M12 = (float)Math.Tan(5.0 * Math.PI / 180.0),
										M21 = (float)Math.Tan(10.0 * Math.PI / 180.0),
										M22 = .75F,
										M31 = 10F,
										M32 = 20F,
									});
									break;
							}
							FillRectangle(context, size, color);
						}
						DrawRectangle(context, size);
					}
				}
			}
		}

		private void FillRectangle(IUGContext context, UGSize size, UGColor color)
		{
			context.FillRectangle(MARGIN, MARGIN, size.Width - 2 * MARGIN, size.Height - 2 * MARGIN, color);
		}

		private void DrawRectangle(IUGContext context, UGSize size)
		{
			context.DrawRectangle(MARGIN, MARGIN, size.Width - 2 * MARGIN, size.Height - 2 * MARGIN, BLACK, 2F, STYLE);
		}
	}
}
