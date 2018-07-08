using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace UniversalGraphics.GdiPlus
{
	public sealed class UGTextLayout : IUGTextLayout
	{
		object IUGObject.Native => Native;
		public TextFormatFlags Native
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(nameof(Native));
				}

				return _native;
			}
		}

		private readonly UGTextFormat _textFormat;
		private readonly string _textString;
		private readonly UGSize _requestedSize;

		private bool _disposed = false;
		private TextFormatFlags _native = TextFormatFlags.PreserveGraphicsTranslateTransform | TextFormatFlags.NoPadding;

		public UGTextLayout(IUGContext context, string textString, IUGTextFormat textFormat)
			: this(context, textString, textFormat, UGSize.MaxValue)
		{ }

		public UGTextLayout(IUGContext context, string textString, IUGTextFormat textFormat, UGSize requestedSize)
		{
			_textFormat = (UGTextFormat)textFormat;
			_textString = textString;
			_requestedSize = requestedSize;
		}

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			GC.SuppressFinalize(this);
		}

		internal void Draw(Graphics graphics, float x, float y, Color color, float scale)
		{
			var rectangle = new Rectangle(
				(int)(scale * x + .5F),
				(int)(scale * y + .5F),
				(int)(scale * _requestedSize.Width + .5F),
				(int)(scale * _requestedSize.Height + .5F));
			TextRenderer.DrawText(graphics, _textString, _textFormat.Native, rectangle, color, _native);
		}

		public UGHorizontalAlignment HorizontalAlignment
		{
			get
			{
				if (_native.HasFlag(TextFormatFlags.Right))
					return UGHorizontalAlignment.Right;
				if (_native.HasFlag(TextFormatFlags.HorizontalCenter))
					return UGHorizontalAlignment.Center;
				return UGHorizontalAlignment.Left;
			}
			set
			{
				switch (value)
				{
					case UGHorizontalAlignment.Left:
						_native &= ~(TextFormatFlags.HorizontalCenter | TextFormatFlags.Right);
						break;
					case UGHorizontalAlignment.Right:
						_native = (_native | TextFormatFlags.Right) &  ~(TextFormatFlags.HorizontalCenter);
						break;
					case UGHorizontalAlignment.Center:
						_native = (_native | TextFormatFlags.HorizontalCenter) & ~(TextFormatFlags.Right);
						break;
					default:
						throw new NotSupportedException();
				}
			}
		}

		public UGRect LayoutBounds
		{
			get
			{
				if (_LayoutBounds.IsEmpty)
				{
					var size = _requestedSize.ToGDISize();
					var result = TextRenderer.MeasureText(_textString, _textFormat.Native, size, _native);
					_LayoutBounds = new UGRect(Vector2.Zero, result.Width, result.Height);
				}
				return _LayoutBounds;
			}
		}
		private UGRect _LayoutBounds = UGRect.Zero;

		public UGVerticalAlignment VerticalAlignment
		{
			get
			{
				if (_native.HasFlag(TextFormatFlags.Bottom))
					return UGVerticalAlignment.Bottom;
				if (_native.HasFlag(TextFormatFlags.VerticalCenter))
					return UGVerticalAlignment.Center;
				return UGVerticalAlignment.Top;
			}
			set
			{
				switch (value)
				{
					case UGVerticalAlignment.Top:
						_native &= ~(TextFormatFlags.VerticalCenter | TextFormatFlags.Bottom);
						break;
					case UGVerticalAlignment.Bottom:
						_native = (_native | TextFormatFlags.Bottom) & ~TextFormatFlags.VerticalCenter;
						break;
					case UGVerticalAlignment.Center:
						_native = (_native | TextFormatFlags.VerticalCenter) & ~TextFormatFlags.Bottom;
						break;
					default:
						throw new NotSupportedException();
				}
			}
		}
	}
}
