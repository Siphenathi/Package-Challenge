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

			var allFileRows = FileProcessor.GetAllTextFileLines(filePath);
			var packageList = new List<Package>();

			for (var rowCount = 0; rowCount < allFileRows.Length; rowCount++)
			{
				var packageColumns = PackageDataProcessor.GetPackageColumns(allFileRows[rowCount]);
				var package = new Package
				{
					PackageLimit = int.Parse(PackageDataProcessor.GetPackageWeightColumn(packageColumns)),
					Items = new List<Item>()
				};
				var packageItemsColumn = PackageDataProcessor.GetPackageItemsColumn(packageColumns);
				var packageItems = PackageDataProcessor.RemoveEmptyRecords(PackageDataProcessor.GetPackageItems(packageItemsColumn));

				var items = new List<Item>();

				foreach (var packageItem in packageItems)
				{
					var currentItemSet = PackageDataProcessor.GetPackageItemSet(packageItem);
					items.Add(new Item
					{
						Index = int.Parse(currentItemSet.FirstOrDefault()),
						Weight = double.Parse(currentItemSet.ElementAtOrDefault(1)),
						Cost = double.Parse(currentItemSet.LastOrDefault().Substring(1, currentItemSet.LastOrDefault().Length - 1))
					});
				}

				package.Items = SortItemsInDescendingOrder(items).ToList();
				packageList.Add(package);
			}

			return "";
		}

		private static string PackProcessorHandler(string filePath)
		{
			var allFileRows = FileProcessor.GetAllTextFileLines(filePath);
			var packageList = GetPackages(allFileRows);

			return "";

		}

		private static IEnumerable<Package> GetPackages(IEnumerable<string> allFileRows)
		{
			var packageList = new List<Package>();

			foreach (var row in allFileRows)
			{
				var packageColumns = PackageDataProcessor.GetPackageColumns(row);
				var package = new Package
				{
					PackageLimit = int.Parse(PackageDataProcessor.GetPackageWeightColumn(packageColumns)),
					Items = new List<Item>()
				};
				var packageItemsColumn = PackageDataProcessor.GetPackageItemsColumn(packageColumns);
				var packageItems = PackageDataProcessor.RemoveEmptyRecords(PackageDataProcessor.GetPackageItems(packageItemsColumn));
				package.Items = SortItemsInDescendingOrder(GetItems(packageItems)).ToList();
				packageList.Add(package);
			}
			return packageList;
		}

		private static IEnumerable<Item> GetItems(IEnumerable<string> packageItems)
		{
			var items = new List<Item>();
			if (items == null || !packageItems.Any())
				return items;

			foreach (var packageItem in packageItems)
			{
				var currentItemSet = PackageDataProcessor.GetPackageItemSet(packageItem);
				items.Add(new Item
				{
					Index = int.Parse(currentItemSet.FirstOrDefault()),
					Weight = double.Parse(currentItemSet.ElementAtOrDefault(1)),
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