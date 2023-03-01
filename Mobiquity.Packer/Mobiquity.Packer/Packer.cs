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
			if (TextIsNotEmpty(validationHandler))
				throw new ApiException(validationHandler);
			return PackProcessorHandler.HandlePacking(GetAllTextFileLines(filePath));
		}

		private static string ValidationHandler(string filePath)
		{
			var fileValidationHandler = InputPathFileValidator.FileValidationHandler(filePath);
			if (TextIsNotEmpty(fileValidationHandler))
				return fileValidationHandler;
			var fileContentValidation =
				PackageFileContentValidator.ValidateFileContent(GetAllTextFileLines(filePath));
			return TextIsNotEmpty(fileContentValidation) ? fileContentValidation : string.Empty;
		}

		private static bool TextIsNotEmpty(string text)
		{
			return InputTextValidator.TextIsNotEmpty(text);
		}

		private static string [] GetAllTextFileLines(string filePath)
		{
			return FileProcessor.GetAllTextFileLines(filePath);
		}
	}
}