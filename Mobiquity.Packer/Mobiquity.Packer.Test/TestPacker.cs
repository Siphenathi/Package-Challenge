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

		[TestCase()]
		public void ExecuteRules_WhenCalledWithOnePackage_ShouldExecuteOneRule()
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var actual = Packer.ExecuteRules(GetOnePackage());

			//--------------Assert--------------
			actual?.Should().Be("1\n");
		}

		[Test]
		public void ExecuteRules_WhenCalledWithTwoPackages_ShouldExecuteTwoRules()
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var actual = Packer.ExecuteRules(GetTwoPackage());

			//--------------Assert--------------
			actual?.Should().Be("2\n");
		}

		[Test]
		public void ExecuteRules_WhenCalledWithThreePackages_ShouldExecuteThreeRules()
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var actual = Packer.ExecuteRules(GetThreePackage());

			//--------------Assert--------------
			actual?.Should().Be("3\n");
		}

		[Test]
		public void Test()
		{
			//--------------Arrange--------------

			//--------------Act------------------
			var actual = Packer.ExecuteRules(GetMoreThanThreePackage());

			//--------------Assert--------------
			actual?.Should().Be("2, 7");
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
				//(7,81.80,€45) (8,19.36,€79) (9,6.76,€64)
				new Package
				{
					PackageWeightLimit = 56,
					Items = new List<Item>
					{
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
							Index = 7,
							Weight = 81.80,
							Cost = 45
						},
						new Item
						{
							Index = 5,
							Weight = 46.81,
							Cost = 36
						}
					}
				}
			};
		}
	}
}