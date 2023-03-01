using System.Collections.Generic;
using System.Linq;
using Mobiquity.Packer.Utility;

namespace Mobiquity.Packer
{
	public static class PackProcessorHandler
	{
		public static string HandlePacking(IEnumerable<string> allFileRows)
		{
			var results = string.Empty;
			foreach (var row in allFileRows)
			{
				var packageColumns = PackageDataProcessor.GetPackageColumns(row);
				var packageWeightLimit = int.Parse(PackageDataProcessor.GetPackageWeightColumn(packageColumns));
				var packageItemsColumn = PackageDataProcessor.GetPackageItemsColumn(packageColumns);
				var packageItems =
					PackageDataProcessor.RemoveEmptyRecords(PackageDataProcessor.GetPackageItems(packageItemsColumn));
				results += GetItemsHandler(packageItems, packageWeightLimit);
			}
			return results;
		}

		private static string GetItemsHandler(IEnumerable<string> packageItems, int packageWeightLimit)
		{
			if (packageItems.Count() == 1)
			{
				return GetItemWeight(packageItems.FirstOrDefault()) > packageWeightLimit ?
					"-\n" : $"{GetItemIndex(packageItems.FirstOrDefault())}\n";
			}
			var firstItem = GetFirstItemWithHigherCost(packageItems, packageWeightLimit);
			if (TextIsEmpty(firstItem))
				return "-\n";

			var secondItem = GetSecondWithHigherCost(packageItems, firstItem, packageWeightLimit);
			if (TextIsEmpty(secondItem))
				return $"{GetItemIndex(firstItem)}\n";

			return FirstItemIsSmallerThanSecondItem(GetItemIndex(firstItem), GetItemIndex(secondItem)) ?
				$"{GetItemIndex(firstItem)}, {GetItemIndex(secondItem)}\n" :
				$"{GetItemIndex(secondItem)}, {GetItemIndex(firstItem)}\n";
		}

		public static string GetFirstItemWithHigherCost(IEnumerable<string> items, int packageWeightLimit)
		{
			var chosenItem = items.FirstOrDefault(item => GetItemWeight(item) <= packageWeightLimit);
			foreach (var item in items)
			{
				var itemWeight = GetItemWeight(item);
				var itemCost = GetItemCost(item);
				var chosenItemWeight = GetItemWeight(chosenItem);
				var chosenItemCost = GetItemCost(chosenItem);

				if (itemWeight > packageWeightLimit)
					continue;
				if (Equals(itemCost, chosenItemCost) && Equals(itemWeight, chosenItemWeight))
					continue;
				if (itemCost > chosenItemCost)
				{
					chosenItem = item;
					continue;
				}
				if (!Equals(itemCost, chosenItemCost)) continue;
				if (itemWeight < chosenItemWeight)
					chosenItem = item;
			}
			return chosenItem;
		}

		public static string GetSecondWithHigherCost(IEnumerable<string> items, string firstItem, int packageWeightLimit)
		{
			var chosenItem = string.Empty;

			foreach (var currentItem in items)
			{
				var currentItemWeight = GetItemWeight(currentItem);
				var currentItemCost = GetItemCost(currentItem);
				var chosenItemCost = GetItemCost(chosenItem);
				var firstItemCost = GetItemCost(firstItem);
				var firstItemWeight = GetItemWeight(firstItem);

				if (currentItemWeight > packageWeightLimit)
					continue;
				if (Equals(currentItemCost, firstItemCost) && Equals(currentItemWeight, firstItemWeight))
					continue;
				if (!(currentItemWeight + firstItemWeight <= packageWeightLimit)) continue;
				if (currentItemCost + firstItemCost > firstItemCost + chosenItemCost)
					chosenItem = currentItem;
			}
			return chosenItem;
		}

		private static bool FirstItemIsSmallerThanSecondItem(int firstItem, int secondItem)
		{
			return firstItem < secondItem;
		}

		private static int GetItemIndex(string item)
		{
			return PackageDataProcessor.GetItemIndex(item);
		}

		private static double GetItemWeight(string item)
		{
			return PackageDataProcessor.GetItemWeight(item);
		}

		private static double GetItemCost(string item)
		{
			return PackageDataProcessor.GetItemCost(item);
		}

		private static bool TextIsEmpty(string text)
		{
			return InputTextValidator.TextIsEmpty(text);
		}
	}
}
