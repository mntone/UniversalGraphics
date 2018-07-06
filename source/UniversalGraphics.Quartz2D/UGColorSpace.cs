using CoreGraphics;
using System;

namespace UniversalGraphics.Quartz2D
{
	public sealed class UGColorSpace : IUGColorSpace
	{
		object IUGObject.Native => Native;
		public CGColorSpace Native { get; }

		internal UGColorSpace(CGColorSpace native)
			=> Native = native;

		public void Dispose()
		{
			Native.Dispose();
			GC.SuppressFinalize(this);
		}

		public static implicit operator CGColorSpace(UGColorSpace d)
			=> d.Native;
	}
}
