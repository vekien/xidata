using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FFXIBatchApp
{
    static class Logger
	{
        private static List<string> lines = new List<string>();

        //
        // Add to log
        //
        public static void Add(string text)
        {
            // create log entry
            text = String.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), text);
            lines.Add(text);

			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
			File.AppendAllText(filePath, text + Environment.NewLine);
		}

        public static List<String> Get()
        {
            return lines.ToList();
        }

        public static void Clear()
        {
            lines.Clear();
        }
    }
}
