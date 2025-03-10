using Xunit;
using FluentAssertions;
using GameOfLifeAPI.Extensions;

namespace GameOfLife.Tests.Extensions
{
    public class BoardExtensionsTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ValidBoard_ShouldReturnTrue(int cellValue)
        {
            int[][] board = new int[][]
            {
                new int[] { cellValue, cellValue },
                new int[] { cellValue, cellValue }
            };

            board.IsValidBoard().Should().BeTrue();
        }

        [Fact]
        public void NullBoard_ShouldReturnFalse()
        {
            int[][] board = null;
            board.IsValidBoard().Should().BeFalse();
        }

        [Fact]
        public void EmptyBoard_ShouldReturnFalse()
        {
            int[][] board = new int[][] { };
            board.IsValidBoard().Should().BeFalse();
        }

        [Fact]
        public void JaggedBoard_ShouldReturnFalse()
        {
            int[][] board = new int[][]
            {
                new int[] { 0, 1 },
                new int[] { 1 }
            };

            board.IsValidBoard().Should().BeFalse();
        }

        [Fact]
        public void BoardWithInvalidValues_ShouldReturnFalse()
        {
            int[][] board = new int[][]
            {
                new int[] { 0, 2 },
                new int[] { 1, -1 }
            };

            board.IsValidBoard().Should().BeFalse();
        }
    }
}
