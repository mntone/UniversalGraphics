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

		public string FontFamily
		{
			get => _FontFamily;
			set
			{
				if (_FontFamily != value)
				{
					_FontFamily = value;

					using (var typeface = Typeface.Create(_FontFamily, TypefaceStyle.Normal))
					{
						_native.SetTypeface(typeface);
					}
				}
			}
		}
		private string _FontFamily;

		public float FontSize
		{
			get => _native.TextSize;
			set => _native.TextSize = value;
		}
	}
}
