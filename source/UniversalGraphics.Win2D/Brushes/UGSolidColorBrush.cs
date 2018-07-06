using Microsoft.Graphics.Canvas.Brushes;
using System;
using System.Diagnostics;

namespace UniversalGraphics.Win2D
{
	public sealed class UGSolidColorBrush : IUGSolidColorBrush, IUGBrushInternal
	{
		object IUGObject.Native => Native;
		ICanvasBrush IUGBrushInternal.Native => Native;
		public CanvasSolidColorBrush Native
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

		private readonly CanvasSolidColorBrush _native;

		private bool _disposed = false;

		public UGSolidColorBrush(IUGContext context, UGColor color)
			=> _native = new CanvasSolidColorBrush(((UGContext)context).Device, color.ToWinRTColor());

		internal UGSolidColorBrush(CanvasSolidColorBrush native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		public UGColor Color => Native.Color.ToUGColor();

		public static implicit operator CanvasSolidColorBrush(UGSolidColorBrush d)
			=> d.Native;
	}
}
