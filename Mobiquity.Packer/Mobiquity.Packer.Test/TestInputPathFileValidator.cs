using FluentAssertions;
using Mobiquity.Packer.Model;
using Mobiquity.Packer.Validation;
using NUnit.Framework;

namespace Mobiquity.Packer.Test
{
	public class TestInputPathFileValidator
	{
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void Pack_WhenCalledWithInvalidInput_ShouldReturnErrorMessage(string filePath)
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var actual  = InputPathFileValidator.FileValidationHandler(filePath);

			//--------------Assert-----------
			actual.Should().Be("You entered invalid file path.");
		}

		[TestCase("de3f3fg3g")]
		[TestCase("c:jjdd\\jejdjd\\geg5")]
		[TestCase("C:\\Users\\Siphenathi\\Documents\\Dev-Time\\Personal-Project\\UserCaptureSystem")]
		public void Pack_WhenCalledWithNonExistingFilePath_ShouldThrowApiException(string filePath)
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var actual = InputPathFileValidator.FileValidationHandler(filePath);

			//--------------Assert-----------
			actual.Should().Contain("No file found in this directory");
		}

		//NB : This will fail due to file path
		[TestCase("C:\\Users\\Siphenathi\\Desktop\\input1.txt")]
		[TestCase("C:\\Users\\Siphenathi\\Desktop\\input2.txt")]
		[TestCase("C:\\Users\\Siphenathi\\Desktop\\input3.txt")]
		public void Pack_WhenCalledWithExistingFilePath_ShouldReturnNoErrorMsg(string filePath)
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var actual = InputPathFileValidator.FileValidationHandler(filePath);

			//--------------Assert-----------
			actual.Should().BeEmpty();
		}
	}
}