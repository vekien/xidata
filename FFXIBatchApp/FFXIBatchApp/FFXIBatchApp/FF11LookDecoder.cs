using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace FFXIBatchApp
{
	internal class FF11LookDecoder
	{
		static Dictionary<string, int> structure = new Dictionary<string, int>()
		{
			{ "init", 4 }, // init 0x01 (this will check for 0x)
			{ "StyleLock", 2 },

			{ "Face", 2 },
			{ "Race", 2 },

			{ "Head", 4 },
			{ "Body", 4 },
			{ "Hands", 4 },
			{ "Legs", 4 },
			{ "Feet", 4 },
			{ "Main", 4 },
			{ "Sub", 4 },
			{ "Range", 4 }
		};

		public static Dictionary<string, int> DecodeLook(string look)
		{
			// The result will be the struct slot, eg: Hands and the ModelID
			Dictionary<string, int> npc = new Dictionary<string, int>();

			// Look pointer
			int pointer = 0;
			foreach (KeyValuePair<string, int> pair in structure)
			{
				string type = pair.Key;
				int length = pair.Value;

				// increment the pointer based on the init
				if (type == "init")
				{
					pointer += look.Substring(0, 2) == "0x" ? length : 2;
					continue;
				}

				// we don't care about this for NPCs
				if (type == "StyleLock")
				{
					pointer += length;
					continue;
				}

				// grab the look entry from the string
				string lookdata = look.Substring(pointer, length);

				// if this is face or race, just hexdec it and continue
				if (new[] { "Face", "Race" }.Contains(type))
				{
					npc[type] = Convert.ToInt32(lookdata, 16);
					pointer += length;
					continue;
				}

				// increment pointer
				pointer += length;

				// slot in
				npc[type] = GetModelIdFromLookData(lookdata);
			}

			return npc;
		}

		private static JObject jsonData;

		/// <summary>
		/// Allows preloading of the look table so it isn't done on every loop.
		/// </summary>
		public static void PreloadLookData()
		{
			string resourceName = "FFXIBatchApp.Resources.LookData.LookTable.json";

			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				jsonData = JObject.Parse(reader.ReadToEnd());
			}
		}

		/// <summary>
		/// Goes through the LookTable to find the Model ID, Path and File ID.
		/// </summary>
		/// <param name="race"></param>
		/// <param name="slot"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string[] GetModelDataFromModelId(string race, string slot, int id)
		{
			JArray gearSlotArray = (JArray)jsonData[race][slot];

			foreach (JObject modelData in gearSlotArray)
			{
				int modelId = (int)modelData["ModelID"];
				if (modelId == id)
				{
					string[] result =
					{
						(string)modelData["FileID"],
						(string)modelData["ModelID"],
						(string)modelData["Path"]
					};

					return result;
				}
			}

			return null;
		}

		/// <summary>
		/// The magic
		/// </summary>
		/// <param name="lookvalue"></param>
		/// <returns></returns>
		public static int GetModelIdFromLookData(string lookdata)
		{
			int a = int.Parse(lookdata.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
			int b = int.Parse(lookdata.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);

			int shift = b << 8;
			int combined = a + shift;
			int masked = combined & 0x0FFF;

			return masked;
		}

		/// <summary>
		/// Simple test to ensure it works.
		/// This is Sagheera in Lower Jeuno
		/// </summary>
		public static void Test()
		{
			Dictionary<string, int> test = DecodeLook("0100080487108720A0308740A050006000700000");

			foreach (KeyValuePair<string, int> pair in test)
			{
				string type = pair.Key;
				int value = pair.Value;

				ConsoleLog($"(lookdata) {type} = {value}");
			}
		}

		/// <summary>
		/// Basic Console Log method
		/// </summary>
		/// <param name="message"></param>
		private static void ConsoleLog(string message)
		{
			Logger.Add(message);
		}
	}
}
