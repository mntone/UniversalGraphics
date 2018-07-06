using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalGraphics.Test
{
	public sealed partial class App : Application
    {
        public App() => InitializeComponent();
		
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
			Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(160.0, 160.0));

			var rootFrame = (Frame)Window.Current.Content;
            if (rootFrame == null)
            {
				rootFrame = new Frame();
				Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                Window.Current.Activate();
            }
        }
    }
}
