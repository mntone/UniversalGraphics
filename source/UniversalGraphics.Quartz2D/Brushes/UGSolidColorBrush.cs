using CoreGraphics;
using System;
using System.Diagnostics;

namespace UniversalGraphics.Quartz2D
{
	public sealed class UGSolidColorBrush : IUGSolidColorBrush
	{
		object IUGObject.Native => Native;
		public CGColor Native
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

		private readonly CGColor _native;

		private bool _disposed = false;

		public UGSolidColorBrush(IUGContext context, UGColor color)
		{
			_native = color.ToCGColor();
		}

		internal UGSolidColorBrush(CGColor native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		public UGColor Color => _native.ToUGColor();

		public static implicit operator CGColor(UGSolidColorBrush d)
			=> d.Native;
	}
}
