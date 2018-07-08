using System;
using System.Diagnostics;

#if __MACOS__
using AppKit;
#else
using UIKit;
#endif

namespace UniversalGraphics.Quartz2D
{
#if __MACOS__
	using PlatformFont = NSFont;
#else
	using PlatformFont = UIFont;
#endif

	public sealed class UGTextFormat : IUGTextFormat
	{
		object IUGObject.Native => Native;
		public PlatformFont Native
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(nameof(Native));
				}

				return GetFont();
			}
		}

		private bool _disposed = false;
		private PlatformFont _native;

		public UGTextFormat() { }

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		private void InvalidateFont()
		{
			if (_native != null)
			{
				_native.Dispose();
				_native = null;
			}
		}

		private PlatformFont GetFont()
		{
			if (_native == null)
			{
#if __MACOS__
				_native = !string.IsNullOrEmpty(FontFamily)
					? NSFont.FromFontName(FontFamily, FontSize)
					: NSFont.SystemFontOfSize(FontSize);
#else
				_native = !string.IsNullOrEmpty(FontFamily)
					? UIFont.FromName(FontFamily, FontSize)
					: UIFont.SystemFontOfSize(FontSize);
#endif
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
					InvalidateFont();
				}
			}
		}
		private string _FontFamily;

		public float FontSize
		{
			get => _FontSize;
			set
			{
				if (_FontSize != value)
				{
					_FontSize = value;
					InvalidateFont();
				}
			}
		}
		private float _FontSize;
	}
}
