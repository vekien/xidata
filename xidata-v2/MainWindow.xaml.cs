using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
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
			// Set the main window instance
			Instances.MainWindow = this;

			InitializeComponent();

			// Subscribe to the DispatcherUnhandledException event
			Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			Loaded += MainWindow_Loaded;
			Closed += MainWindow_Closed;
		}

		#region Window Events and GUI (Close/Loaded, Background)

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			// Start logger output timer
			Logger.Init();
			StartLoggerTimer();

			// Ini Settings
			Settings.InitSettings();

			// Set background image
			SetBackground("background.png");

			// Setup content type dropdown
			InputContentType.Get().SelectedIndex = 0;
			InputContentType.DepSelectItems = String.Join(",", Data.ContentType);
			InputNoesisArguments.DepText = Settings.GetNoesisArgs();

			// Subscribe to hook listener
			HookSubscribe();

			// Credits
			Logger.Add("Welcome to XI Data v2.0");
			Logger.Add("Build by Vekien");
		}

		private void MainWindow_Closed(object sender, EventArgs e)
		{
			// Unsub the hook listener
			HookUnsubscribe();


			// Shutdown the application
			Application.Current.Shutdown();
		}

		private void ButtonCloseApp_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Window_Drag(object sender, MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (e.LeftButton == MouseButtonState.Pressed)
			{
				DragMove();

			}
		}

		/// <summary>
		/// Set the background image for the application
		/// </summary>
		/// <param name="backgroundImage"></param>
		private void SetBackground(string backgroundImage)
		{
			// Load image from local resources
			string newImageResourcePath = $"pack://application:,,,/assets/{backgroundImage}";
			BitmapImage bitmapImage = new(new Uri(newImageResourcePath));
			AppBackgroundImage.ImageSource = bitmapImage;
		}

		/// <summary>
		/// Stop button clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonStopExtract_Click(object sender, RoutedEventArgs e)
		{
			StopExtraction();
		}

		/// <summary>
		/// Start extraction
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonStartExtract_Click(object sender, RoutedEventArgs e)
		{
			StartExtraction();
		}

		/// <summary>
		/// Catch exceptions
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			Logger.Exception(e.Exception);
			e.Handled = true;
		}

		/// <summary>
		/// Catch other exceptions
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			Logger.Exception(ex);
		}

		/// <summary>
		/// Get the windows current dispatcher
		/// </summary>
		/// <returns></returns>
		public Dispatcher GetDispatcher()
		{
			return Application.Current.Dispatcher;
		}

		#endregion

		#region Application Keyboard Hook Listener

		/// -----------------------------------------------------------------------------------------
		/// Escape Key Listener
		/// - This listens for "Escape" key
		/// It uses: https://github.com/gmamaladze/globalmousekeyhook
		/// -----------------------------------------------------------------------------------------

		private IKeyboardMouseEvents m_GlobalHook;
		private void HookSubscribe()
		{
			m_GlobalHook = Hook.GlobalEvents();
			m_GlobalHook.KeyPress += M_GlobalHook_KeyPress;
		}

		private void HookUnsubscribe()
		{
			m_GlobalHook.KeyPress -= M_GlobalHook_KeyPress;
			m_GlobalHook.Dispose();
		}

		private void M_GlobalHook_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			string character = e.KeyChar.ToString();
			character = ((sbyte)e.KeyChar == 13) ? "ENTER" : character;
			character = ((sbyte)e.KeyChar == 32) ? "SPACE" : character;
			//Logger.Add($"Key: {(sbyte)e.KeyChar} == {character}");

			// Escape
			if ((sbyte)e.KeyChar == 27)
			{
				return;
			}

			// A
			if ((sbyte)e.KeyChar == 97)
			{
				if (CheckboxHookToStop.IsChecked())
				{
					StopExtraction();
				}

				return;
			}
		}

		#endregion

		#region Logging

		/// <summary>
		/// Initialise the application logger
		/// </summary>
		private void StartLoggerTimer()
		{
			// clear current log view
			AppLog.Text = "";

			DispatcherTimer logTimer = new()
			{
				Interval = TimeSpan.FromMilliseconds(300)
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

		private void StopExtraction()
		{
			Logger.Add("Stopping extraction...");
			Extractor.Stop();
		}

		private void StartExtraction()
		{
			// Grab form data
			string XiContentType = InputContentType.Get().SelectedItem.ToString().Trim();
			string XiNoesisArgs = InputNoesisArguments.Get().Text.ToString().Trim();
			int XiNoesisLimit = InputLimitExportCount.GetNumber();

			// Init extractor
			Extractor.Init(XiContentType, XiNoesisArgs, XiNoesisLimit);
			Logger.Space();
			Logger.Add($"Extractor initialized for: {XiContentType}");

			// Open Noesis
			Logger.Add("Starting Noesis...");
			Auto.OpenNoesis();

			// Start Extraction
			Extractor.Start();
		}
	}
}
