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

		[TestCase(" ", "Invalid package for row 1")]
		[TestCase("33 ", "Invalid package meta data for this row 33")]
		[TestCase("dh : (1, 2.3, R45)  ", "Invalid package weight limit for this row dh : (1, 2.3, R45)")]
		[TestCase("150 : (1, 2.3, R45)  ", "150 is invalid package weight. Package weight must be less or equal to 100")]
		[TestCase("50 : (1) (3) (1) (2) (6) (1) (3) (1) (2) (6) (1) (3) (1) (2) (6) (3)",
		"Row 1 has more than 15 items")]
		[TestCase("75 : (1, 85.31)", "One of items in row 1 doesn't have 3 values (index, weight & cost)")]
		[TestCase("75 : (R, 85.31, R38) (1, 2.3, R45)", "Invalid index number for itemSet (R, 85.31, R38) in row 1")]
		[TestCase("75 : (1, 8K5, R38) (1, 2.3, R45)", "Invalid weight for itemSet (1, 8K5, R38) in row 1")]
		[TestCase("75 : (1, 85, KL) (1, 2.3, R45)", "Invalid cost for itemSet (1, 85, KL) in row 1")]

		public void ValidateFileContent_WhenCalledWithFileThatHasInvalidPackage_ShouldReturnErrorMessage(string fileContent, string errorMessage)
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var actual = PackageFileContentValidator.ValidateFileContent(GetInvalidPackageRowContent(fileContent));

			//--------------Assert-----------
			actual.Should().Be(errorMessage);
		}

		[TestCase("75 : (1, 85.31, R38) ")]
		[TestCase("75 : (1, 90.4, R38) (2, 20.3, R45)" )]
		[TestCase("75 : (1, 57.31, R38) (1, 95.4, R38) (2, 33.3, R45)")]
		public void ValidateFileContent_WhenCalledWithFileThatHasValidPackage_ShouldReturnNoErrorMessage(string fileContent)
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var actual = PackageFileContentValidator.ValidateFileContent(GetInvalidPackageRowContent(fileContent));

			//--------------Assert-----------
			actual.Should().BeEmpty();
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