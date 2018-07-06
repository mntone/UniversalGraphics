using Android.Graphics;
using System;
using System.Diagnostics;

namespace UniversalGraphics.Droid2D
{
	public sealed class UGSolidColorBrush : IUGSolidColorBrush
	{
		object IUGObject.Native => Native;
		public Color Native
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

		private readonly Color _native;

		private bool _disposed = false;

		public UGSolidColorBrush(IUGContext context, UGColor color)
		{
			_native = color.ToAGColor();
		}

		internal UGSolidColorBrush(Color native)
			=> _native = native;

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			GC.SuppressFinalize(this);
		}

		public UGColor Color => Native.ToUGColor();

		public static implicit operator Color(UGSolidColorBrush d)
			=> d.Native;
	}
}
