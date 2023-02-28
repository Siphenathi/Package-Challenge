using System;
using System.Collections.Generic;
using System.Linq;
using Mobiquity.Packer.Utility;

namespace Mobiquity.Packer
{
	public class PackageDataProcessor
	{
		public static string GetPackageWeightColumn(IEnumerable<string> packageRowColumns)
		{
			return TrimText(packageRowColumns.ElementAtOrDefault(0));
		}

		public static string GetPackageItemsColumn(IEnumerable<string> packageRowColumns)
		{
			return TrimText(packageRowColumns.ElementAtOrDefault(1));
		}

		public static IEnumerable<string> RemoveEmptyRecords(IEnumerable<string> list)
		{
			return list.Where(InputTextValidator.TextIsNotEmpty).ToList();
		}

		public static string TrimText(string text)
		{
			return InputTextValidator.TextIsEmpty(text) ? text : text.Trim();
		}

		public static IEnumerable<string> GetPackageColumns(string row) => SplitString(row, new[] { ':' });

		public static IEnumerable<string> GetPackageItems(string row) => SplitString(row, new[] { '(', ')' });

		public static IEnumerable<string> GetPackageItemSet(string row) => SplitString(row, new[] { ',' });

		public static IEnumerable<string> SplitString(string text, char[] delimiter)
		{
			return text.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
