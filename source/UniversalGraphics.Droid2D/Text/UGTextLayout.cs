using System;
using System.Diagnostics;
using Android.Graphics;
using Android.Text;

namespace UniversalGraphics.Droid2D
{
	public sealed class UGTextLayout : IUGTextLayout
	{
		object IUGObject.Native => Native;
		public StaticLayout Native
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(nameof(Native));
				}

				return GetLayout();
			}
		}

		private readonly string _textString;
		private readonly UGTextFormat _textFormat;
		private readonly UGSize _requestedSize;

		private bool _disposed = false;
		private StaticLayout _native;
		private UGTextAntialiasing _textAntialiasing;

		public UGTextLayout(IUGContext context, string textString, IUGTextFormat textFormat)
			: this(context, textString, textFormat, UGSize.MaxValue)
		{ }

		public UGTextLayout(IUGContext context, string textString, IUGTextFormat textFormat, UGSize requestedSize)
		{
			_textString = textString;
			_textFormat = (UGTextFormat)textFormat;
			_requestedSize = requestedSize;
		}

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			if (_native != null)
			{
				_native.Dispose();
				_native = null;
			}
			GC.SuppressFinalize(this);
		}

		internal void Draw(Canvas canvas, UGColor color)
		{
			_textFormat.Native.Color = color.ToAGColor();

			var layout = GetLayout();
			var bounds = LayoutBounds;
			canvas.Translate(0F, bounds.Y);
			layout.Draw(canvas);
		}

		private void InvalidateLayout()
		{
			if (_native != null)
			{
				_native.Dispose();
				_native = null;
			}
			InvalidateBounds();
		}

		private void InvalidateBounds()
		{
			_LayoutBounds = UGRect.Zero;
		}

		private StaticLayout GetLayout()
		{
			if (_native == null)
			{
				_native = new StaticLayout(
					_textString, 0, _textString.Length,
					_textFormat.Native,
					(int)_requestedSize.Width,
					_HorizontalAlignment.ToATLayoutAlignment(),
					1F,
					0F,
					false);
			}
			return _native;
		}

		internal void SetTextAntialiasing(UGTextAntialiasing textAntialiasing)
		{
			if (_textAntialiasing != textAntialiasing)
			{
				_textAntialiasing = textAntialiasing;
				InvalidateBounds();
			}
		}

		private void ComputeBounds()
		{
			var layout = GetLayout();
			_LayoutBounds = new UGRect(
				0F,
				0F,
				layout.Width,
				layout.Height);
			if (VerticalAlignment != UGVerticalAlignment.Top)
			{
				if (VerticalAlignment == UGVerticalAlignment.Bottom)
				{
					_LayoutBounds.Y = Math.Max(0F, _requestedSize.Height - _LayoutBounds.Height);
				}
				else if (VerticalAlignment == UGVerticalAlignment.Center)
				{
					_LayoutBounds.Y = Math.Max(0F, (_requestedSize.Height - _LayoutBounds.Height) / 2F);
				}
			}
		}

		public UGHorizontalAlignment HorizontalAlignment
		{
			get => _HorizontalAlignment;
			set
			{
				if (_HorizontalAlignment != value)
				{
					_HorizontalAlignment = value;
					InvalidateLayout();
				}
			}
		}
		private UGHorizontalAlignment _HorizontalAlignment;

		public UGRect LayoutBounds
		{
			get
			{
				if (_LayoutBounds.IsEmpty)
				{
					var paint = _textFormat.Native;
					switch (_textAntialiasing)
					{
						case UGTextAntialiasing.Aliased:
							paint.AntiAlias = false;
							paint.LinearText = false;
							paint.SubpixelText = false;
							break;

						case UGTextAntialiasing.Antialiased:
							paint.AntiAlias = true;
							paint.LinearText = true;
							paint.SubpixelText = false;
							break;

						case UGTextAntialiasing.Auto:
						case UGTextAntialiasing.SubpixelAntialiased:
						default:
							paint.AntiAlias = true;
							paint.LinearText = true;
							paint.SubpixelText = true;
							break;
					}

					ComputeBounds();
				}
				return _LayoutBounds;
			}
		}
		private UGRect _LayoutBounds = UGRect.Zero;

		public UGVerticalAlignment VerticalAlignment
		{
			get => _VerticalAlignment;
			set
			{
				if (_VerticalAlignment != value)
				{
					_VerticalAlignment = value;
					InvalidateBounds();
				}
			}
		}
		private UGVerticalAlignment _VerticalAlignment;
	}
}
