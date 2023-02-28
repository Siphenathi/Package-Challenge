using System;

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
			return int.TryParse(text, out _);
		}

		public static bool ValueIsDouble(string text)
		{
			return double.TryParse(text, out _);
		}
	}
}