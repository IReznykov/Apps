using System.Collections.Generic;
using FluentAssertions;
using Ikc5.AutomataScreenSaver.Life.Models;
using Xunit;

namespace Life.Models.Tests
{
	public class ChartRepositoryTests
	{
		[Fact]
		public void ChartRepository_Should_Initialize()
		{
			var chartRepository = new ChartRepository();
			chartRepository.Should().NotBeNull();

			chartRepository.TotalCellCountList.Should().NotBeNull();
			chartRepository.TotalCellCountList.Count.Should().Be(0);

			chartRepository.BornedCellCountList.Should().NotBeNull();
			chartRepository.BornedCellCountList.Count.Should().Be(0);

			chartRepository.DiedCellCountList.Should().NotBeNull();
			chartRepository.DiedCellCountList.Count.Should().Be(0);

			chartRepository.AgeCountList.Should().NotBeNull();
			chartRepository.AgeCountList.Count.Should().Be(0);
		}

		[Fact]
		public void ChartRepository_Should_AddToTotalCellCountList()
		{
			var chartRepository = new ChartRepository();
			chartRepository.Should().NotBeNull();

			// AddTotalCellCount
			const int expectedValue = 10;
			chartRepository.AddTotalCellCount(expectedValue);
			chartRepository.TotalCellCountList.Count.Should().Be(1);
			chartRepository.TotalCellCountList[0].Should().Be(expectedValue);
		}

		[Theory]
		[InlineData(100)]
		[InlineData(101)]
		[InlineData(102)]
		[InlineData(103)]
		[InlineData(104)]
		[InlineData(105)]
		[InlineData(106)]
		public void ChartRepository_Should_RemoveExtraValuesFromTotalCellCountList(int count)
		{
			var chartRepository = new ChartRepository();
			chartRepository.Should().NotBeNull();

			// AddTotalCellCount
			const int length = 100;
			for (var pos = 0; pos < count; pos++)
			{
				chartRepository.AddTotalCellCount(pos);
			}
			// check TotalCellCount
			chartRepository.TotalCellCountList.Count.Should().Be(length);
			chartRepository.TotalCellCountList[0].Should().Be(count - length);
			chartRepository.TotalCellCountList[length - 1].Should().Be(count - 1);
		}

		[Fact]
		public void ChartRepository_Should_AddToBornedCellCountList()
		{
			var chartRepository = new ChartRepository();
			chartRepository.Should().NotBeNull();

			// AddBornedCellCount
			const int expectedValue = 10;
			chartRepository.AddBornedCellCount(expectedValue);
			chartRepository.BornedCellCountList.Count.Should().Be(1);
			chartRepository.BornedCellCountList[0].Should().Be(expectedValue);
		}

		[Theory]
		[InlineData(100)]
		[InlineData(101)]
		[InlineData(102)]
		[InlineData(103)]
		[InlineData(104)]
		[InlineData(105)]
		[InlineData(106)]
		public void ChartRepository_Should_RemoveExtraValuesFromBornedCellCountList(int count)
		{
			var chartRepository = new ChartRepository();
			chartRepository.Should().NotBeNull();

			// AddBornedCellCount
			const int length = 100;
			for (var pos = 0; pos < count; pos++)
			{
				chartRepository.AddBornedCellCount(pos);
			}
			// check BornedCellCount
			chartRepository.BornedCellCountList.Count.Should().Be(length);
			chartRepository.BornedCellCountList[0].Should().Be(count - length);
			chartRepository.BornedCellCountList[length - 1].Should().Be(count - 1);
		}

		[Fact]
		public void ChartRepository_Should_AddToDiedCellCountList()
		{
			var chartRepository = new ChartRepository();
			chartRepository.Should().NotBeNull();

			// AddDiedCellCount
			const int expectedValue = 10;
			chartRepository.AddDiedCellCount(expectedValue);
			chartRepository.DiedCellCountList.Count.Should().Be(1);
			chartRepository.DiedCellCountList[0].Should().Be(expectedValue);
		}

		[Theory]
		[InlineData(100)]
		[InlineData(101)]
		[InlineData(102)]
		[InlineData(103)]
		[InlineData(104)]
		[InlineData(105)]
		[InlineData(106)]
		public void ChartRepository_Should_RemoveExtraValuesFromDiedCellCountList(int count)
		{
			var chartRepository = new ChartRepository();
			chartRepository.Should().NotBeNull();

			// AddDiedCellCount
			const int length = 100;
			for (var pos = 0; pos < count; pos++)
			{
				chartRepository.AddDiedCellCount(pos);
			}
			// check DiedCellCount
			chartRepository.DiedCellCountList.Count.Should().Be(length);
			chartRepository.DiedCellCountList[0].Should().Be(count - length);
			chartRepository.DiedCellCountList[length - 1].Should().Be(count - 1);
		}

		[Fact]
		public void ChartRepository_Should_UpdateAgeCountList()
		{
			var chartRepository = new ChartRepository();
			chartRepository.Should().NotBeNull();

			// UpdateAges
			var actualValue = new List<int>(new[] { 1, 2, 3, 4, 5, 6, 7 });
			var expectedValue = new List<int>(new[] { 2, 3, 4, 5, 6, 7 });
			chartRepository.UpdateAges(actualValue);
			chartRepository.AgeCountList.Count.Should().Be(expectedValue.Count);
			for (var pos = 0; pos < expectedValue.Count; pos++)
				chartRepository.AgeCountList[pos].Should().Be(expectedValue[pos]);
		}

		[Fact]
		public void ChartRepository_Should_IgnoreZeroValuesFromAgeCountList()
		{
			var chartRepository = new ChartRepository();
			chartRepository.Should().NotBeNull();

			// init array
			const int arrayLength = 60;
			const int expectedLength = 49;
			var expectedValue = new int[arrayLength];
			for (var pos = 0; pos < 40; pos++)
			{
				expectedValue[pos] = 100 - pos;
			}

			// check AgeCount
			chartRepository.UpdateAges(expectedValue);
			chartRepository.AgeCountList.Count.Should().Be(expectedLength);
			chartRepository.AgeCountList[0].Should().Be(100 - 1);
			chartRepository.AgeCountList[1].Should().Be(100 - 2);
			chartRepository.AgeCountList[expectedLength - 1].Should().Be(0);
		}

		[Fact]
		public void ChartRepository_Should_SumValuesFromAgeCountList()
		{
			var chartRepository = new ChartRepository();
			chartRepository.Should().NotBeNull();

			// init array
			const int arrayLength = 60;
			const int expectedLength = 50;
			var expectedValue = new int[arrayLength];
			for (var pos = 0; pos < arrayLength; pos++)
			{
				expectedValue[pos] = 100 - pos;
			}
			// check AgeCount
			chartRepository.UpdateAges(expectedValue);
			chartRepository.AgeCountList.Count.Should().Be(expectedLength);
			chartRepository.AgeCountList[0].Should().Be(100 - 1);
			chartRepository.AgeCountList[1].Should().Be(100 - 2);
			chartRepository.AgeCountList[expectedLength - 1].Should().Be(506);
		}

	}

}
