using System;
using System.Windows.Media;

namespace UniversalGraphics.Wpf
{
	internal sealed class CanvasLayer : IDisposable
	{
		public readonly UGContext _context;

		internal int _count;

		public CanvasLayer(UGContext context) : this(context, 0) { }

		public CanvasLayer(UGContext context, float alpha) : this(context, 1)
		{
			_context.Native.PushOpacity(alpha);
		}

		public CanvasLayer(UGContext context, UGRect clipRect) : this(context, 1)
		{
			var geometry = new RectangleGeometry(clipRect.ToWPFRect());
			_context.Native.PushClip(geometry);
		}

		public CanvasLayer(UGContext context, float alpha, UGRect clipRect) : this(context, 2)
		{
			_context.Native.PushOpacity(alpha);
			var geometry = new RectangleGeometry(clipRect.ToWPFRect());
			_context.Native.PushClip(geometry);
		}

		private CanvasLayer(UGContext context, int count)
		{
			_context = context;
			_count = count;
		}

		public void Dispose()
		{
			var native = _context.Native;
			while (_count-- > 0)
			{
				native.Pop();
			}
			_context._layers.Pop();
			GC.SuppressFinalize(this);
		}
	}
}
