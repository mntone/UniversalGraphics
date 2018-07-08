using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace UniversalGraphics.Wpf
{
	public sealed class UGTextLayout : IUGTextLayout
	{
		object IUGObject.Native => Native;
		public GlyphRun Native
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(nameof(Native));
				}

				return GetGlyphRun();
			}
		}

		private readonly string _textString;
		private readonly UGTextFormat _textFormat;
		private readonly UGSize _requestedSize;

		private bool _disposed = false;
		private GlyphRun _native;

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
			_native = null;
			GC.SuppressFinalize(this);
		}

		private void InvalidateFormattedText()
		{
			_LayoutBounds = UGRect.Zero;
			_native = null;
		}

		private GlyphRun GetGlyphRun()
		{
			if (_native == null)
			{
				var glyphTypeface = _textFormat.Native;
				var fontSize = _textFormat.FontSize;
				var length = _textString.Length;
				var glyphIndices = new ushort[length];
				var advanceWidths = new double[length];

				var totalWidth = 0.0;
				var height = 0.0;
				for (var i = 0; i < length; ++i)
				{
					glyphTypeface.CharacterToGlyphMap.TryGetValue(_textString[i], out var glyphIndex);
					glyphIndices[i] = glyphIndex;

					var width = fontSize * glyphTypeface.AdvanceWidths[glyphIndex];
					advanceWidths[i] = width;
					totalWidth += width;

					height = Math.Max(height, glyphTypeface.AdvanceHeights[glyphIndex]);
				}

				_native = new GlyphRun(
					glyphTypeface,
					0,
					false,
					fontSize,
					glyphIndices,
					new Point(0, Math.Round(fontSize * glyphTypeface.Baseline)),
					advanceWidths,
					null, null, null, null, null, null);

				_LayoutBounds = _native.ComputeAlignmentBox().ToUGRect();
				ComputeBounds();
			}
			return _native;
		}

		private void ComputeBounds()
		{
			_LayoutBounds = _native.ComputeAlignmentBox().ToUGRect();
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
			else
			{
				_LayoutBounds.Y = 0F;
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
					InvalidateFormattedText();
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
					var glyphRun = GetGlyphRun();
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
					InvalidateFormattedText();
				}
			}
		}
		private UGVerticalAlignment _VerticalAlignment;
	}
}
