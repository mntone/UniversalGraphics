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
				using (var descriptor = !string.IsNullOrEmpty(FontFamily)
					? NSFontDescriptor.FromNameSize(FontFamily, FontSize)
					: new NSFontDescriptor())
				{
					if (IsItalic)
					{
						using (var descriptor2 = descriptor.FontDescriptorWithSymbolicTraits(NSFontSymbolicTraits.ItalicTrait))
						{
							_native = NSFont.FromDescription(descriptor2, FontSize);
						}
					}
					else
					{
						_native = NSFont.FromDescription(descriptor, FontSize);
					}
				}
#else
				using (var descriptor = !string.IsNullOrEmpty(FontFamily)
					? UIFontDescriptor.FromName(FontFamily, FontSize)
					: new UIFontDescriptor())
				{
					if (IsItalic)
					{
						using (var descriptor2 = descriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Italic))
						{
							_native = UIFont.FromDescriptor(descriptor2, FontSize);
						}
					}
					else
					{
						_native = UIFont.FromDescriptor(descriptor, FontSize);
					}
				}
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

		public bool IsItalic
		{
			get => _IsItalic;
			set
			{
				if (_IsItalic != value)
				{
					_IsItalic = value;
					InvalidateFont();
				}
			}
		}
		private bool _IsItalic;
	}
}
