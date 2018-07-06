using System;
using System.Diagnostics;
using System.Windows.Media;

namespace UniversalGraphics.Wpf
{
	public sealed class UGSolidColorBrush : IUGSolidColorBrush, IUGBrushInternal
	{
		object IUGObject.Native => Native;
		Brush IUGBrushInternal.Native => Native;
		public SolidColorBrush Native
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

		private readonly SolidColorBrush _native;

		private bool _disposed = false;

		public UGSolidColorBrush(IUGContext context, UGColor color)
		{
			var native = new SolidColorBrush(color.ToWPFColor());
			native.Freeze();
			_native = native;
		}

		internal UGSolidColorBrush(SolidColorBrush native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			GC.SuppressFinalize(this);
		}

		public UGColor Color => Native.Color.ToUGColor();

		public static implicit operator SolidColorBrush(UGSolidColorBrush d)
			=> d.Native;
	}
}
