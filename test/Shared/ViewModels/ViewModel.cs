using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using UniversalGraphics.Test.Infrastructures;

namespace UniversalGraphics.Test.ViewModels
{
	public sealed class ViewModel : ReactiveObject
	{
		private CompositeDisposable _disposables;

		public ViewModel()
		{
			_disposables = new CompositeDisposable();
			this.ObservableForProperty(vm => vm.Delegate, beforeChange: true)
				.Subscribe(OnDelegateChanging)
				.AddTo(_disposables);

			this.WhenAnyValue(vm => vm.SelectedIndex)
				.Subscribe(OnDelegateChanged)
				.AddTo(_disposables);

			SelectedIndex = DelegateTypes.Select((d, idx)=> new { Data = d, Index = idx })
				.Where(t => t.Data == typeof(Delegates.ColorPalletCanvasViewDelegate))
				.Select(t => t.Index)
				.Single();
		}

		private void OnDelegateChanging(IObservedChange<ViewModel, IUGCanvasViewDelegate> oldValues)
		{
			if (oldValues.Value is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		private void OnDelegateChanged(int idx)
		{
			var type = DelegateTypes[idx];
			var @delegate = DelegateRetriver.CreateDelegate(type);
			Delegate = @delegate;
		}

		private Type[] DelegateTypes => DelegateRetriver.GetDelegateTypes();
		public string[] Data => DelegateTypes.Select(type => type.Name).ToArray();

		public int SelectedIndex
		{
			get => _SelectedIndex;
			set => this.RaiseAndSetIfChanged(ref _SelectedIndex, value);
		}
		private int _SelectedIndex;
		
		public IUGCanvasViewDelegate Delegate
		{
			get => _Delegate;
			set => this.RaiseAndSetIfChanged(ref _Delegate, value);
		}
		private IUGCanvasViewDelegate _Delegate;
	}
}
