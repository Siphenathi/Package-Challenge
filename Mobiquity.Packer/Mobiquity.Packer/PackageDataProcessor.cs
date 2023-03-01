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

		public static int GetItemIndex(string item)
		{
			if (InputTextValidator.TextIsEmpty(item))
				return 0;
			var currentItemSet = GetPackageItemSet(item);
			return int.Parse(InputTextValidator.TrimText(currentItemSet.FirstOrDefault()) ?? string.Empty);
		}

		public static double GetItemWeight(string item)
		{
			if (InputTextValidator.TextIsEmpty(item))
				return 0;
			var currentItemSet = GetPackageItemSet(item);
			return double.Parse(InputTextValidator.TrimText(currentItemSet.ElementAtOrDefault(1)) ?? string.Empty);
		}

		public static double GetItemCost(string item)
		{
			if (InputTextValidator.TextIsEmpty(item))
				return 0;
			var currentItemSet = GetPackageItemSet(item);
			var lastRecord = InputTextValidator.TrimText(currentItemSet.LastOrDefault());
			return double.Parse(lastRecord.Substring(1, lastRecord.Length - 1));
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
