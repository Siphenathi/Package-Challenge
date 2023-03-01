using System.Collections.Generic;
using System.Linq;
using Mobiquity.Packer.Model;
using Mobiquity.Packer.Utility;
using Mobiquity.Packer.Validation;

namespace Mobiquity.Packer
{
	public static class Packer
	{
		public static string Pack(string filePath)
		{
			var validationHandler = ValidationHandler(filePath);
			if (InputTextValidator.TextIsNotEmpty(validationHandler))
				throw new ApiException(validationHandler);
			return PackProcessorHandler(filePath);
		}

		public static string PackProcessorHandler(string filePath)
		{
			var allFileRows = FileProcessor.GetAllTextFileLines(filePath);
			var packageList = GetPackages(allFileRows);
			return ExecuteRules(packageList);
		}

		public static string ExecuteRules(IEnumerable<Package> packageList)
		{
			var result = string.Empty;
			foreach (var package in packageList)
			{
				switch (package.Items.Count)
				{
					case 0:
						result += "-\n";
						continue;
					case 1:
						result += $"{package.Items.FirstOrDefault()?.Index}\n";
						continue;
				}

				//var firstItem = GetFirstItem(package.Items);
				var firstItem = GetFirstItem(package.Items, package.PackageWeightLimit);
				var secondItem = GetSecondItem(package.Items, firstItem, package.PackageWeightLimit);
			}

			return result;
		}

		private static Item GetFirstItem(IEnumerable<Item> items, double packageWeightLimit)
		{
			var firstChosenItem = items.FirstOrDefault(x => x.Weight < packageWeightLimit);

			foreach (var item in items)
			{
				if (item.Weight > packageWeightLimit)
					continue;
				if (item.Cost > firstChosenItem?.Cost)
				{
					firstChosenItem = item;
					continue;
				}
				if (!Equals(item.Cost, firstChosenItem?.Cost)) continue;
				if (item.Weight < firstChosenItem.Weight)
					firstChosenItem = item;
			}
			return firstChosenItem;
		}

		private static Item GetFirstItem(IEnumerable<Item> items)
		{
			var firstChosenItem = items.FirstOrDefault();

			foreach (var item in items.Skip(1))
			{
				if (item.Cost > firstChosenItem?.Cost)
				{
					firstChosenItem = item;
					continue;
				}
				if (!Equals(item.Cost, firstChosenItem?.Cost)) continue;
				if (item.Weight < firstChosenItem.Weight)
					firstChosenItem = item;
			}
			return firstChosenItem;
		}

		private static Item GetSecondItem(IEnumerable<Item> items, Item firstItem, double packageWeightLimit)
		{
			var secondChosenItem = new Item
			{
				Cost = 0
			};

			foreach (var item in items)
			{
				if (item.Weight > packageWeightLimit)
					continue;
				if(Equals(item.Cost, firstItem.Cost) && Equals(item.Weight, firstItem.Weight))
					continue;
				if (!(item.Weight + firstItem.Weight <= packageWeightLimit)) continue;
				if (item.Cost + firstItem.Cost > firstItem.Cost + secondChosenItem.Cost)
					secondChosenItem = item;
			}
			return secondChosenItem;
		}

		//private static string 

		private static IEnumerable<Package> GetPackages(IEnumerable<string> allFileRows)
		{
			var packageList = new List<Package>();
			foreach (var row in allFileRows)
			{
				var packageColumns = PackageDataProcessor.GetPackageColumns(row);
				var packageWeightLimit = int.Parse(PackageDataProcessor.GetPackageWeightColumn(packageColumns));
				var package = new Package
				{
					PackageWeightLimit = packageWeightLimit,
					Items = new List<Item>()
				};
				var packageItemsColumn = PackageDataProcessor.GetPackageItemsColumn(packageColumns);
				var packageItems = PackageDataProcessor.RemoveEmptyRecords(PackageDataProcessor.GetPackageItems(packageItemsColumn));
				package.Items = SortItemsInDescendingOrder(GetItems(packageItems, packageWeightLimit)).ToList();
				packageList.Add(package);
			}
			return packageList;
		}

		private static IEnumerable<Item> GetItems(IEnumerable<string> packageItems, int packageWeightLimit)
		{
			var items = new List<Item>();
			if (items == null || !packageItems.Any())
				return items;

			foreach (var packageItem in packageItems)
			{
				var currentItemSet = PackageDataProcessor.GetPackageItemSet(packageItem);
				var weight = double.Parse(currentItemSet.ElementAtOrDefault(1));
				if (weight > packageWeightLimit)
					continue;

				items.Add(new Item
				{
					Index = int.Parse(currentItemSet.FirstOrDefault()),
					Weight = weight,
					Cost = double.Parse(currentItemSet.LastOrDefault().Substring(1, currentItemSet.LastOrDefault().Length - 1))
				});
			}
			return items;
		}

		private static IEnumerable<Item> SortItemsInDescendingOrder(IEnumerable<Item> items)
		{
			if (items == null || !items.Any())
				return new List<Item>();
			return items.OrderBy(x => x.Weight).Reverse();
		}

		private static string ValidationHandler(string filePath)
		{
			var fileValidationHandler = InputPathFileValidator.FileValidationHandler(filePath);
			if (InputTextValidator.TextIsNotEmpty(fileValidationHandler))
				return fileValidationHandler;
			var fileContentValidation =
				PackageFileContentValidator.ValidateFileContent(FileProcessor.GetAllTextFileLines(filePath));
			return InputTextValidator.TextIsNotEmpty(fileContentValidation) ? fileContentValidation : string.Empty;
		}
	}
}