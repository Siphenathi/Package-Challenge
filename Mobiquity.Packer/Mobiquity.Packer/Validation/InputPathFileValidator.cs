using Mobiquity.Packer.Utility;

namespace Mobiquity.Packer.Validation
{
	public static class InputPathFileValidator
	{
		public static string FileValidationHandler(string filePath)
		{
			if (InputTextValidator.TextIsEmpty(filePath))
				return "You entered invalid file path.";
			return FileProcessor.FileDoesNotExist(filePath) ?
				$"No file found in this directory '{filePath}'" :
				string.Empty;
		}
	}
}