using System;
using System.Diagnostics;
using Microsoft.Graphics.Canvas.Text;

namespace UniversalGraphics.Win2D
{
	public sealed class UGTextLayout : IUGTextLayout
	{
		object IUGObject.Native => Native;
		public CanvasTextLayout Native
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
		
		private readonly CanvasTextLayout _native;

		private bool _disposed = false;

		public UGTextLayout(IUGContext context, string textString, IUGTextFormat textFormat)
			: this(context, textString, textFormat, UGSize.MaxValue)
		{ }

		public UGTextLayout(IUGContext context, string textString, IUGTextFormat textFormat, UGSize requestedSize)
		{
			var device = ((UGContext)context).Device;
			var format = ((UGTextFormat)textFormat).Native;
			_native = new CanvasTextLayout(device, textString, format, requestedSize.Width, requestedSize.Height);
		}

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		public UGHorizontalAlignment HorizontalAlignment
		{
			get => _native.HorizontalAlignment.ToUGHorizontalAlignment();
			set => _native.HorizontalAlignment = value.ToWin2DHorizontalAlignment();
		}
		public UGRect LayoutBounds => _native.LayoutBounds.ToUGRect();
		public UGVerticalAlignment VerticalAlignment
		{
			get => _native.VerticalAlignment.ToUGVerticalAlignment();
			set => _native.VerticalAlignment = value.ToWin2DVerticalAlignment();
		}
	}
}
