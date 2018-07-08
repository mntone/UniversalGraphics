using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;

namespace UniversalGraphics.GdiPlus
{
	public sealed class UGTextFormat : IUGTextFormat
	{
		object IUGObject.Native => Native;
		public Font Native
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

		private FontFamily _fontFamily;
		private Font _native;

		public UGTextFormat() { }

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			InvalidateFont();
			GC.SuppressFinalize(this);
		}

		private void InvalidateFont()
		{
			if (_native != null)
			{
				_native.Dispose();
				_native = null;
			}
			if (_fontFamily != null)
			{
				_fontFamily.Dispose();
				_fontFamily = null;
			}
		}

		private Font GetFont()
		{
			if (_native == null)
			{
				_fontFamily = !string.IsNullOrEmpty(FontFamily)
					? new FontFamily(FontFamily)
					: new FontFamily(GenericFontFamilies.SansSerif);
				_native = new Font(_fontFamily, FontSize);
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
