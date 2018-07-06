using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;
using UniversalGraphics.GdiPlus;
using UniversalGraphics.Test.Infrastructures;
using UniversalGraphics.Test.ViewModels;

namespace UniversalGraphics.Test
{
	public partial class Form1 : Form, IViewFor<ViewModel>
	{
		private const int TOP_MARGIN = 40;
		private UGCanvasControl _control;

		private CompositeDisposable _disposables;

		public Form1()
		{
			InitializeComponent();
			
			var clientArea = ClientRectangle;
			_control = new UGCanvasControl(this, "", clientArea.Left, clientArea.Top + TOP_MARGIN, clientArea.Width, clientArea.Height - TOP_MARGIN);

			_disposables = new CompositeDisposable();
			_disposables.Add(_control);

			ViewModel = new ViewModel();
			OnBind();
		}

		private void OnBind()
		{
			this.OneWayBind(ViewModel, vm => vm.Data, v => v.delegateTypesComboBox.DataSource)
				.AddTo(_disposables);
			var selectionChanged = Observable.FromEvent<EventHandler, EventArgs>(
				 f => (_, e) => f(e),
				 h => delegateTypesComboBox.SelectedIndexChanged += h,
				 h => delegateTypesComboBox.SelectedIndexChanged += h);
			this.Bind(ViewModel, vm => vm.SelectedIndex, v => v.delegateTypesComboBox.SelectedIndex, selectionChanged)
				.AddTo(_disposables);
			this.OneWayBind(ViewModel, vm => vm.Delegate, v => v._control.Delegate)
				.AddTo(_disposables);
		}

		protected override void OnClosed(EventArgs e)
		{
			_disposables.Dispose();

			base.OnClosed(e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			if (_control != null)
			{
				var size = ClientSize;
				size.Height -= TOP_MARGIN;
				_control.Size = size;
			}
		}

		public ViewModel ViewModel { get; set; }
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ViewModel)value; }
		}
	}
}
