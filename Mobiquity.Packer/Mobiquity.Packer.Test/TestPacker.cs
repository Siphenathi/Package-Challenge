using FluentAssertions;
using Mobiquity.Packer.Model;
using NUnit.Framework;

namespace Mobiquity.Packer.Test
{
	public class Tests
	{
		[TestCase("de3f3fg3g")]
		[TestCase("c:jjdd\\jejdjd\\geg5")]
		[TestCase("C:\\Users\\Siphenathi\\Documents\\Dev-Time\\Personal-Project\\UserCaptureSystem")]
		public void Pack_WhenCalledWithNonExistingFilePath_ShouldThrowApiException(string filePath)
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var exception = Assert.Throws<ApiException>(() => Packer.Pack(filePath));

			//--------------Assert-----------
			exception?.Message.Should().Contain("No file found in this directory");
		}

		[Test]
		public void Pack_WhenCalledWithEmptyFile_ShouldThrowApiException()
		{
			//--------------Arrange--------------
			const string filePath = "C:\\Users\\Siphenathi\\Documents\\Dev-Time\\Personal-Project\\Job Hunting\\Mobiquity\\skeleton_net\\resources\\EmptyFile.txt";

			//--------------Act------------------
			var exception = Assert.Throws<ApiException>(() => Packer.Pack(filePath));

			//--------------Assert--------------
			exception?.Message.Should().Contain("EmptyFile file is empty");
		}

		[Test]
		public void Pack_WhenCalledWithEmptyRowFile_ShouldThrowApiException()
		{//FileWithEmptyRow
		 //--------------Arrange--------------
			const string filePath = 
				"C:\\Users\\Siphenathi\\Documents\\Dev-Time\\Personal-Project\\Job Hunting\\" +
				"Mobiquity\\skeleton_net\\resources\\FileWithEmptyRow.txt";

			//--------------Act------------------
			var exception = Assert.Throws<ApiException>(() => Packer.Pack(filePath));

			//--------------Assert--------------
			exception?.Message.Should().Contain("Package data for row 1 is empty");
		}

		[Test]
		public void Pack_WhenCalledWithFileThatHasInvalidPackageData_ShouldThrowApiException()
		{
			//--------------Arrange--------------
			const string filePath =
				"C:\\Users\\Siphenathi\\Documents\\Dev-Time\\Personal-Project\\Job Hunting\\" +
				"Mobiquity\\skeleton_net\\resources\\InvalidPackageData.txt";

			//--------------Act------------------
			var exception = Assert.Throws<ApiException>(() => Packer.Pack(filePath));

			//--------------Assert--------------
			exception?.Message.Should().Contain("Invalid package data for this row");
		}

		[Test]
		public void Pack_WhenCalledWithFileThatHasIInvalidPackageWeighLimit_ShouldThrowApiException()
		{
			//--------------Arrange----------------
			const string filePath =
				"C:\\Users\\Siphenathi\\Documents\\Dev-Time\\Personal-Project\\Job Hunting\\" +
				"Mobiquity\\skeleton_net\\resources\\InvalidPackageWeighLimit.txt";

			//--------------Act------------------
			var exception = Assert.Throws<ApiException>(() => Packer.Pack(filePath));

			//--------------Assert--------------
			exception?.Message.Should().Contain("Invalid package limit for this row");
		}

		[Test]
		public void Pack_WhenCalledWithFileThatHasMoreThan15Items_ShouldThrowApiException()
		{
			//--------------Arrange----------------
			const string filePath =
				"C:\\Users\\Siphenathi\\Documents\\Dev-Time\\Personal-Project\\Job Hunting\\" +
				"Mobiquity\\skeleton_net\\resources\\fileWithMoreThan15Items.txt";

			//--------------Act------------------
			var exception = Assert.Throws<ApiException>(() => Packer.Pack(filePath));

			//--------------Assert--------------
			exception?.Message.Should().Contain("Row 3 has more than 15 items");
		}

		[Test]
		public void Pack_WhenCalledWithFile_ShouldThrowApiException()
		{
			//--------------Arrange----------------
			const string filePath =
				"C:\\Users\\Siphenathi\\Documents\\Dev-Time\\Personal-Project\\Job Hunting\\" +
				"Mobiquity\\skeleton_net\\resources\\input.txt";

			//--------------Act------------------
			var exception = Assert.Throws<ApiException>(() => Packer.Pack(filePath));

			//--------------Assert--------------
			exception?.Message.Should().Contain("Row 3 has more than 15 items");
		}
	}
}