using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mobiquity.Packer.Model;

namespace Mobiquity.Packer
{
	public static class Packer
	{
		public static string Pack(string filePath)
		{
			var validationHandler = ValidationHandler(filePath);
			if (TextIsNotEmpty(validationHandler))
				throw new ApiException(validationHandler);
			return "";

			//Run constraints

		}

		private static string ValidationHandler(string filePath)
		{
			var fileValidationHandler = FileValidationHandler(filePath);
			if (TextIsNotEmpty(fileValidationHandler))
				return fileValidationHandler;
			var fileContentValidation =
				PackageFileContentValidationHandler(GetFileName(filePath), GetAllTextFileLines(filePath));
			return TextIsNotEmpty(fileContentValidation) ? fileContentValidation : string.Empty;
		}

		private static string FileValidationHandler(string filePath)
		{
			if (TextIsEmpty(filePath))
				return "You entered invalid file path.";
			return FileDoesNotExist(filePath) ? 
				$"No file found in this directory '{filePath}'" : 
				string.Empty;
		}

		private static string PackageFileContentValidationHandler(string fileName,
			IReadOnlyCollection<string> allFileRows)
		{
			if (allFileRows.Count < 1)
				return $"{fileName} file is empty";

			for (var rowCount = 0; rowCount < allFileRows.Count; rowCount++)
			{
				var currentRow = allFileRows.ElementAt(rowCount).Trim();
				if (TextIsEmpty(currentRow))
					return $"Package data for row {rowCount + 1} is empty";

				var currentPackageValidationHandler = PackageInputValidationHandler(currentRow);
				if (TextIsNotEmpty(currentPackageValidationHandler))
					return currentPackageValidationHandler;

				var currentPackageItemsValidationHandler = PackageItemsValidationHandler(currentRow, rowCount);
				if (TextIsNotEmpty(currentPackageItemsValidationHandler))
					return currentPackageItemsValidationHandler;
			}
			return string.Empty;
		}

		private static string PackageInputValidationHandler(string row)
		{
			var packageColumns = GetPackageColumns(row);
			if (packageColumns.Length != 2)
				return $"Invalid package data for this row {row}";
			var packageWeight = GetPackageWeightColumn(packageColumns);
			if(!ValueIsTheInteger(packageWeight)) 
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
				if (!ValueIsTheInteger(currentItemSet.First()))
					return $"Invalid index number for itemSet {currentItemSetRow} in row {rowCount + 1}";
				if (!ValueIsDouble(currentItemSet[1]))
					return $"Invalid weight for itemSet {currentItemSetRow} in row {rowCount + 1}";
				var currentItemCost = currentItemSet.LastOrDefault();
				var itemCost = currentItemCost.Substring(1, currentItemCost.Length - 1);
				if (!ValueIsTheInteger(itemCost))
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

		private static bool ValueIsTheInteger(string text)
		{
			return int.TryParse(text, out _);
		}

		private static bool ValueIsDouble(string text)
		{
			return double.TryParse(text, out _);
		}

		private static string [] GetPackageColumns(string row)
		{
			return SplitString(row, new[] { ':'});
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

		private static bool TextIsEmpty(string text)
		{
			return string.IsNullOrWhiteSpace(text);
		}

		private static bool TextIsNotEmpty(string text)
		{
			return !string.IsNullOrWhiteSpace(text);
		}

		private static bool FileDoesNotExist(string filePath)
		{
			return !File.Exists(filePath);
		}

		private static string[] GetAllTextFileLines(string filePath)
		{
			return File.ReadAllLines(filePath);
		}

		private static string GetFileName(string filePath)
		{
			return Path.GetFileNameWithoutExtension(filePath);
		}
	}
}