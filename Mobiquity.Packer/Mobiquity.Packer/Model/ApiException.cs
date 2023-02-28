using System;

namespace Mobiquity.Packer.Model
{
	public class ApiException : Exception
	{
		public ApiException(string message) : base(message)
		{
		}
	}
}