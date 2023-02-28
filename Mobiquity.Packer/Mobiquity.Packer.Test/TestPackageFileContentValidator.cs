using FluentAssertions;
using Mobiquity.Packer.Validation;
using NUnit.Framework;

namespace Mobiquity.Packer.Test
{
	public class TestPackageFileContentValidator
	{
		[Test]
		public void ValidateFileContent_WhenCalledWithEmptyFile_ShouldReturnErrorMessage()
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var actual = PackageFileContentValidator.ValidateFileContent(GetEmptyFileContent());

			//--------------Assert-----------
			actual.Should().Be("Provided empty file");
		}

		[Test]
		public void ValidateFileContent_WhenCalledWithFileThatHasInvalidPackage_ShouldReturnErrorMessage()
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var actual = PackageFileContentValidator.ValidateFileContent(GetInvalidPackageRowContent(""));

			//--------------Assert-----------
			actual.Should().Be("Invalid package for row 1");
		}



		private static string[] GetEmptyFileContent()
		{
			return new string []{};
		}

		private static string[] GetInvalidPackageRowContent(string fileContent)
		{
			return new[] { fileContent };
		}
	}
}