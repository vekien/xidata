using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;

namespace xidata_v2.source
{
	internal class Extractor
	{
		private static readonly string DirectoryOutput = $"{Folders.Root()}\\output";

		private static string XiContentType;
		private static string XiNoesisArgs;
		private static int XiNoesisLimit;

		private static BackgroundWorker ExtractWorker;
		private static List<string> ContentData;
		private static bool IsEnabled = false;

		public static void Init(string ct, string na, int nl)
		{
			XiContentType = ct;
			XiNoesisArgs = na;
			XiNoesisLimit = nl;
		}

		/// <summary>
		/// Start the background worker
		/// </summary>
		public static void Start()
		{
			Instances.MainWindow.ButtonStartExtract.Disable();
			Instances.MainWindow.LabelRunning.Visibility = Visibility.Visible;

			// Grab content
			ContentData = Data.GetDataForContentType(XiContentType);

			// Create background worker
			ExtractWorker = new()
			{
				WorkerSupportsCancellation = true,
				WorkerReportsProgress = true
			};

			ExtractWorker.DoWork += (sender, e) =>
			{
				IsEnabled = true;

				// Wait for Noesis to open
				while (!Auto.IsNoesisOpen)
				{
					Logger.Add("- Waiting for Noesis to be ready...");
					Thread.Sleep(2500);
				}

				// Start Noesis
				Logger.Add("Starting Extraction...");

				// Switch based on content type
				switch(XiContentType)
				{
					case "Animations":
						ExtractAnimations();
						break;
				}

				Logger.Add($"Finished: {XiContentType}");
			};

			ExtractWorker.RunWorkerCompleted += (sender, e) =>
			{
				Stop();
			};

			ExtractWorker.RunWorkerAsync();
		}

		/// <summary>
		/// Stop the background worker
		/// </summary>
		public static void Stop()
		{
			IsEnabled = false;

			ExtractWorker.CancelAsync();
			ExtractWorker.Dispose();

			Instances.MainWindow.ButtonStartExtract.Enable();
			Instances.MainWindow.LabelRunning.Visibility = Visibility.Hidden;
		}

		/// <summary>
		/// Perform the data extract for animations
		/// </summary>
		private static void ExtractAnimations()
		{
			int count = 0;
			int total = XiNoesisLimit > 0 ? XiNoesisLimit : ContentData.Count;

			Logger.Add($"Starting: {total} Animations extract...");
			Logger.Space();

			foreach (string anim in ContentData)
			{
				

				if (!IsEnabled) return;

				try
				{
					// Grab anim data
					string[] values = anim.Split('|');
					string xi_filename = values[0];
					string xi_category = values[1];
					string xi_dat = values[2];
					string xi_name = values[8];
					string xi_name_clean = values[9];
					string xi_race_name = values[11];
					string xi_race_skeleton = values[14];

					// Save data
					string out_folder = $"{DirectoryOutput}\\animations\\{xi_race_name}\\{xi_category}\\{xi_name_clean}";
					string out_filename = $"{out_folder}\\{xi_name_clean}.fbx";

					// Source data
					string source_filename = $"{Settings.GetFFxiPath()}\\{xi_dat}";
					string source_skeleton = $"{Settings.GetFFxiPath()}\\{xi_race_skeleton}";

					if (!Directory.Exists(out_folder))
					{
						Directory.CreateDirectory(out_folder);
					}

					// Check if anything has already been extracted to this folder, if so, we skip!
					bool fbx_found = Directory.GetFiles(out_folder, "*.fbx").Length > 0;
					bool png_found = Directory.GetFiles(out_folder, "*.png").Length > 0;

					if (fbx_found || png_found) 
					{
						Logger.Add($"- {count}/{total} Skipped (Exists): {xi_race_name} {xi_category} {xi_name_clean}");
						continue; 
					}

					count++;
					Logger.Add($"- {count}/{total} Processing: {xi_race_name} {xi_category} {xi_name_clean} :: {xi_dat}");

					// Focus Noesis
					Auto.FocusNoesis();

					// Begin extract send key logic.
					Auto.SendKey("%f");
					Auto.SendKey("o");

					if (Auto.WaitForActiveWindow("Open"))
					{
						if (!IsEnabled) return;

						Thread.Sleep(500);
						Auto.SendText(source_filename, 0, "Open");

						Auto.SendKey("{ENTER}", 0, "Open");
						Thread.Sleep(500);

						if (!IsEnabled) return;

						// If there are 2 Noesis windows, it's usually an error.
						if (Auto.CountActiveWindows("Noesis") == 2)
						{
							Logger.Add($"-- Error: Could not process {xi_dat} as the preview error showed up. Skipping!");
							Auto.SendKeyN(4, "{ESCAPE}", 200, "");
							continue;
						}

						// Escape any popups as we'll handle the skeleton later
						Auto.SendKeyN(4, "{ESCAPE}", 200, "");

						// Now open the Export Window
						Auto.SendKey("%f");
						Auto.SendKey("e");

						if (!IsEnabled) return;

						// Wait for the Export Media window to open
						if (Auto.WaitForActiveWindow("Export Media"))
						{
							if (!IsEnabled) return;

							// Destination
							Auto.SendKeyTabN(3);
							Auto.SendText(out_filename, 0, "Export Media");

							// Animation output type
							Auto.SendKeyTabN(4);

							// Loop down to noemultifbx
							Auto.SendKeyN(12, "{DOWN}");

							// Advanced options
							Auto.SendKeyTabN(1);

							if (!IsEnabled) return;

							// Paste Noesis args
							Auto.SendText(XiNoesisArgs, 0, "Export Media");
							Auto.SendKey("{ENTER}", 0, "Export Media");
							Thread.Sleep(500);

							if (!IsEnabled) return;

							// If an "Open" modal popped up, it's the skeleton, so we'll insert that
							if (Auto.GetActiveWindowTitle() == "Open")
							{
								Auto.SendText(source_skeleton, 0, "Open");
								Auto.SendKey("{ENTER}", 0, "Open");
								Thread.Sleep(500);
							}

							if (!IsEnabled) return;

							// The complete window is called Noesis
							// We wait a long time here because exports can have a lot of files
							if (Auto.WaitForActiveWindowCount("Noesis", 2))
							{
								Auto.SendKeyN(4, "{ESCAPE}", 200, "");
							}
						}
					}

					Thread.Sleep(250);
				}
				catch (Exception ex)
				{
					Logger.Add($"Error: {ex.Message}");
				}

				// Limit check
				if (XiNoesisLimit > 0 && count >= XiNoesisLimit)
				{
					break;
				}
			}
		}
	}
}
