using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using xidata_v2.source;

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

		#region Window Events and GUI (Close/Loaded, Background)

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			// Start logger output timer
			Logger.Init();
			InitLogger();

			// Set background image
			SetBackground("background.png");

			Logger.Add("Welcome to XI Data v2.0");
			Logger.Add("Build by Vekien");

		
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

		#region Logging

		/// <summary>
		/// Initialise the application logger
		/// </summary>
		private void InitLogger()
		{
			// clear current log view
			AppLog.Text = "";

			DispatcherTimer logTimer = new()
			{
				Interval = TimeSpan.FromMilliseconds(1000)
			};

			logTimer.Tick += (sender, e) => {
				// Fetch the most recent logs
				List<string> recentLogs = Logger.GetRecent();

				if (recentLogs.Count > 0)
				{
					// Append them 
					foreach (string line in recentLogs)
					{
						AppLog.AppendText($"{line}\n");
						AppLog.ScrollToEnd();
					}
				}
			};

			// Start log timer
			logTimer.Start();
		}

		#endregion

		/// <summary>
		/// returns all the lines in a xidata resource data txt file.
		/// </summary>
		/// <param name="resourceDataName"></param>
		/// <returns></returns>
		private static List<string> GetResourceData(string resourceDataName)
		{
			List<string> lines = [];
			string filename = $"{Folders.Root()}\\data\\{resourceDataName}.txt";

			if (File.Exists(filename))
			{
				using StreamReader reader = new(filename);

				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine();
					lines.Add(line);
				}
			}
			else
			{
				Logger.Add("xidata resource file not found: " + filename);
			}

			return lines;
		}
	}
}
