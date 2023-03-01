using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Mobiquity.Packer.Test
{
	public class TestPackProcessorHandler
	{
		[Test]
		public void GetFirstItemWithHigherCost_WhenCalledWithOneItemWithWeightGreaterThanPackageWeightLimit_ShouldReturnNothing()
		{
			//--------------Arrange--------------
			const int packageWeightLimit = 8;
			var items = new List<string>
			{
				"1,15.3,€34"
			};

			//--------------Act------------------
			var actual = PackProcessorHandler.GetFirstItemWithHigherCost(items, packageWeightLimit);

			//--------------Assert---------------
			actual.Should().BeNullOrEmpty();
		}

		[Test]
		public void GetFirstItemWithHigherCost_WhenCalledWithTwoItemsOneWithWeightLessThanPackageWeightLimit_ShouldReturnItemWithLessWeight()
		{
			//--------------Arrange--------------
			const int packageWeightLimit = 8;
			var items = new List<string>
			{
				"1,15.3,€34",
				"2, 7.5, $78"
			};

			//--------------Act------------------
			var actual = PackProcessorHandler.GetFirstItemWithHigherCost(items, packageWeightLimit);

			//--------------Assert---------------
			actual.Should().Be("2, 7.5, $78");
		}

		[Test]
		public void GetFirstItemWithHigherCost_WhenCalledWithManyItemsOneWithWeightLessThanPackageWeightLimit_ShouldReturnItemWithGreaterCost()
		{
			//--------------Arrange--------------
			const int packageWeightLimit = 8;
			var items = new List<string>
			{
				"1,15.3,€34",
				"2, 7.5, $58",
				"3, 6, $78"
			};

			//--------------Act------------------
			var actual = PackProcessorHandler.GetFirstItemWithHigherCost(items, packageWeightLimit);

			//--------------Assert---------------
			actual.Should().Be("3, 6, $78");
		}

		[Test]
		public void GetFirstItemWithHigherCost_WhenCalledWithManyItemsOneWithSameWeightButLessThanPackageWeightLimit_ShouldReturnItemWithGreaterCost()
		{
			//--------------Arrange--------------
			const int packageWeightLimit = 8;
			var items = new List<string>
			{
				"1,15.3,€34",
				"2, 7.5, $58",
				"3, 7.5, $78"
			};

			//--------------Act------------------
			var actual = PackProcessorHandler.GetFirstItemWithHigherCost(items, packageWeightLimit);

			//--------------Assert---------------
			actual.Should().Be("3, 7.5, $78");
		}

		[Test]
		public void GetSecondWithHigherCost_WhenCalledWithOneItemWithWeightGreaterThanPackageWeightLimit_ShouldReturnNothing()
		{
			//--------------Arrange--------------
			const int packageWeightLimit = 8;
			const string firstItem = "2, 7.5, $58";
			var items = new List<string>
			{
				"1,15.3,€34"
			};

			//--------------Act------------------
			var actual = PackProcessorHandler.GetSecondWithHigherCost(items, firstItem, packageWeightLimit);

			//--------------Assert---------------
			actual.Should().BeNullOrEmpty();
		}

		[Test]
		public void GetSecondWithHigherCost_WhenCalledWithTwoItemsWithSumOfWeightGreaterThanPackageWeightLimit_ShouldReturnNothing()
		{
			//--------------Arrange--------------
			const int packageWeightLimit = 8;
			const string firstItem = "2, 7.5, $58";
			var items = new List<string>
			{
				"1,15.3,€24",
				"1,3.3,€34"
			};

			//--------------Act------------------
			var actual = PackProcessorHandler.GetSecondWithHigherCost(items, firstItem, packageWeightLimit);

			//--------------Assert---------------
			actual.Should().BeNullOrEmpty();
		}

		[Test]
		public void GetSecondWithHigherCost_WhenCalledWithManyItems_WithTwoOfItemsHavingSumOfWeight_LessThanPackageWeightLimit_ShouldReturnNothing()
		{
			//--------------Arrange--------------
			const int packageWeightLimit = 75;
			const string firstItem = "2,14.55,€74";
			var items = new List<string>
			{
				"1,3.3,€34",
				"2,14.55,€74",
				"3,3.98,€16",
				"4,26.24,€55",
				"7,60.02,€74",
				"8,93.18,€35"
			};

			//--------------Act------------------
			var actual = PackProcessorHandler.GetSecondWithHigherCost(items, firstItem, packageWeightLimit);

			//--------------Assert---------------
			actual.Should().Be("7,60.02,€74");
		}

		//GetSecondItemWithHigherCost

		[TestCase("8 : (1,15.3,€34)", "-\n")]
		[TestCase("8 : (1,17.3,€34) (1,15.3,€21) (1,25.3,€78) ", "-\n")]
		[TestCase("10 : (2, 9.5, €20)", "2\n")]
		[TestCase("81 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)", "4\n")]
		[TestCase("75 : (1,85.31,€29) (2,14.55,€74) (3,3.98,€16) (4,26.24,€55) " +
		          "(5,63.69,€52) (6,76.25,€75) (7,60.02,€74) (8,93.18,€35) (9,89.95,€78)", "2, 7\n")]
		[TestCase("56 : (1,90.72,€13) (2,33.80,€40) (3,43.15,€10) (4,37.97,€16) (5,46.81,€36) " +
		          "(6,48.77,€79) (7,81.80,€45) (8,19.36,€79) (9,6.76,€64)", "8, 9\n")]
		public void ExecuteRules_WhenCalledWithNoModel_ShouldExecuteOneRule(string fileRows, string expected)
		{
			//--------------Arrange--------------
			//--------------Act------------------
			var actual = PackProcessorHandler.HandlePacking(new[] { fileRows });

			//--------------Assert--------------
			actual?.Should().Be(expected);
		}
	}
}