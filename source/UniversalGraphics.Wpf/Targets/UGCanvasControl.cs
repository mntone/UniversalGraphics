using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace UniversalGraphics.Wpf
{
	public abstract class UGCanvasElementBase : UIElement
	{
		protected UGCanvasElementBase() { }

		protected sealed override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);

			var matrix = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
			var scale = (float)matrix.M11;
			using (var ugContext = new UGContext(this, drawingContext, RenderSize, scale))
			{
				DrawOverride(ugContext);
			}
		}

		protected virtual void DrawOverride(IUGContext context)
		{
		}
	}

	public abstract class UGCanvasBindableBase : FrameworkElement
	{
		protected UGCanvasBindableBase() { }

		protected sealed override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);

			var matrix = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
			var scale = (float)matrix.M11;
			using (var ugContext = new UGContext(this, drawingContext, RenderSize, scale))
			{
				DrawOverride(ugContext);
			}
		}

		protected virtual void DrawOverride(IUGContext context)
		{
		}
	}

	public class UGCanvasBindableElement : UGCanvasBindableBase
	{
		public UGCanvasBindableElement() => Initialize();

		protected virtual void Initialize()
		{
			IsDesignMode = DesignerProperties.GetIsInDesignMode(this);
		}

		protected override void DrawOverride(IUGContext context)
		{
			if (IsDesignMode)
				return;

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

			InvalidateVisual();
		}

		public bool IsDesignMode { get; private set; }

		public IUGCanvasViewDelegate Delegate
		{
			get => (IUGCanvasViewDelegate)GetValue(DelegateProperty);
			set => SetValue(DelegateProperty, value);
		}

		public static readonly DependencyProperty DelegateProperty
			= DependencyProperty.Register(nameof(Delegate), typeof(IUGCanvasViewDelegate), typeof(UGCanvasBindableElement), new PropertyMetadata(null, OnDelegateChanged));

		private static void OnDelegateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var canvasElement = (UGCanvasBindableElement)d;
			canvasElement.OnDelegateChanged((IUGCanvasViewDelegate)e.OldValue);
		}
	}
}
