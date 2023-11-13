using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace xidata_v2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			Loaded += MainWindow_Loaded;
			Closed += MainWindow_Closed;
		}

		// stuffs


		#region Window Events and GUI (Close/Loaded, Background)

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			SetBackground("background.png");
		}

		private void MainWindow_Closed(object sender, EventArgs e)
		{
			// Shutdown the application
			Application.Current.Shutdown();
		}

		public void CloseApplication()
		{
			// todo - end background workers and timers.
			Close();
		}

		private void ButtonCloseApp_Click(object sender, RoutedEventArgs e)
		{
			CloseApplication();
		}

		public void SetBackground(string backgroundImage)
		{
			// Load image from local resources
			string newImageResourcePath = $"pack://application:,,,/assets/{backgroundImage}";
			BitmapImage bitmapImage = new(new Uri(newImageResourcePath));
			AppBackgroundImage.ImageSource = bitmapImage;
		}

		private void Window_Drag(object sender, MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (e.LeftButton == MouseButtonState.Pressed)
			{
				DragMove();

			}
		}

		#endregion
	}
}
