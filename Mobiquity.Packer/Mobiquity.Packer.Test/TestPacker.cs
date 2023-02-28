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
	}
}