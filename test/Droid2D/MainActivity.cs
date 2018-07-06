using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Java.Lang;
using ReactiveUI;
using ReactiveUI.AndroidSupport;
using UniversalGraphics.Droid2D;
using UniversalGraphics.Test.Infrastructures;
using UniversalGraphics.Test.ViewModels;

namespace UniversalGraphics.Test
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
	public class MainActivity : ReactiveAppCompatActivity<ViewModel>
	{
		private CompositeDisposable _disposables;

		private Spinner _delegateSpinnerView;
		private UGCanvasView _canvasView;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_main);

			System.Diagnostics.Debug.Assert(_disposables == null);
			System.Diagnostics.Debug.Assert(ViewModel == null);
			_disposables = new CompositeDisposable();

			ViewModel = new ViewModel();

			_delegateSpinnerView = FindViewById<Spinner>(Resource.Id.delegate_spinner_view);
			this.OneWayBind(ViewModel, vm => vm.Data, v => v._delegateSpinnerView.Adapter, data => new SpinnerAdapter(this, data))
				.AddTo(_disposables);
			this.WhenAnyValue(v => v.ViewModel.SelectedIndex)
				.Subscribe(idx => _delegateSpinnerView.SetSelection(idx, true))
				.AddTo(_disposables);
			Observable.FromEvent<EventHandler<AdapterView.ItemSelectedEventArgs>, AdapterView.ItemSelectedEventArgs>(
				f => (s, e) => f(e),
				h => _delegateSpinnerView.ItemSelected += h,
				h => _delegateSpinnerView.ItemSelected -= h)
				.Subscribe(e => ViewModel.SelectedIndex = e.Position)
				.AddTo(_disposables);

			_canvasView = FindViewById<UGCanvasView>(Resource.Id.canvas_view);
			this.OneWayBind(ViewModel, vm => vm.Delegate, v => v._canvasView.Delegate)
				.AddTo(_disposables);
		}

		protected override void OnDestroy()
		{
			System.Diagnostics.Debug.Assert(_disposables != null);
			System.Diagnostics.Debug.Assert(ViewModel != null);
			ViewModel = null;

			_disposables.Dispose();
			_disposables = null;

			base.OnDestroy();
		}

		private sealed class SpinnerAdapter : ArrayAdapter<string>
		{
			public SpinnerAdapter(Context context, string[] data) : base(context, Android.Resource.Layout.SimpleSpinnerItem, data) { }
		}
	}
}
