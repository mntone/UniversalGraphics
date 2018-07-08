using System.Numerics;

#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
using UniversalGraphics.Win2D;
#elif WINDOWS_WPF
using UniversalGraphics.Wpf;
#elif __ANDROID__
using UniversalGraphics.Droid2D;
#elif __MACOS__ || __IOS__ || __TVOS__ || __WATCHOS__
using UniversalGraphics.Quartz2D;
#else
using UniversalGraphics.GdiPlus;
#endif

namespace UniversalGraphics.Test.Delegates
{
	public sealed class TextCanvasViewDelegate : UGCanvasViewDelegate
	{
		private readonly UGColor WHITE = new UGColor(255, 255, 255);
		private readonly UGColor GRAY = new UGColor(127, 127, 127);
		private readonly UGColor BLACK = new UGColor(0, 0, 0);
		private readonly UGColor BLUE = new UGColor(0, 0, 255);
		private readonly UGStrokeStyle STYLE = new UGStrokeStyle() { DashStyle = new UGDashStyle(UGDashStyle.Type.Dash) };

		private UGTextFormat _textFormat;

		public override void InitializeResources(IUGContext context)
		{
			_textFormat = new UGTextFormat()
			{
				FontSize = 24,
			};
		}

		public override void OnDraw(IUGContext context)
		{
			base.OnDraw(context);

			context.ClearColor(WHITE);

			var halfWidth = context.CanvasSize.Width / 2F;
			var pos = new Vector2(10F, 10F);
			var size = new UGSize(halfWidth - 20F, 200F);

			context.TextAntialiasing = UGTextAntialiasing.Aliased;
			using (var textAntialiasingTextLayout = new UGTextLayout(context, "[Aliased]", _textFormat, size))
			{
				context.DrawTextLayout(textAntialiasingTextLayout, pos, BLACK);
				pos.Y += textAntialiasingTextLayout.LayoutBounds.Height;
			}
			using (var leftTextLayout = new UGTextLayout(context, "Left text.", _textFormat, size))
			{
				leftTextLayout.HorizontalAlignment = UGHorizontalAlignment.Left;
				context.DrawTextLayout(leftTextLayout, pos, BLUE);
				pos.Y += leftTextLayout.LayoutBounds.Height;
			}
			using (var centerTextLayout = new UGTextLayout(context, "Center text.", _textFormat, size))
			{
				centerTextLayout.HorizontalAlignment = UGHorizontalAlignment.Center;
				context.DrawTextLayout(centerTextLayout, pos, BLUE);
				pos.Y += centerTextLayout.LayoutBounds.Height;
			}
			using (var rightTextLayout = new UGTextLayout(context, "Right text.", _textFormat, size))
			{
				rightTextLayout.HorizontalAlignment = UGHorizontalAlignment.Right;
				context.DrawTextLayout(rightTextLayout, pos, BLUE);
				pos.Y += rightTextLayout.LayoutBounds.Height;
			}
			pos.Y += 50F;

			context.TextAntialiasing = UGTextAntialiasing.Antialiased;
			using (var textAntialiasingTextLayout = new UGTextLayout(context, "[Antialiased]", _textFormat, size))
			{
				context.DrawTextLayout(textAntialiasingTextLayout, pos, BLACK);
				pos.Y += textAntialiasingTextLayout.LayoutBounds.Height;
			}
			using (var leftTextLayout = new UGTextLayout(context, "Left text.", _textFormat, size))
			{
				leftTextLayout.HorizontalAlignment = UGHorizontalAlignment.Left;
				context.DrawTextLayout(leftTextLayout, pos, BLUE);
				pos.Y += leftTextLayout.LayoutBounds.Height;
			}
			using (var centerTextLayout = new UGTextLayout(context, "Center text.", _textFormat, size))
			{
				centerTextLayout.HorizontalAlignment = UGHorizontalAlignment.Center;
				context.DrawTextLayout(centerTextLayout, pos, BLUE);
				pos.Y += centerTextLayout.LayoutBounds.Height;
			}
			using (var rightTextLayout = new UGTextLayout(context, "Right text.", _textFormat, size))
			{
				rightTextLayout.HorizontalAlignment = UGHorizontalAlignment.Right;
				context.DrawTextLayout(rightTextLayout, pos, BLUE);
				pos.Y += rightTextLayout.LayoutBounds.Height;
			}
			pos.Y += 50F;

			context.TextAntialiasing = UGTextAntialiasing.SubpixelAntialiased;
			using (var textAntialiasingTextLayout = new UGTextLayout(context, "[SubpixelAntialiased]", _textFormat, size))
			{
				context.DrawTextLayout(textAntialiasingTextLayout, pos, BLACK);
				pos.Y += textAntialiasingTextLayout.LayoutBounds.Height;
			}
			using (var leftTextLayout = new UGTextLayout(context, "Left text.", _textFormat, size))
			{
				leftTextLayout.HorizontalAlignment = UGHorizontalAlignment.Left;
				context.DrawTextLayout(leftTextLayout, pos, BLUE);
				pos.Y += leftTextLayout.LayoutBounds.Height;
			}
			using (var centerTextLayout = new UGTextLayout(context, "Center text.", _textFormat, size))
			{
				centerTextLayout.HorizontalAlignment = UGHorizontalAlignment.Center;
				context.DrawTextLayout(centerTextLayout, pos, BLUE);
				pos.Y += centerTextLayout.LayoutBounds.Height;
			}
			using (var rightTextLayout = new UGTextLayout(context, "Right text.", _textFormat, size))
			{
				rightTextLayout.HorizontalAlignment = UGHorizontalAlignment.Right;
				context.DrawTextLayout(rightTextLayout, pos, BLUE);
				pos.Y += rightTextLayout.LayoutBounds.Height;
			}
			pos.Y += 50F;

			context.FillRectangle(halfWidth, 0F, halfWidth, context.CanvasSize.Height, GRAY);

			var pos2 = new Vector2(10F + halfWidth, 10F);
			var size2 = new UGSize(halfWidth - 20F, 200F);
			context.DrawRectangle(pos2, size2, BLACK, 2F, STYLE);
			using (var centerTextLayout = new UGTextLayout(context, "H = Center\nV = Center\nText Test", _textFormat, size2))
			{
				centerTextLayout.HorizontalAlignment = UGHorizontalAlignment.Center;
				centerTextLayout.VerticalAlignment = UGVerticalAlignment.Center;
				context.DrawTextLayout(centerTextLayout, pos2, BLUE);
				pos2.Y += size2.Height + 10F;
			}
			context.DrawRectangle(pos2, size2, BLACK, 2F, STYLE);
			using (var centerTextLayout = new UGTextLayout(context, "H = Center\nV = Bottom\nText Test", _textFormat, size2))
			{
				centerTextLayout.HorizontalAlignment = UGHorizontalAlignment.Center;
				centerTextLayout.VerticalAlignment = UGVerticalAlignment.Bottom;
				context.DrawTextLayout(centerTextLayout, pos2, BLUE);
				pos2.Y += size2.Height + 10F;
			}
		}
	}
}
