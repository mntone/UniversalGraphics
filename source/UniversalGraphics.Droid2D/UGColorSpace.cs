using Android.Graphics;
using System;

namespace UniversalGraphics.Droid2D
{
	public sealed class UGColorSpace : IUGColorSpace
	{
		object IUGObject.Native => Native;

		public ColorSpace Native { get; }

		internal UGColorSpace(ColorSpace native)
			=> Native = native;

		public void Dispose()
		{
			Native.Dispose();
			GC.SuppressFinalize(this);
		}

		public static implicit operator ColorSpace(UGColorSpace d)
			=> d.Native;
	}
}
