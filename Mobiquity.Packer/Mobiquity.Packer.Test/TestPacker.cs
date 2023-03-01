using System.Collections.Generic;
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
		public void Pack_WhenCalled_ShouldExecuteOneRule()
		{
			//--------------Arrange--------------
			const string filePath =
				@"C:\Users\Siphenathi\Documents\Dev-Time\Personal-Project\Job Hunting\Mobiquity\skeleton_net\resources\input.txt";

			//--------------Act------------------
			var actual = Packer.Pack(filePath);

			//--------------Assert--------------
			actual?.Should().Be("-\n");
		}

		//[Test]
		//public void ExecuteRules_WhenCalledWithOnePackage_ShouldExecuteOneRule()
		//{
		//	//--------------Arrange--------------

		//	//--------------Act------------------
		//	var actual = Packer.ExecuteRulesUsingPackageModel(GetOnePackage());

		//	//--------------Assert--------------
		//	actual?.Should().Be("1\n");
		//}

		//[Test]
		//public void ExecuteRules_WhenCalledWithTwoPackages_ShouldExecuteTwoRules()
		//{
		//	//--------------Arrange--------------

		//	//--------------Act------------------
		//	var actual = Packer.ExecuteRulesUsingPackageModel(GetTwoPackage());

		//	//--------------Assert--------------
		//	actual?.Should().Be("2\n");
		//}

		//[Test]
		//public void ExecuteRules_WhenCalledWithThreePackages_ShouldExecuteThreeRules()
		//{
		//	//--------------Arrange--------------

		//	//--------------Act------------------
		//	var actual = Packer.ExecuteRulesUsingPackageModel(GetThreePackage());

		//	//--------------Assert--------------
		//	actual?.Should().Be("3\n");
		//}

		//[Test]
		//public void Test()
		//{
		//	//--------------Arrange--------------

		//	//--------------Act------------------
		//	var actual = Packer.ExecuteRulesUsingPackageModel(GetMoreThanThreePackage1());

		//	//--------------Assert--------------
		//	actual?.Should().Be("2, 7");
		//}

		//===================================Using No packing Model=================================================>
		[TestCase("8 : (1,15.3,€34)", "-\n")]
		[TestCase("8 : (1,17.3,€34) (1,15.3,€21) (1,25.3,€78) ", "-\n")]
		[TestCase("10 : (2, 9.5, €20)", "2\n")]
		[TestCase("81 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)", "4\n")]
		[TestCase("75 : (1,85.31,€29) (2,14.55,€74) (3,3.98,€16) (4,26.24,€55) " +
				  "(5,63.69,€52) (6,76.25,€75) (7,60.02,€74) (8,93.18,€35) (9,89.95,€78)", "2, 7")]
		[TestCase("56 : (1,90.72,€13) (2,33.80,€40) (3,43.15,€10) (4,37.97,€16) (5,46.81,€36) " +
				  "(6,48.77,€79) (7,81.80,€45) (8,19.36,€79) (9,6.76,€64)", "8, 9")]
		public void ExecuteRules_WhenCalledWithNoModel_ShouldExecuteOneRule(string fileRows, string expected)
		{
			//--------------Arrange--------------
			//--------------Act------------------
			var actual = Packer.PackProcessorHandler(new []{fileRows});

			//--------------Assert--------------
			actual?.Should().Be(expected);
		}

		private static IEnumerable<Package> GetOnePackage()
		{
			return new List<Package>
			{
				new Package
				{
					PackageWeightLimit = 8,
					Items = new List<Item>
					{
						new Item
						{
							Index = 1,
							Weight = 7,
							Cost = 34
						}
					}
				}
			};
		}

		private static IEnumerable<Package> GetTwoPackage()
		{
			return new List<Package>
			{
				new Package
				{
					PackageWeightLimit = 8,
					Items = new List<Item>
					{
						new Item
						{
							Index = 1,
							Weight = 7.3,
							Cost = 4
						},
						new Item
						{
							Index = 2,
							Weight = 5.6,
							Cost = 35
						}
					}
				}
			};
		}

		private static IEnumerable<Package> GetThreePackage()
		{
			return new List<Package>
			{
				new Package
				{
					PackageWeightLimit = 8,
					Items = new List<Item>
					{
						new Item
						{
							Index = 1,
							Weight = 7.8,
							Cost = 34
						},
						new Item
						{
							Index = 2,
							Weight = 7.6,
							Cost = 5
						},
						new Item
						{
							Index = 3,
							Weight = 5.6,
							Cost = 40
						}
					}
				}
			};
		}

		private static IEnumerable<Package> GetMoreThanThreePackage()
		{
			return new List<Package>
			{
				new Package
				{
					PackageWeightLimit = 75,
					Items = new List<Item>
					{
						new Item
						{
							Index = 1,
							Weight = 85.31,
							Cost = 29
						},
						new Item
						{
							Index = 2,
							Weight = 14.55,
							Cost = 74
						},
						new Item
						{
							Index = 3,
							Weight = 3.98,
							Cost = 16
						},
						new Item
						{
							Index = 4,
							Weight = 26.24,
							Cost = 55
						},
						new Item
						{
							Index = 5,
							Weight = 63.69,
							Cost = 52
						},
						new Item
						{
							Index = 6,
							Weight = 76.25,
							Cost = 75
						},
						new Item
						{
							Index = 7,
							Weight = 60.02,
							Cost = 74
						},
						new Item
						{
							Index = 8,
							Weight = 93.18,
							Cost = 35
						},
						new Item
						{
							Index = 9,
							Weight = 89.95,
							Cost = 78
						}
					}
				}
			};
		}

		private static IEnumerable<Package> GetMoreThanThreePackage1()
		{
			return new List<Package>
			{
				new Package
				{
					PackageWeightLimit = 56,
					Items = new List<Item>
					{
						new Item
						{
							Index = 9,
							Weight = 6.76,
							Cost = 64
						},
						new Item
						{
							Index = 3,
							Weight = 43.15,
							Cost = 10
						},
						new Item
						{
							Index = 4,
							Weight = 37.97,
							Cost = 16
						},
						new Item
						{
							Index = 5,
							Weight = 46.81,
							Cost = 36
						},
						new Item
						{
							Index = 6,
							Weight = 48.77,
							Cost = 79
						},
						new Item
						{
							Index = 1,
							Weight = 90.72,
							Cost = 13
						},
						new Item
						{
							Index = 2,
							Weight = 33.80,
							Cost = 40
						},
						new Item
						{
							Index = 7,
							Weight = 81.80,
							Cost = 45
						},
						new Item
						{
							Index = 8,
							Weight = 19.36,
							Cost = 79
						},
					}
				}
			};
		}
	}
}