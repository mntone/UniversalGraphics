using Android.Graphics;
using Android.OS;
using System;

namespace UniversalGraphics.Droid2D
{
	public sealed class UGCanvasRenderTarget : IUGCanvasRenderTarget
	{
		object IUGObject.Native => Native;
		public Bitmap Native
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

		private readonly Bitmap _native;

		private bool _disposed = false;

		public UGCanvasRenderTarget(UGSize canvasSize) : this(canvasSize, 1F) { }

		public UGCanvasRenderTarget(UGSize canvasSize, float scale)
		{
			Size = canvasSize;
			Scale = scale;

			var width = (int)(scale * Width + .5F);
			var height = (int)(scale * Height + .5F);
			Bitmap native;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				using (var colorSpace = ColorSpace.Get(ColorSpace.Named.Srgb))
				{
					native = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888, true, colorSpace);
				}
			}
			else
			{
				native = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
			}
			native.Density = (int)(160F * Scale + .5F);
			_native = native;
		}

		public void Dispose()
		{
			System.Diagnostics.Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		public IUGContext CreateDrawingSession()
		{
			var canvas = new Canvas(_native);
			canvas.Scale(Scale, Scale);
			return new UGContext(canvas, Size, Scale, () =>
			{
				canvas.Dispose();
				canvas = null;
			});
		}

		public float Width => Size.Width;
		public float Height => Size.Height;
		public UGSize Size { get; }
		public float Scale { get; }
	}
}
