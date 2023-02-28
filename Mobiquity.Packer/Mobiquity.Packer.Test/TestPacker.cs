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
		public void Learn()
		{
			//--------------Arrange--------------
			var filePath =
				@"C:\Users\Siphenathi\Documents\Dev-Time\Personal-Project\Job Hunting\Mobiquity\skeleton_net\resources\input2.txt";
			//--------------Act------------------
			var actual = Packer.Pack(filePath);

			//--------------Assert--------------
			actual?.Should().Contain("gerg");
		}

		//[Test]
		//public void Learn444()
		//{
		//	//--------------Arrange--------------
		//	var filePath =
		//		@"C:\Users\Siphenathi\Documents\Dev-Time\Personal-Project\Job Hunting\Mobiquity\skeleton_net\resources\input.txt";
		//	//--------------Act------------------
		//	var actual = Packer.Pack(filePath);

		//	//--------------Assert-----------
		//	actual?.Should().Contain("gerg");
		//}
	}
}