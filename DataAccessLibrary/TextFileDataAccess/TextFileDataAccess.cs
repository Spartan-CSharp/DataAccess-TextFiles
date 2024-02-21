using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataAccessLibrary.TextFileDataAccess
{
	internal static class TextFileDataAccess
	{
		internal static List<string> ReadAllCSVEntries(string filePath)
		{
			List<string> output = new List<string>();

			if (File.Exists(filePath))
			{
				output = File.ReadAllLines(filePath).ToList();
			}

			return output;
		}

		internal static void WriteAllCSVEntries(string filePath, List<string> entries)
		{
			File.WriteAllLines(filePath, entries);
		}
	}
}
