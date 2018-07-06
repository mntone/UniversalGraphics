using System;
using UniversalGraphics.GdiPlus.Win32;

namespace UniversalGraphics.GdiPlus
{
	public sealed class UGColorSpace : IUGColorSpace
	{
		object IUGObject.Native => Native;
		public Win32ColorSpace Native { get; }

		internal UGColorSpace(Win32ColorSpace native)
		{
			Native = native;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
