using UniversalGraphics.Test.ViewModels;
using Windows.UI.Xaml.Controls;

namespace UniversalGraphics.Test
{
	public sealed partial class MainPage : Page
	{
		private ViewModel ViewModel { get; }

		public MainPage()
		{
			InitializeComponent();

			ViewModel = new ViewModel();
		}
	}
}
