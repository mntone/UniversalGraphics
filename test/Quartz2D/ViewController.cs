//#define USE_LAYER

using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using AppKit;
using Foundation;
using ReactiveUI;
using UniversalGraphics.Quartz2D;
using UniversalGraphics.Test.Infrastructures;
using UniversalGraphics.Test.ViewModels;

namespace UniversalGraphics.Test
{
	public partial class ViewController : ReactiveViewController<ViewModel>
	{
		private readonly NSString INDEX_OF_SELECTED_ITEM_PROPERTY_NAME = new NSString("indexOfSelectedItem");

		private CompositeDisposable _disposables;

#if USE_LAYER
		private UGCanvasCALayer _layer;
#else
		private UGCanvasView _canvas;
#endif

		public ViewController(IntPtr handle) : base(handle)
		{
			_disposables = new CompositeDisposable();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
#if USE_LAYER
				if (_layer != null)
				{
					_layer.RemoveFromSuperLayer();
					_layer.Dispose();
					_layer = null;
				}
#else
				if (_canvas != null)
				{
					_canvas.Dispose();
					_canvas = null;
				}
#endif
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var frame = View.Frame;
			frame.Height -= delegatePopUpButton.VisibleRect().Height;
#if USE_LAYER
			View.WantsLayer = true;
			var layer = new UGCanvasCALayer();
			layer.Frame = View.Frame;
			View.Layer.AddSublayer(layer);
			_layer = layer;
#else
			var canvas = new UGCanvasView(frame)
			{
				AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable,
			};
			View.AddSubview(canvas);
			_canvas = canvas;
#endif

			ViewModel = new ViewModel();
			this.WhenAnyValue(v => v.ViewModel.Data)
				.Subscribe(data =>
				{
					delegatePopUpButton.RemoveAllItems();
					delegatePopUpButton.AddItems(data);
				})
				.AddTo(_disposables);
			this.WhenAnyValue(v => v.ViewModel.SelectedIndex)
				.Subscribe(idx => delegatePopUpButton.SelectItem(idx))
			    .AddTo(_disposables);
			Observable.FromEvent<EventHandler, EventArgs>(
				f => (s, e) => f(e),
				h => delegatePopUpButton.Activated += h,
				h => delegatePopUpButton.Activated -= h)
				.Subscribe(_ => ViewModel.SelectedIndex = (int)delegatePopUpButton.IndexOfSelectedItem)
				.AddTo(_disposables);

#if USE_LAYER
			this.OneWayBind(ViewModel, vm => vm.Delegate, v => v._layer.CanvasDelegate)
				.AddTo(_disposables);
#else
			this.OneWayBind(ViewModel, vm => vm.Delegate, v => v._canvas.Delegate)
				.AddTo(_disposables);
#endif
		}
	}
}
