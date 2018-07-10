using System;
using System.Diagnostics;
using Android.Graphics;
using Android.Text;

namespace UniversalGraphics.Droid2D
{
	public sealed class UGTextFormat : IUGTextFormat
	{
		object IUGObject.Native => Native;
		public TextPaint Native
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

		private readonly TextPaint _native;

		private bool _disposed = false;

		public UGTextFormat() => _native = new TextPaint();

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		private void SetFont()
		{
			using (var typeface = Typeface.Create(_FontFamily, IsItalic ? TypefaceStyle.Italic : TypefaceStyle.Normal))
			{
				_native.SetTypeface(typeface);
			}
		}

		public string FontFamily
		{
			get => _FontFamily;
			set
			{
				if (_FontFamily != value)
				{
					_FontFamily = value;
					SetFont();
				}
			}
		}
		private string _FontFamily;

		public float FontSize
		{
			get => _native.TextSize;
			set => _native.TextSize = value;
		}

		public bool IsItalic
		{
			get => _IsItalic;
			set
			{
				if (_IsItalic != value)
				{
					_IsItalic = value;
					SetFont();
				}
			}
		}
		private bool _IsItalic;
	}
}
