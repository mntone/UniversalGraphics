using System;
using System.Diagnostics;
using System.Drawing;

namespace UniversalGraphics.GdiPlus
{
	public sealed class UGSolidColorBrush : IUGSolidColorBrush, IUGBrushInternal
	{
		object IUGObject.Native => Native;
		Brush IUGBrushInternal.Native => Native;
		public SolidBrush Native
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

		private readonly SolidBrush _native;

		private bool _disposed = false;

		public UGSolidColorBrush(IUGContext context, UGColor color)
			=> _native = new SolidBrush(color.ToGDIColor());

		internal UGSolidColorBrush(SolidBrush native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		public UGColor Color => Native.Color.ToUGColor();

		public static implicit operator SolidBrush(UGSolidColorBrush d)
			=> d.Native;
	}
}
