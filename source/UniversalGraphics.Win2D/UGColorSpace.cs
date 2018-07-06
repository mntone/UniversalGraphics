using Microsoft.Graphics.Canvas;
using System;

namespace UniversalGraphics.Win2D
{
	public sealed class UGColorSpace : IUGColorSpace
	{
		object IUGObject.Native => Native;

		public CanvasColorSpace Native { get; }

		internal UGColorSpace(CanvasColorSpace native)
			=> Native = native;

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		public static implicit operator CanvasColorSpace(UGColorSpace d)
			=> d.Native;
	}
}
