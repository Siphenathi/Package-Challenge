namespace Mobiquity.Packer.Utility
{
	public class InputTextValidator
	{
		public static bool TextIsEmpty(string text)
		{
			return string.IsNullOrWhiteSpace(text);
		}

		public static bool TextIsNotEmpty(string text)
		{
			return !string.IsNullOrWhiteSpace(text);
		}

		public static bool ValueIsTheInteger(string text)
		{
			return int.TryParse(text.Trim(), out _);
		}

		public static bool ValueIsDouble(string text)
		{
			return double.TryParse(text.Trim(), out _);
		}

		public static string TrimText(string text)
		{
			return TextIsEmpty(text) ? string.Empty : text.Trim();
		}
	}
}