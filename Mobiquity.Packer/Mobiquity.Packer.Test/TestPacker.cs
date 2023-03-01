using FluentAssertions;
using Mobiquity.Packer.Model;
using NUnit.Framework;

namespace Mobiquity.Packer.Test
{
	public class TestPacker
	{
		[TestCase("de3f3fg3g")]
		[TestCase("c:jjdd\\jejdjd\\geg5")]
		[TestCase("C:\\Users\\Siphenathi\\Documents\\Dev-Time\\Personal-Project\\UserCaptureSystem")]
		public void Pack_WhenCalledWithNonExistingFilePath_ShouldThrowApiException(string filePath)
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var exception = Assert.Throws<ApiException>(() => Packer.Pack(filePath));

			//--------------Assert--------------
			exception?.Message.Should().Contain("No file found in this directory");
		}

		[Test]
		public void Pack_WhenCalledWithValidFilePath_ShouldReturnPackingResults()
		{
			//--------------Arrange--------------
			const string filePath = @"C:\Users\Siphenathi\Documents\Dev-Time\Personal-Project\Job Hunting\Mobiquity\skeleton_net\resources\input.txt";

			//--------------Act------------------
			var actual = Packer.Pack(filePath);

			//--------------Assert--------------
			actual.Should().Be("4\n-\n2, 7\n8, 9\n");
		}
	}
}