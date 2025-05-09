using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Threading;

namespace xidata_v2.source
{
    class Logger
    {
		// Where to output the log
		private const int LOG_MAX_SIZE = 2000;
		private const int LOG_MAX_SIZE_FILE = 8000;

		readonly static string logFilename = $"{Folders.Root()}\\app.log";
		readonly static string exceptionsFilename = $"{Folders.Root()}\\exceptions.log";

		// List of log lines
		private static readonly List<string> lines = [];
		private static readonly List<string> linesRecent = [];
		private static readonly List<string> linesToLogs = [];

		/// <summary>
		/// Initialises
		/// </summary>
		public static void Init()
		{
			// Setup timer to write to file
			DispatcherTimer timer = new()
			{
				Interval = TimeSpan.FromMilliseconds(1000)
			};

			timer.Tick += (sender, e) => {
				// grab recorded log lines and clear the list
				List<string> lines = new(linesToLogs);
				linesToLogs.Clear();

				if (lines.Count > 0)
				{
					try
					{
						File.AppendAllLines(logFilename, lines);
					}
					catch (Exception ex)
					{
						Add($"Failed to write logs: {ex.Message}");
					}
				}

				// Truncate
				TruncateLogFile();
			};

			timer.Start();
		}

		/// <summary>
		/// Add new text to the log lines
		/// </summary>
		public static void Add(string text)
		{
			text = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), text);

			// add to lists
			lines.Add(text);
			linesRecent.Add(text);
			linesToLogs.Add(text);
		}

		/// <summary>
		/// Add an array of text lines to the log
		/// </summary>
		/// <param name="texts"></param>
		public static void Add(string[] texts)
		{
			foreach (string text in texts)
			{
				Add(text);
			}
		}

		/// <summary>
		/// Add a simple new line space to the log
		/// </summary>
		public static void Space()
		{
			Add(" ");
		}

		/// <summary>
		/// Log exceptions
		/// </summary>
		public static void Exception(Exception exception)
		{
			// write to file
			string text = $"!!!  EXCEPTION !!!\n{exception}\n\n";

			// add to lists
			lines.Add(text);
			linesRecent.Add(text);
			linesToLogs.Add(text);
			Space();

			if (exception.StackTrace != null)
			{
				text += $"\n\nStack Trace:\n{exception.StackTrace}";
			}

			text += "\n\n";

			try
			{
				File.AppendAllText(exceptionsFilename, text);
			}
			catch
			{
				// ignore
			}
		}

		/// <summary>
		/// Truncate the logs
		/// </summary>
		private static void TruncateLogLines()
		{
			// truncate the log to 1000 lines to prevent the log window from crashing
			if (lines.Count > LOG_MAX_SIZE)
			{
				lines.RemoveRange(0, lines.Count - LOG_MAX_SIZE);
			}
		}

		/// <summary>
		/// This will truncate the log file to prevent massive log pastes.
		/// </summary>
		public static void TruncateLogFile()
		{
			if (!File.Exists(logFilename))
			{
				return;
			}

			// read all lines from the file
			List<string> filelines = File.ReadAllLines(logFilename).ToList();

			// don't do anything if we're not above max lines
			if (filelines.Count < LOG_MAX_SIZE_FILE)
			{
				return;
			}

			// if there are more than maxLines lines, delete lines from the beginning of the list
			while (filelines.Count > LOG_MAX_SIZE_FILE)
			{
				filelines.RemoveAt(0);
			}

			// insert the new line of text at the beginning of the list
			filelines.Insert(0, "[File Truncated to " + LOG_MAX_SIZE_FILE + " lines]");

			// write the updated lines to the file
			File.WriteAllLines(logFilename, filelines);
		}

		/// <summary>
		/// Get all log lines
		/// </summary>
		/// <returns></returns>
		public static List<string> Get()
		{
			return [.. lines];
		}

		/// <summary>
		/// Get the most recent log lines and clear out the existing.
		/// </summary>
		/// <returns></returns>
		public static List<string> GetRecent()
		{
			List<string> recent = new(linesRecent);
			linesRecent.Clear();
			return recent;
		}

		/// <summary>
		/// Clear all log lines
		/// </summary>
		public static void Clear()
		{
			lines.Clear();
		}
	}
}
