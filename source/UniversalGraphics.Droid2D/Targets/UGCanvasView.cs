using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using System;

namespace UniversalGraphics.Droid2D
{
	public abstract class UGCanvasViewBase : View
	{
		public UGCanvasViewBase(Context context) : base(context) { }

		public UGCanvasViewBase(Context context, IAttributeSet attrs) : base(context, attrs) { }

		public UGCanvasViewBase(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		public UGCanvasViewBase(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }

		protected sealed override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);

			var density = Resources.DisplayMetrics.Density;
			var canvasSize = new UGSize(Width / density, Height / density);
			using (var ugContext = new UGContext(canvas, canvasSize, density))
			{
				canvas.Scale(density, density);
				DrawOverride(ugContext);
			}
		}

		protected virtual void DrawOverride(IUGContext context)
		{
		}
	}

	public class UGCanvasView : UGCanvasViewBase
	{
		public UGCanvasView(Context context) : base(context) => Initialize();

		public UGCanvasView(Context context, IAttributeSet attrs) : base(context, attrs) => Initialize();

		public UGCanvasView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) => Initialize();

		public UGCanvasView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) => Initialize();

		protected virtual void Initialize() { }

		protected override void DrawOverride(IUGContext context)
		{
			Delegate?.OnDraw(context);
		}

		private void OnInvalidating(object sender, EventArgs e) => Invalidate();

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
