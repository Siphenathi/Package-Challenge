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
				var currentRow = allFileRows.ElementAt(rowCount).Trim();
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
			var packageColumns = GetPackageColumns(row);
			if (packageColumns.Length != 2)
				return $"Invalid package meta data for this row {row}";
			var packageWeight = GetPackageWeightColumn(packageColumns);
			if (!InputTextValidator.ValueIsTheInteger(packageWeight))
				return $"Invalid package weight limit for this row {row}";
			if (int.Parse(packageWeight) < 0 || int.Parse(packageWeight) > 100)
				return $"{packageWeight} is invalid package weight. Package weight must be less or equal to 100";
			return string.Empty;
		}

		private static string PackageItemsValidationHandler(string row, int rowCount)
		{
			var packageColumns = GetPackageColumns(row);
			var packageItemsColumn = GetPackageItemsColumn(packageColumns);
			var packageItems = GetPackageItems(packageItemsColumn);

			if (packageItems.Length > 15)
				return $"Row {rowCount + 1} has more than 15 items";

			foreach (var currentItemSetRow in packageItems)
			{
				var currentItemSet = GetPackageItemSet(currentItemSetRow);
				if (currentItemSet.Length != 3)
					return $"One of items in row {rowCount + 1} doesn't have 3 values (index, weight & cost)";
				if (!InputTextValidator.ValueIsTheInteger(currentItemSet.First()))
					return $"Invalid index number for itemSet {currentItemSetRow} in row {rowCount + 1}";
				if (!InputTextValidator.ValueIsDouble(currentItemSet[1]))
					return $"Invalid weight for itemSet {currentItemSetRow} in row {rowCount + 1}";
				var currentItemCost = currentItemSet.LastOrDefault();
				var itemCost = currentItemCost.Substring(1, currentItemCost.Length - 1);
				if (!InputTextValidator.ValueIsTheInteger(itemCost))
					return $"Invalid cost for itemSet {currentItemSetRow} in row {rowCount + 1}";
			}

			return string.Empty;
		}

		private static string GetPackageWeightColumn(IReadOnlyList<string> packageRowColumns)
		{
			return packageRowColumns[0].Trim();
		}

		private static string GetPackageItemsColumn(IReadOnlyList<string> packageRowColumns)
		{
			return packageRowColumns[1].Trim();
		}

		private static string[] GetPackageColumns(string row)
		{
			return SplitString(row, new[] { ':' });
		}

		private static string[] GetPackageItems(string row)
		{
			return SplitString(row, new[] { '(', ')', ' ' });
		}

		private static string[] GetPackageItemSet(string row)
		{
			return SplitString(row, new[] { ',' });
		}

		private static string[] SplitString(string text, char[] delimiter)
		{
			return text.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}