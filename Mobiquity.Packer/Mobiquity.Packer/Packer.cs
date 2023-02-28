using System;
using System.Collections.Generic;
using System.IO;
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
			return "";

			//Run constraints

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