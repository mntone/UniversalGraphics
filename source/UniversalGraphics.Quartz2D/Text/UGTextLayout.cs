using CoreGraphics;
using System;
using System.Diagnostics;
using Foundation;

#if __MACOS__
using AppKit;
#else
using UIKit;
#endif

namespace UniversalGraphics.Quartz2D
{
	public sealed class UGTextLayout : IUGTextLayout
	{
		object IUGObject.Native => Native;
		public NSString Native
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

		private readonly NSString _native;
		private readonly UGTextFormat _textFormat;
		private readonly UGSize _requestedSize;

		private bool _disposed = false;

		public UGTextLayout(IUGContext context, string textString, IUGTextFormat textFormat)
			: this(context, textString, textFormat, UGSize.MaxValue)
		{ }

		public UGTextLayout(IUGContext context, string textString, IUGTextFormat textFormat, UGSize requestedSize)
		{
			_native = new NSString(textString);
			_textFormat = (UGTextFormat)textFormat;
			_requestedSize = requestedSize;
		}

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		internal void Draw(CGContext context, float x, float y, UGColor color)
		{
#if __MACOS__
			NSStringAttributes attributes = null;
			using (var nsColor = color.ToNSColor())
			{
				attributes = new NSStringAttributes()
				{
					Font = _textFormat.Native,
					ForegroundColor = nsColor,
				};
			}
#else
			UIStringAttributes attributes = null;
			using (var uiColor = color.ToUIColor())
			{
				attributes = new UIStringAttributes()
				{
					Font = _textFormat.Native,
					ForegroundColor = uiColor,
				};
			}
#endif

			var bounds = LayoutBounds;
			var rect = new CGRect(
				x + bounds.X,
				y + bounds.Y,
				bounds.Width,
				bounds.Height);
			var scaleMatrix = CGAffineTransformHelper.CreateScale(
				1F,
				-1F,
				(float)rect.X,
				(float)rect.Y);
			try
			{
				context.SaveState();
				context.ConcatCTM(scaleMatrix);
				context.TranslateCTM(0, -bounds.Height);
#if __MACOS__
				_native.DrawInRect(rect, attributes);
#else
				_native.DrawString(rect, attributes);
#endif
			}
			finally { context.RestoreState(); }
		}

		private void InvalidateBounds()
		{
			_LayoutBounds = UGRect.Zero;
		}

		private void ComputeBounds()
		{
#if __MACOS__
			var attributes = new NSStringAttributes()
			{
				Font = _textFormat.Native,
			};
			
			var size = _native.StringSize(attributes);
#else
			var attributes = new UIStringAttributes()
			{
				Font = _textFormat.Native,
			};

			var size = _native.GetSizeUsingAttributes(attributes);
#endif
			_LayoutBounds = new UGRect(
				0F,
				0F,
				(float)size.Width,
				(float)size.Height);
			if (HorizontalAlignment != UGHorizontalAlignment.Left)
			{
				if (HorizontalAlignment == UGHorizontalAlignment.Right)
				{
					_LayoutBounds.X = Math.Max(0F, _requestedSize.Width - _LayoutBounds.Width);
				}
				else if (HorizontalAlignment == UGHorizontalAlignment.Center)
				{
					_LayoutBounds.X = Math.Max(0F, (_requestedSize.Width - _LayoutBounds.Width) / 2F);
				}
			}
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
					InvalidateBounds();
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
