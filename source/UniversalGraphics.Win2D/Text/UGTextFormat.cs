using System;
using System.Diagnostics;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI.Text;

namespace UniversalGraphics.Win2D
{
	public sealed class UGTextFormat : IUGTextFormat
	{
		object IUGObject.Native => Native;
		public CanvasTextFormat Native
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

		private readonly CanvasTextFormat _native;

		private bool _disposed = false;

		public UGTextFormat() => _native = new CanvasTextFormat();

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}
		
		public string FontFamily
		{
			get => _native.FontFamily;
			set => _native.FontFamily = value;
		}
		public float FontSize
		{
			get => _native.FontSize;
			set => _native.FontSize = value;
		}
		public bool IsItalic
		{
			get => _native.FontStyle != FontStyle.Normal;
			set => _native.FontStyle = FontStyle.Italic;
		}
	}
}
