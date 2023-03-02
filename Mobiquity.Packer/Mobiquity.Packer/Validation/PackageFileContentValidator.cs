using System;
using System.Collections.Generic;
using System.Linq;
using Mobiquity.Packer.Utility;

namespace Mobiquity.Packer.Validation
{
	public static class PackageFileContentValidator
	{
		public static string ValidateFileContent(
	IReadOnlyCollection<string> allFileRows)
		{
			if (allFileRows.Count < 1)
				return "Provided empty file";

			for (var rowCount = 0; rowCount < allFileRows.Count; rowCount++)
			{
				var currentRow = InputTextValidator.TrimText(allFileRows.ElementAt(rowCount));
				if (InputTextValidator.TextIsEmpty(currentRow))
					return $"Invalid package for row {rowCount + 1}";

				var currentPackageValidationHandler = PackageInputValidationHandler(currentRow);
				if (InputTextValidator.TextIsNotEmpty(currentPackageValidationHandler))
					return currentPackageValidationHandler;

				var currentPackageItemsValidationHandler = PackageItemsValidationHandler(currentRow, rowCount);
				if (InputTextValidator.TextIsNotEmpty(currentPackageItemsValidationHandler))
					return currentPackageItemsValidationHandler;
			}
			return string.Empty;
		}

		private static string PackageInputValidationHandler(string row)
		{
			var packageColumns = PackageDataProcessor.GetPackageColumns(row);
			if (packageColumns.Count() != 2)
				return $"Invalid package meta data for this row {row}";

			var packageWeight = PackageDataProcessor.GetPackageWeightColumn(packageColumns);
			if (!InputTextValidator.ValueIsTheInteger(packageWeight))
				return $"Invalid package weight limit for this row {row}";

			if (int.Parse(packageWeight) < 1 || int.Parse(packageWeight) > 100)
				return $"{packageWeight} is invalid package weight. Package weight must be less or equal to 100";
			return string.Empty;
		}

		private static string PackageItemsValidationHandler(string row, int rowCount)
		{
			var packageColumns = PackageDataProcessor.GetPackageColumns(row);
			var packageItemsColumn = PackageDataProcessor.GetPackageItemsColumn(packageColumns);
			var packageItems = PackageDataProcessor.RemoveEmptyRecords(PackageDataProcessor.GetPackageItems(packageItemsColumn));

			if (packageItems.Count() > 15)
				return $"Row {rowCount + 1} has more than 15 items";

			foreach (var currentItemSetRow in packageItems)
			{
				var currentItemSet = PackageDataProcessor.GetPackageItemSet(currentItemSetRow);
				if (currentItemSet.Count() != 3)
					return $"One of items in row {rowCount + 1} doesn't have 3 values (index, weight & cost)";

				if (!InputTextValidator.ValueIsTheInteger(currentItemSet.First()))
					return $"Invalid index number for itemSet ({currentItemSetRow}) in row {rowCount + 1}";

				if (!InputTextValidator.ValueIsDouble(currentItemSet.ElementAtOrDefault(1)))
					return $"Invalid weight for itemSet ({currentItemSetRow}) in row {rowCount + 1}";

				if(double.Parse(currentItemSet.ElementAtOrDefault(1)) < 1 || int.Parse(currentItemSet.ElementAtOrDefault(1)) > 100)
					return $"{currentItemSet.ElementAtOrDefault(1)} is invalid package weight in row {rowCount + 1}. Item weight must be less or equal to 100";

				var currentItemCost = InputTextValidator.TrimText(currentItemSet.LastOrDefault());
				var itemCost = currentItemCost.Substring(1, currentItemCost.Length - 1);
				if (!InputTextValidator.ValueIsTheInteger(itemCost))
					return $"Invalid cost for itemSet ({currentItemSetRow}) in row {rowCount + 1}";
				if(int.Parse(itemCost) < 1 || int.Parse(itemCost) > 100)
					return $"{itemCost} is invalid item cost in row {rowCount + 1}. Item cost must be less or equal to 100";
			}
			return string.Empty;
		}
	}
}