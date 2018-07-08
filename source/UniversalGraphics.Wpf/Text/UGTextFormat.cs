using System;
using System.Diagnostics;
using System.Windows.Media;

namespace UniversalGraphics.Wpf
{
	public sealed class UGTextFormat : IUGTextFormat
	{
		object IUGObject.Native => Native;
		public GlyphTypeface Native
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(nameof(Native));
				}

				return GetTypeface();
			}
		}

		private bool _disposed = false;
		private GlyphTypeface _native = null;

		public UGTextFormat() { }

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			GC.SuppressFinalize(this);
		}

		private void InvalidateTypeface() => _native = null;

		private GlyphTypeface GetTypeface()
		{
			if (_native == null)
			{
				var typeface = !string.IsNullOrEmpty(FontFamily)
					? new Typeface(FontFamily)
					: new Typeface("Segoe UI");
				if (!typeface.TryGetGlyphTypeface(out var glyphTypeface))
				{
					throw new NotSupportedException();
				}
				_native = glyphTypeface;
			}
			return _native;
		}

		public string FontFamily
		{
			get => _FontFamily;
			set
			{
				if (_FontFamily != value)
				{
					_FontFamily = value;
					InvalidateTypeface();
				}
			}
		}
		private string _FontFamily;

		public float FontSize { get; set; }
	}
}
