using System.IO;

namespace Mobiquity.Packer.Utility
{
	public class FileProcessor
	{
		public static bool FileDoesNotExist(string filePath)
		{
			return !File.Exists(filePath);
		}

		public static string[] GetAllTextFileLines(string filePath)
		{
			return File.ReadAllLines(filePath);
		}

		public static string GetFileName(string filePath)
		{
			return Path.GetFileNameWithoutExtension(filePath);
		}
	}
}