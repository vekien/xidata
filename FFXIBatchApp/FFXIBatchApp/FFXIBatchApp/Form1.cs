using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Linq;
using Gma.System.MouseKeyHook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FFXIBatchApp
{
	public partial class Form1 : Form
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

		Thread ActiveExtractThread;
        bool ActiveThreadRunning = false;

		public Form1()
		{
			InitializeComponent();
		}

		[STAThread]
		private void Form1_Load(object sender, EventArgs e)
		{
            // Load settings
            LoadSettings();

			// Subscribe to hook listeners
			HookSubscribe();

			// Introduction
			ConsoleLog("-----------------------------");
			ConsoleLog("FFXI Batch Exporter v1.0");
			ConsoleLog("> Built by: Vekien");
			ConsoleLog("-----------------------------");

			if (Settings.GetSetting("PathFFXI") == "" || Settings.GetSetting("PathNoesis") == "")
			{
				ConsoleLog("!! Please set your FFXI and Noesis Path");
				ConsoleLog("!! Then restart the application!!!");
				ConsoleLog("!! NOTE: It will take a couple seconds to load as it's building data lists!");
				tabControl1.SelectedIndex = 6;
				return;
			}

			// Start the build of local data
			BuildLocalJsonData();
		}

        private void Form1_Unload(object sender, FormClosingEventArgs e)
        {
            // Remove hook listeners
            HookUnsubscribe();
			StopActiveThread();
		}

        /// <summary>
        /// Load the users settings
        /// </summary>
        private void LoadSettings()
        {
            PathFFXI.Text = Settings.GetSetting("PathFFXI");
            PathNoesis.Text = Settings.GetSetting("PathNoesis");
		}

		/// <summary>
		/// Build the local JSON stuff.
		/// </summary>
		private void BuildLocalJsonData()
        {
			string pathFFXI = Settings.GetSetting("PathFFXI");
			string pathNoesis = Settings.GetSetting("pathNoesis");

            if (pathFFXI.Length == 0) 
            {
                ConsoleLog("Please set your FFXI Path in the settings below!");
                return;
            }

			if (pathNoesis.Length == 0)
			{
				ConsoleLog("Please set your Noesis Path in the settings below!");
				return;
			}

			ConsoleLog("Building local data...");

			// Build local data
			(new AltanaListConverter()).init();
			(new NoesisConverter()).init();

			ConsoleLog("Local data built!!");
			ConsoleLog("-----------------------------");
		}

        /// <summary>
        /// Basic Console Log method
        /// </summary>
        /// <param name="message"></param>
        private void ConsoleLog(string message)
        {
            Logger.Add(message);
        }

        /// <summary>
        /// My lush log timer lol Populates the log box, can reduce friction and 
        /// also means the log writes to a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogTimer_Tick(object sender, EventArgs e)
        {
			// Can't be fucked dealing with thread invokers
			// Just keep checking local var
			ToggleButtons();

            try
            {
                foreach (string entry in Logger.Get())
                {
                    //LogBox.SelectionStart = 0;
                    // LogBox.SelectionLength = 0;
                    LogBox.SelectedText = entry + Environment.NewLine;

                    // set the current caret position to the end
                    LogBox.SelectionStart = LogBox.Text.Length;

                    // scroll it automatically
                    LogBox.ScrollToCaret();
                }

                Logger.Clear();
            }
            catch { }
        }

        /// <summary>
        /// Send a key to a specific window title.
        /// Default to Noesis.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="wait"></param>
        /// <param name="window"></param>
        private void SendKey(string key, int wait = 300, string window = "Noesis")
        {
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
                ConsoleLog($"!!! (SendKey) Exception Thrown: {Ex.Message}");
            }
        }

		/// <summary>
		/// This will Copy the text to the clipboard and then paste it, as it's a thousand times faster than Sendkey
		/// </summary>
		/// <param name="text"></param>
		/// <param name="wait"></param>
		/// <param name="window"></param>
		private void SendText(string text, int wait = 300, string window = "Noesis")
		{
			Clipboard.SetText(text);
			SendKey("^v", wait, window);
		}

        /// <summary>
        /// Sends the tab key multiple times, hard coded to "Export Media" as that
        /// is usually the only window that needs this logic...
        /// </summary>
        /// <param name="count"></param>
        /// <param name="window"></param>
        private void SendKeyTabN(int count, int wait = 100, string window = "Export Media")
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
		private void SendKeyN(int count, string key, int wait = 100, string window = "Export Media")
		{
			for (int i = 1; i <= count; i++)
			{
				SendKey(key, wait, window);
			}
		}

        /// <summary>
        /// Stops the current active thread
        /// </summary>
        private void StopActiveThread()
        {
			if (ActiveExtractThread == null)
            {
                return;
            }

            ConsoleLog($">> Stopping active extraction...");
            ActiveExtractThread.Abort();
			ActiveThreadRunning = false;
			ConsoleLog(">> Active extract stopped!");
        }

		/// <summary>
		/// Can be called within the thread to abort itself.
		/// </summary>
		private void StopActiveThreadInternal()
		{
			ConsoleLog($">> Stopping thread internally...");
			Thread.CurrentThread.Abort();
			ActiveThreadRunning = false;
		}

        /// <summary>
        /// Wait for the active window to be said windowTitle, improves timing of send key flows.
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        private bool WaitForActiveWindow(string windowTitle, int waitInSeconds = 20, bool stopOnError = false)
        {
			// Will wait for a max of 30 seconds.
			for (int i = 0; i <= waitInSeconds; i++)
            {
				// ConsoleLog($"Active Window = {GetActiveWindowTitle()}");

				if (GetActiveWindowTitle() == windowTitle)
                {
                    return true;
                }

                Thread.Sleep(1000);
            }

            ConsoleLog($"!! Error: Could not detect the Window: {windowTitle} - Current active window: {GetActiveWindowTitle()}");

			// send escape a couple times
			SendKey("{ESCAPE}", 300, "");
			SendKey("{ESCAPE}", 300, "");
			SendKey("{ESCAPE}", 300, "");

			if (stopOnError)
			{
				StopActiveThreadInternal();
			}

            return false;
        }

		private bool WaitForActiveWindowCount(string windowTitle, int waitInSeconds = 20, int windowCount = 1, bool stopOnError = false)
		{
			// Will wait for a max of 30 seconds.
			for (int i = 0; i <= waitInSeconds; i++)
			{
				// ConsoleLog($"{windowTitle} count = {CountActiveWindows(windowTitle)}");

				if (CountActiveWindows(windowTitle) == windowCount)
				{
					return true;
				}

				Thread.Sleep(1000);
			}

			ConsoleLog($"!! Error: Could not find enough windows titled {windowTitle}");

			// send escape a couple times
			SendKey("{ESCAPE}", 300, "");
			SendKey("{ESCAPE}", 300, "");
			SendKey("{ESCAPE}", 300, "");

			if (stopOnError)
			{
				StopActiveThreadInternal();
			}

			return false;
		}

		/// <summary>
		/// Wait for the a process to start
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private bool WaitForProcess(string name, int waitInSeconds = 60, bool stopOnError = false)
		{
			int delay = waitInSeconds * 2;

			// Will wait for a max of 30 seconds.
			for (int i = 0; i <= delay; i++)
			{
				if (IsProcessRunning("Noesis"))
				{
					return true;
				}

				Thread.Sleep(400);
			}

			ConsoleLog($"!! Error: Could not detect the open process: {name}");

			if (stopOnError)
			{
				StopActiveThreadInternal();
			}

			return false;
		}

        /// <summary>
        /// https://stackoverflow.com/questions/115868/how-do-i-get-the-title-of-the-current-active-window-using-c
        /// </summary>
        /// <returns></returns>
        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

		private int CountActiveWindows(string windowTitle)
		{
			List<IntPtr> windows = new List<IntPtr>();
			EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
			{
				windows.Add(hWnd);
				return true;
			}, IntPtr.Zero);

			int count = 0;
			foreach (IntPtr hWnd in windows)
			{
				StringBuilder sb = new StringBuilder(256);
				if (GetWindowText(hWnd, sb, sb.Capacity) > 0 && sb.ToString() == windowTitle)
				{
					count++;
				}
			}

			return count;
		}

		/// <summary>
		/// Checks if Noesis is running, if not, start it!
		/// </summary>
		private void StartNoesis()
		{
			if (IsProcessRunning("Noesis"))
			{
				return;
			}

			// Start Noesis
			ConsoleLog("Starting Noesis...");
			Process.Start(Settings.GetSetting("PathNoesis"));

			// Wait for it to load
			WaitForProcess("Noesis");

			// When Noesis Loads, it auto routes to previous directory and file,
			// so we will add another delay here to let it do that...
			Thread.Sleep(5000);
			ConsoleLog(">> Noesis Ready!");
		}

		/// <summary>
		/// Checks if a process is running
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private bool IsProcessRunning(string name)
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

		/// -----------------------------------------------------------------------------------------
		/// GUI Buttons
		/// -----------------------------------------------------------------------------------------

		/// <summary>
		/// Discord for FF11 Modding
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FFXIModdingDiscordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://discord.gg/nHESRq3fHu");
		}

		/// <summary>
		/// Link to github
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GithubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://github.com/vekien/xidata");
		}

		private void OpenOutputFolder_Click(object sender, EventArgs e)
		{
			string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
			Process.Start(appDirectory);
		}

		private void OpenLogFile_Click(object sender, EventArgs e)
		{
			string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
			Process.Start($"{appDirectory}/log.txt");
		}

		/// <summary>
		/// Stop all extractions, aborts the current thread.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StopEverything_Click(object sender, EventArgs e)
        {
            StopActiveThread();
        }

        [STAThread]
        private void StartNpcExtract_Click(object sender, EventArgs e)
		{
            if (ActiveThreadRunning) { return; }

            ActiveExtractThread = new Thread(() => HandleNpcMainExtract());
            ActiveExtractThread.SetApartmentState(ApartmentState.STA);
            ActiveExtractThread.Start();

			StopEverything.Focus();
		}

		[STAThread]
		private void StartZoneExtract_Click(object sender, EventArgs e)
		{
			if (ActiveThreadRunning) { return; }

			ActiveExtractThread = new Thread(() => HandleZonesExtract());
			ActiveExtractThread.SetApartmentState(ApartmentState.STA);
			ActiveExtractThread.Start();

			StopEverything.Focus();
		}

		[STAThread]
		private void StartWeaponsExtract_Click(object sender, EventArgs e)
		{
			if (ActiveThreadRunning) { return; }

			ActiveExtractThread = new Thread(() => HandleWeaponsExtract());
			ActiveExtractThread.SetApartmentState(ApartmentState.STA);
			ActiveExtractThread.Start();

			StopEverything.Focus();
		}

		[STAThread]
		private void StartArmorExtract_Click(object sender, EventArgs e)
		{
			if (ActiveThreadRunning) { return; }

			ActiveExtractThread = new Thread(() => HandleArmorExtract());
			ActiveExtractThread.SetApartmentState(ApartmentState.STA);
			ActiveExtractThread.Start();

			StopEverything.Focus();
		}

		[STAThread]
		private void StartAnimationsExtract_Click(object sender, EventArgs e)
		{
			if (ActiveThreadRunning) { return; }

			ActiveExtractThread = new Thread(() => HandleAnimationsExtract());
			ActiveExtractThread.SetApartmentState(ApartmentState.STA);
			ActiveExtractThread.Start();

			StopEverything.Focus();
		}

		[STAThread]
		private void StartNpcCommonExtract_Click(object sender, EventArgs e)
		{
			if (ActiveThreadRunning) { return; }

			ActiveExtractThread = new Thread(() => HandleNpcLookExtract());
			ActiveExtractThread.SetApartmentState(ApartmentState.STA);
			ActiveExtractThread.Start();

			StopEverything.Focus();
		}

		[STAThread]
		private void StartZoneRoomsExtract_Click(object sender, EventArgs e)
		{
			if (ActiveThreadRunning) { return; }

			ActiveExtractThread = new Thread(() => HandleZonesRoomsExtract());
			ActiveExtractThread.SetApartmentState(ApartmentState.STA);
			ActiveExtractThread.Start();

			StopEverything.Focus();
		}

		/// <summary>
		/// Toggle the state of buttons based on if a thread is running or not.
		/// </summary>
		private void ToggleButtons()
        {
            bool buttonsEnabled = !ActiveThreadRunning;

			if (Settings.GetSetting("PathFFXI") == "" || Settings.GetSetting("PathNoesis") == "")
			{
				buttonsEnabled = false;
			}

			Button[] buttons =
			{
				StartZoneExtract,
				StartZoneRoomsExtract,
				StartArmorExtract,
				StartWeaponsExtract,
				StartNpcExtract,
				StartAnimationsExtract,
				StartNpcCommonExtract
			};

			foreach (Button button in buttons)
			{
				button.Enabled = buttonsEnabled;
				button.BackColor = buttonsEnabled ? Color.YellowGreen : Color.Gray;
			}
		}

		/// -----------------------------------------------------------------------------------------
		/// Escape Key Listener
		/// - This listens for "Escape" key
		/// It uses: https://github.com/gmamaladze/globalmousekeyhook
		/// -----------------------------------------------------------------------------------------

		private IKeyboardMouseEvents m_GlobalHook;
        private void HookSubscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }

        private void HookUnsubscribe()
        {
            // m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;

            // It is recommened to dispose it
            m_GlobalHook.Dispose();
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            string character = e.KeyChar.ToString();

            // fix some
            character = ((sbyte)e.KeyChar == 13) ? "ENTER" : character;
            character = ((sbyte)e.KeyChar == 32) ? "SPACE" : character;

            //ConsoleLog($"Key: {(sbyte)e.KeyChar} == {character}");

            // Escape
            if ((sbyte)e.KeyChar == 27)
            {
                //ConsoleLog($"[HOOK] (Escape Key detected!)");
                //StopActiveThread();
                return;
            }

			// A
			if ((sbyte)e.KeyChar == 97)
			{
				if (EnableStopHook.Checked)
				{
					ConsoleLog($"[HOOK] (A Key detected!)");
					StopActiveThread();
				}
				
				return;
			}
		}

		/// ----------------------------------------------------------------------------------------- 
        /// Settings
        /// -----------------------------------------------------------------------------------------

		private void PathFFXI_SelectPath(object sender, MouseEventArgs e)
		{
			var dialog = new FolderBrowserDialog();
            dialog.Description = "Browse To: SquareEnix\\FINAL FANTASY XI";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string selectedPath = dialog.SelectedPath;
				string checkFilename = $"{selectedPath}\\FFXiMain.dll";
				
				// Check noesis exists in the selected path
				if (File.Exists(checkFilename))
				{
					PathFFXI.Text = selectedPath;
					Settings.SaveSetting("PathFFXI", PathFFXI.Text);
                    ConsoleLog($"Saved FFXI Path: {selectedPath}");
				}
				else
				{
					ConsoleLog("Error: Could not detect FFXI Folder, make sure it is your SquareEnix\\FINAL FANTASY XI Folder");
				}
			}
		}

		private void PathNoesis_SelectPath(object sender, MouseEventArgs e)
		{
			var dialog = new FolderBrowserDialog();
			dialog.Description = "Browse To your Noesis Folder";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
                string selectedPath = dialog.SelectedPath;
                string checkFilename = $"{selectedPath}\\Noesis64.exe";

                // Check noesis exists in the selected path
				if (File.Exists(checkFilename))
                {
					PathNoesis.Text = checkFilename;
					Settings.SaveSetting("PathNoesis", checkFilename);
					ConsoleLog($"Saved Noesis: {checkFilename}");
				}
                else
                {
                    ConsoleLog("Error: Could not find Noesis64.exe in the selected path, try again.");
                }	
			}
		}

		/// -----------------------------------------------------------------------------------------
		/// Send Key Logic Flows
		/// -----------------------------------------------------------------------------------------

		private string ExtractSkeleton = "";

		private void FlowIntroduction(string flowName)
        {
			ConsoleLog($"[[ {flowName} ]]");
			ConsoleLog("Data will be exported as FBX with PNG textures.");
			ConsoleLog("This will take some time. Please try not stop it!");
			ConsoleLog("If it crashes or you do stop it, start again to continue.");
			ConsoleLog($"Time Now: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
			ConsoleLog("Starting...");
			ConsoleLog(" ");

			ActiveThreadRunning = true;
		}

        private void FlowFinished(string outputFolder)
        {
			// Stop thread, we're done
			ConsoleLog("[[ FINISHED ]]");
            ConsoleLog($"Saved all data to: {outputFolder}");

			ActiveThreadRunning = false;
		}

		/// <summary>
		/// The full SendKeys logic flow for handling a Noesis Export.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		/// <param name="args"></param>
		private void HandleNoesisExport(string source, string destination, string args, string race = "")
		{
			string ffxiPath = Settings.GetSetting("PathFFXI");
			string raceSkeleton = FF11Race.GetSkeleton(race);
			string raceSkeletonPath = $"{ffxiPath}\\ROM{raceSkeleton}.DAT";

			// Open a DAT
			SendKey("%f", 400);
			SendKey("o", 400);

			if (WaitForActiveWindow("Open"))
			{
				SendText(source, 400, "Open");
				SendKey("{ENTER}", 1000, "Open");

				// If the open window, opens again, it means this model has a skeleton
				if (GetActiveWindowTitle() == "Open")
				{
					if (string.IsNullOrEmpty(race))
					{
						SendKey("{ESCAPE}", 200, "");
						SendKey("{ESCAPE}", 200, "");
					} 
					else
					{
						SendText(raceSkeletonPath, 300, "Open");
						SendKey("{ENTER}", 1500, "Open");
						Thread.Sleep(1000);
					}
				}

				// Now open the Export Window
				SendKey("%f", 400);
				SendKey("e", 400);

				// Wait for the Export Media window to open
				if (WaitForActiveWindow("Export Media"))
				{
					// Destination
					SendKeyTabN(3);
					SendText(destination, 250, "Export Media");

					// Texture output type
					SendKeyTabN(3);

					// Loop down to png
					SendKeyN(21, "{DOWN}", 8);

					// Animation output type
					SendKeyTabN(1);

					// Loop down to noemultifbx
					SendKeyN(12, "{DOWN}", 8);

					// Advanced options
					SendKeyTabN(1);

					// Paste Noesis args
					SendText(args, 300, "Export Media");
					SendKey("{ENTER}", 1000, "Export Media");

					// If the open window, opens again, it means this model has a skeleton
					if (GetActiveWindowTitle() == "Open")
					{
						if (string.IsNullOrEmpty(race))
						{
							SendKey("{ESCAPE}", 200, "");
							SendKey("{ESCAPE}", 200, "");
						}
						else
						{
							SendText(raceSkeletonPath, 300, "Open");
							SendKey("{ENTER}", 1500, "Open");
							Thread.Sleep(1000);
						}
					}

					// The complete window is called Noesis
					// We wait a long time here because exports can have a lot of files
					if (WaitForActiveWindowCount("Noesis", 20, 2, true))
					{
						// close the Confirm and Export window
						Thread.Sleep(50);
						SendKey("{ESCAPE}", 300, "");
						SendKey("{ESCAPE}", 300, "");
						SendKey("{ESCAPE}", 300, "");
					}
				}
			}
		}

		private void HandleNoesisExportAnims(string source, string destination, string args, string race = "")
		{
			string ffxiPath = Settings.GetSetting("PathFFXI");
			string raceSkeleton = FF11Race.GetSkeleton(race);
			string raceSkeletonPath = $"{ffxiPath}\\ROM{raceSkeleton}.DAT";

			// Open a DAT
			SendKey("%f", 400);
			SendKey("o", 400);

			if (WaitForActiveWindow("Open"))
			{
				SendText(source, 400, "Open");
				SendKey("{ENTER}", 1500, "Open");

				// If there are 2 Noesis windows, it's usually an error.
				if (CountActiveWindows("Noesis") == 2)
				{
					SendKey("{ESCAPE}", 300, "");
					SendKey("{ESCAPE}", 300, "");

					return;
				}

				// Escape any popups
				SendKey("{ESCAPE}", 300, "");
				SendKey("{ESCAPE}", 300, "");
				SendKey("{ESCAPE}", 300, "");
				SendKey("{ESCAPE}", 300, "");

				// Now open the Export Window
				SendKey("%f", 400);
				SendKey("e", 400);

				// Wait for the Export Media window to open
				if (WaitForActiveWindow("Export Media"))
				{
					// Destination
					SendKeyTabN(3);
					SendText(destination, 250, "Export Media");

					// Animation output type
					SendKeyTabN(4);

					// Loop down to noemultifbx
					SendKeyN(12, "{DOWN}", 8);

					// Advanced options
					SendKeyTabN(1);

					// Paste Noesis args
					SendText(args, 300, "Export Media");
					SendKey("{ENTER}", 500, "Export Media");
					Thread.Sleep(1000);

					// If an "Open" modal popped up, it's likely skeleton, so we'll insert that
					if (GetActiveWindowTitle() == "Open")
					{
						SendText(raceSkeletonPath, 300, "Open");
						SendKey("{ENTER}", 500, "Open");
						Thread.Sleep(1000);
					}

					// --------------------------------------------
					// Export wil be starting now....
					// --------------------------------------------

					// The complete window is called Noesis
					// We wait a long time here because exports can have a lot of files
					if (WaitForActiveWindowCount("Noesis", 20, 2, true))
					{
						// close the Confirm and Export window
						SendKey("{ESCAPE}", 300, "");
						SendKey("{ESCAPE}", 300, "");
						SendKey("{ESCAPE}", 300, "");
					}
				}
			}
		}

		/// <summary>
		/// Run a Noesis ?cmode Command.
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="idleStateCode"></param>
		private void HandleNoesisCMode(string arguments, int idleStateCode = 0)
		{
			// Grab Noesis Path
			string noesisPath = Settings.GetSetting("PathNoesis");

			var proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = noesisPath,
					Arguments = $"?cmode {arguments}",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = false,
					WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
				}
			};

			proc.Start();

			// Run the command slightly different based on idle state codes.
			switch (idleStateCode)
			{
				default:
					ConsoleLog($"!! Error: Unknown idle state provided: {idleStateCode}");
					break;

				// normal, just wait for the process to complete
				case 0:
					proc.WaitForExit();
					break;

				// Look for an "Open" dialog and then close it.
				case 1:
					if (WaitForActiveWindow("Open", 10))
					{
						// close it
						SendKey("{ESCAPE}", 200, "Open");

						// Wait for command to finish
						proc.WaitForExit();
					}

					break;

				// Look for an "Open" dialog and enter in the skeleton
				case 2:
					Clipboard.SetText(ExtractSkeleton);

					// short wait
					if (WaitForActiveWindow("Open", 10))
					{
						// Paste in the dat and press Enter, then give it a bit of time to load (usually is fast)
						SendKey("^v", 300, "Open");
						SendKey("{ENTER}", 300, "Open");

						// Wait for command to finish
						proc.WaitForExit();
					}

					break;
			}
		}

        /// <summary>
        /// Extract all weapons
        /// </summary>
		private void HandleWeaponsExtract()
        {
            string jsonFile = $"ToolData\\weapons_1.json";
            string outputFolder = "ToolOutput_Weapons";
            string noesisArgs = NoesisArgsWeapons.Text.Trim();
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string ffxiPath = Settings.GetSetting("PathFFXI");
			FlowIntroduction("Weapons");

			// Load the JSON file into a string
			ConsoleLog($"Parsing JSON: {jsonFile}");
            string json = File.ReadAllText(jsonFile);
            JObject jsonObj = JObject.Parse(json);

            // Loop through each property in the JObject
            foreach (JProperty slotdata in jsonObj.Properties())
            {
                // eg: "Main", "Range", "Sub"
                string slotName = slotdata.Name;
                JObject slots = (JObject)slotdata.Value;

                // Loop through each item in the JArray
                foreach (JProperty typedata in slots.Properties())
                {
                    // eg: "Hand", "Dagger", "Sword" etc
                    string typeName = typedata.Name;
                    JArray weapons = (JArray)typedata.Value;

                    // if type is "None" SKIP
                    if (typeName == "None") { continue; }

                    // Loop through each item in the JArray
                    foreach (string item in weapons)
                    {
                        string[] itemData = item.Split('|');
                        string itemName = itemData[0];
                        string itemPath = itemData[1];

						// Set the path to the full name
						itemPath = $"{ffxiPath}\\ROM{itemPath}.DAT";

                        // Set the save output
						string saveTo = $"{outputFolder}\\{slotName}\\{typeName}\\{itemName}";
						Directory.CreateDirectory(saveTo);

						// Append the filename onto the save to
						saveTo = $"{appDirectory}{saveTo}\\{itemName}.fbx";

						// check if the file already exists, if so, then skip!
						if (File.Exists(saveTo))
						{
							continue;
						}

						// Build the Noesis cmode command
						string command = $"\"{itemPath}\" \"{saveTo}\" {noesisArgs}";
						ConsoleLog($"--- {slotName} - {typeName} - {itemName}");

						// Run the cmode command
						HandleNoesisCMode(command, 1);
                    }
                }
            }

			FlowFinished(outputFolder);
		}

		/// <summary>
		/// Extract all animations
		/// </summary>
		private void HandleAnimationsExtract()
        {
			string filename = $"ToolData\\anims_1.txt";
			string outputFolder = "ToolOutput_Animations";
			string noesisArgs = NoesisArgsAnimations.Text.Trim();
			string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string ffxiPath = Settings.GetSetting("PathFFXI");
			FlowIntroduction("Animations");
			StartNoesis();

			// Load the JSON
			string[] anims = File.ReadAllText(filename).Split('\n');

			// And we begin
			foreach (string animline in anims)
			{
				string[] animData = animline.Split('|');
				string race = animData[0];
				string type = animData[1];
				string name = animData[2];
				string path = animData[3];

				// set save path
				string saveTo = $"{outputFolder}\\{race}\\{type}\\{name}";
				Directory.CreateDirectory(saveTo);

				// Begin Noesis Export
				string sourceFile = $"{ffxiPath}\\ROM{path}.DAT";
				string destinationFile = $"{appDirectory}{saveTo}\\{name}.fbx";

				// skip if any files already exist
				bool fbxFilesFound = Directory.GetFiles($"{appDirectory}{saveTo}\\", "*.fbx").Length > 0;
				bool pngFilesFound = Directory.GetFiles($"{appDirectory}{saveTo}\\", "*.png").Length > 0;
				if (fbxFilesFound || pngFilesFound) { continue; }

				ConsoleLog($"-- Anim Export: {race} - {type} - {name}");

				// Handle the Noesis Export part!
				HandleNoesisExportAnims(sourceFile, destinationFile, noesisArgs, race);
			}

			FlowFinished(outputFolder);
		}

		/// <summary>
		/// Extract all NPCs
		/// </summary>
        private void HandleNpcMainExtract()
        {
			string jsonFile = $"ToolData\\npc_1.json";
			string outputFolder = "ToolOutput_NPC1";
			string noesisArgs = NoesisArgsNPC.Text.Trim();
			string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string ffxiPath = Settings.GetSetting("PathFFXI");
			int skipped = 0;
			FlowIntroduction("NPC Main");
			StartNoesis();

			// Load the JSON
			string jsonfile = File.ReadAllText(jsonFile);
			var data = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonfile);

			// And we begin
			foreach (var category in data)
			{
				string categoryName = category.Key;

				foreach (string npc in category.Value)
				{
					string[] npcData = npc.Split('|');
					string npcName = npcData[0];
					string npcPath = npcData[1];

					// set save path
					string saveTo = $"{outputFolder}\\{categoryName}\\{npcName}";
					Directory.CreateDirectory(saveTo);

					// Begin Noesis Export
					string sourceFile = $"{ffxiPath}\\ROM{npcPath}.DAT";
					string destinationFile = $"{appDirectory}{saveTo}\\{npcName}.fbx";

					// if destination file exists, skip
					if (File.Exists(destinationFile)) { skipped++; continue; }

					if (skipped > 0)
					{
						ConsoleLog($"-- Skipped {skipped} already existsing extracts.");
						skipped = 0;
					}

					ConsoleLog($"-- {categoryName}: {npcName}");

					// Handle the Noesis Export part!
					HandleNoesisExport(sourceFile, destinationFile, noesisArgs);
				}

			}

			FlowFinished(outputFolder);
		}

		/// <summary>
		/// Loop through all Look NPCs
		/// </summary>
        private void HandleNpcLookExtract()
        {
			string jsonFile = $"ToolData\\npc_3.json";
			string outputFolder = "ToolOutput_NPC2";
			string noesisArgs = NoesisArgsNPC.Text.Trim();
			string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
			int skipped = 0;
			FlowIntroduction("NPC Main");
			StartNoesis();

			// Load the JSON
			string jsonfile = File.ReadAllText(jsonFile);
			var data = JsonConvert.DeserializeObject<List<string>>(jsonfile);
			int total = data.Count;
			int count = 0;

			foreach (string npc in data)
			{
				count++;

				string[] npcData = npc.Split('|');
				string npcID = npcData[0];
				string npcName = npcData[1];
				string npcZone = npcData[2];
				string npcPath = npcData[3];

				// set save path
				string saveTo = $"{outputFolder}\\{npcZone}\\{npcID}_{npcName}";
				Directory.CreateDirectory(saveTo);

				// Begin Noesis Export
				string sourceFile = $"{appDirectory}{npcPath}";
				string destinationFile = $"{appDirectory}{saveTo}\\{npcID}_{npcName}.fbx";

				// if destination file exists, skip
				if (File.Exists(destinationFile)) { skipped++; continue; }

				if (skipped > 0)
				{
					ConsoleLog($"-- Skipped {skipped} already existsing extracts.");
					skipped = 0;
				}

				ConsoleLog($"-- ({count} / {total}) {npcZone}: ({npcID}) {npcName}");

				// Handle the Noesis Export part!
				HandleNoesisExport(sourceFile, destinationFile, noesisArgs);
			}

			FlowFinished(outputFolder);
		}
	}
}