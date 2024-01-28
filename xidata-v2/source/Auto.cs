using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace xidata_v2.source
{
	internal class Auto
	{
		[DllImport("user32.dll")]
		static extern IntPtr FindWindow(string className, string windowName);
		[DllImport("user32.dll")]
		static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("user32.dll")]
		static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll")]
		static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
		[DllImport("user32.dll")]
		static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
		delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

		public static bool IsNoesisOpen = false;

		/// <summary>
		/// Checks if Noesis is running, if not, start it!
		/// </summary>
		public static void OpenNoesis()
		{
			if (IsProcessRunning("Noesis"))
			{
				IsNoesisOpen = true;
				return;
			}

			IsNoesisOpen = false;

			// Start worker thread
			BackgroundWorker worker = new()
			{
				WorkerSupportsCancellation = true,
				WorkerReportsProgress = true
			};

			worker.DoWork += (sender, e) =>
			{
				Process.Start(Settings.GetNoesisPath());
				WaitForProcess("Noesis");

				// When Noesis Loads, it auto routes to previous directory and file,
				// so we will add another delay here to let it do that...
				Thread.Sleep(5500);
			};

			worker.RunWorkerCompleted += (sender, e) =>
			{
				IsNoesisOpen = true;
				worker.CancelAsync();
				worker.Dispose();
			};

			worker.RunWorkerAsync();
		}

		/// <summary>
		/// Checks if a process is running
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool IsProcessRunning(string name)
		{
			foreach (Process process in Process.GetProcesses())
			{
				if (process.ProcessName.Contains(name))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Send a key to a specific window title.
		/// Default to Noesis.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="wait"></param>
		/// <param name="window"></param>
		public static void SendKey(string key, int wait = 0, string window = "Noesis")
		{
			// Set wait
			wait = wait > 0 ? wait : Settings.GetSendKeyDelay();

			try
			{
				// Focus a window
				if (window != "")
				{
					IntPtr hWnd = FindWindow(null, window);
					SetForegroundWindow(hWnd);
				}

				// send the key
				//ConsoleLog($">>(SendKey) Key: {key}");
				SendKeys.SendWait(key);
				Thread.Sleep(wait);
			}
			catch (Exception Ex)
			{
				Logger.Add($"!!! (SendKey) Exception Thrown: {Ex.Message}");
			}
		}

		/// <summary>
		/// This will Copy the text to the clipboard and then paste it, as it's a thousand times faster than Sendkey
		/// </summary>
		/// <param name="text"></param>
		/// <param name="wait"></param>
		/// <param name="window"></param>
		public static void SendText(string text, int wait = 0, string window = "Noesis")
		{
			Instances.MainWindow.GetDispatcher().Invoke(() =>
			{
				Clipboard.SetText(text);
			});
			
			SendKey("^v", wait, window);
		}

		/// <summary>
		/// Sends the tab key multiple times, hard coded to "Export Media" as that
		/// is usually the only window that needs this logic...
		/// </summary>
		/// <param name="count"></param>
		/// <param name="window"></param>
		public static void SendKeyTabN(int count, int wait = 25, string window = "Export Media")
		{
			for (int i = 1; i <= count; i++)
			{
				SendKey("{Tab}", wait, window);
			}
		}

		/// <summary>
		/// Sends a specific key multiple times.
		/// </summary>
		/// <param name="count"></param>
		/// <param name="key"></param>
		/// <param name="window"></param>
		public static void SendKeyN(int count, string key, int wait = 25, string window = "Export Media")
		{
			for (int i = 1; i <= count; i++)
			{
				SendKey(key, wait, window);
			}
		}

		/// <summary>
		/// Wait for the active window to be said windowTitle, improves timing of send key flows.
		/// </summary>
		/// <param name="windowTitle"></param>
		/// <returns></returns>
		public static bool WaitForActiveWindow(string windowTitle, int waitInSeconds = 30)
		{
			waitInSeconds *= 2;

			for (int i = 0; i <= waitInSeconds; i++)
			{
				if (GetActiveWindowTitle() == windowTitle)
				{
					return true;
				}

				Thread.Sleep(500);
			}

			Logger.Add($"!! Error: Could not detect the Window: {windowTitle} - Current active window: {GetActiveWindowTitle()}");
			return false;
		}

		/// <summary>
		/// Wait for a specific number of windows with the name windowTitle
		/// </summary>
		/// <param name="windowTitle"></param>
		/// <param name="waitInSeconds"></param>
		/// <param name="windowCount"></param>
		/// <param name="stopOnError"></param>
		/// <returns></returns>
		public static bool WaitForActiveWindowCount(string windowTitle, int windowCount = 1, int waitInSeconds = 30)
		{
			waitInSeconds *= 2;

			for (int i = 0; i <= waitInSeconds; i++)
			{
				if (CountActiveWindows(windowTitle) == windowCount)
				{
					return true;
				}

				Thread.Sleep(500);
			}

			Logger.Add($"!! Error: Could not find a total of {windowCount} windows titled {windowTitle}");
			return false;
		}

		/// <summary>
		/// Wait for the a process to start
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool WaitForProcess(string name, int waitInSeconds = 30)
		{

			waitInSeconds *= 2;

			for (int i = 0; i <= waitInSeconds; i++)
			{
				if (IsProcessRunning("Noesis"))
				{
					return true;
				}

				Thread.Sleep(500);
			}

			Logger.Add($"!! Error: Could not detect the open process: {name}");
			return false;
		}

		/// <summary>
		/// https://stackoverflow.com/questions/115868/how-do-i-get-the-title-of-the-current-active-window-using-c
		/// </summary>
		/// <returns></returns>
		public static string GetActiveWindowTitle()
		{
			const int nChars = 256;
			StringBuilder Buff = new(nChars);
			IntPtr handle = GetForegroundWindow();

			if (GetWindowText(handle, Buff, nChars) > 0)
			{
				return Buff.ToString();
			}
			return null;
		}

		/// <summary>
		/// Get a count of the number of windows with this window title.
		/// </summary>
		/// <param name="windowTitle"></param>
		/// <returns></returns>
		public static int CountActiveWindows(string windowTitle)
		{
			List<IntPtr> windows = [];
			EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
			{
				windows.Add(hWnd);
				return true;
			}, IntPtr.Zero);

			int count = 0;
			foreach (IntPtr hWnd in windows)
			{
				StringBuilder sb = new(256);
				if (GetWindowText(hWnd, sb, sb.Capacity) > 0 && sb.ToString() == windowTitle)
				{
					count++;
				}
			}

			return count;
		}

		/// <summary>
		/// Focus Noesis
		/// </summary>
		public static void FocusNoesis()
		{
			IntPtr hWnd = FindWindow(null, "Noesis");
			SetForegroundWindow(hWnd);
			Thread.Sleep(500);
		}
	}
}
