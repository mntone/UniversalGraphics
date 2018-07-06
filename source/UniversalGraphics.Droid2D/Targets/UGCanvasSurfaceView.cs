using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using System;

namespace UniversalGraphics.Droid2D
{
	public abstract class UGCanvasSurfaceViewBase : SurfaceView, ISurfaceHolderCallback
	{
		private bool _destroyed = false;

		public UGCanvasSurfaceViewBase(Context context) : base(context) => Initialize();

		public UGCanvasSurfaceViewBase(Context context, IAttributeSet attrs) : base(context, attrs) => Initialize();

		public UGCanvasSurfaceViewBase(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) => Initialize();

		public UGCanvasSurfaceViewBase(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) => Initialize();

		protected virtual void Initialize()
		{
			Holder.AddCallback(this);
		}

		private void Draw()
		{
			if (Holder.IsCreating || _destroyed) return;

			Canvas canvas = null;
			try
			{
				canvas = Holder.LockCanvas();
				var density = Resources.DisplayMetrics.Density;
				var canvasSize = new UGSize(Width / density, Height / density);
				using (var ugContext = new UGContext(canvas, canvasSize, density))
				{
					canvas.Scale(density, density);
					DrawOverride(ugContext);
				}
			}
			finally
			{
				if (canvas != null)
				{
					Holder.UnlockCanvasAndPost(canvas);
				}
			}
		}

		protected void InvalidateVisual()
		{
			Draw();
		}

		protected virtual void DrawOverride(IUGContext context)
		{
		}

		void ISurfaceHolderCallback.SurfaceCreated(ISurfaceHolder holder) => Draw();
		void ISurfaceHolderCallback.SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height) => Draw();
		void ISurfaceHolderCallback.SurfaceDestroyed(ISurfaceHolder holder) => _destroyed = true;
	}

	public class UGCanvasSurfaceView : UGCanvasSurfaceViewBase
	{
		public UGCanvasSurfaceView(Context context) : base(context) { }

		public UGCanvasSurfaceView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		public UGCanvasSurfaceView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		public UGCanvasSurfaceView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }

		protected override void DrawOverride(IUGContext context)
		{
			Delegate?.OnDraw(context);
		}

		private void OnInvalidating(object sender, EventArgs e) => InvalidateVisual();

		private void OnDelegateChanged(IUGCanvasViewDelegate oldDelegate)
		{
			if (oldDelegate is IUGAnimatableCanvasViewDelegate oldAnimatable)
			{
				oldAnimatable.Invalidating -= OnInvalidating;
			}

			if (Delegate is IUGAnimatableCanvasViewDelegate animatable)
			{
				animatable.Invalidating += OnInvalidating;
			}

			Invalidate();
		}

		public IUGCanvasViewDelegate Delegate
		{
			get => _Delegate;
			set
			{
				if (_Delegate != value)
				{
					var oldDelegate = _Delegate;
					_Delegate = value;
					OnDelegateChanged(oldDelegate);
				}
			}
		}
		private IUGCanvasViewDelegate _Delegate;
	}
}
