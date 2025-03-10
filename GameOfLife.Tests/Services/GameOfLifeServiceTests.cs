using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GameOfLifeAPI.Persistence;
using GameOfLifeAPI.Services;
using static System.Net.Mime.MediaTypeNames;

namespace GameOfLife.Tests.Services
{
    public class GameOfLifeServiceTests
    {
        private GameOfLifeService CreateService(out GameOfLifeDbContext context)
        {
            var options = new DbContextOptionsBuilder<GameOfLifeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new GameOfLifeDbContext(options);
            return new GameOfLifeService(context);
        }

        [Fact]
        public async Task AddBoard_ShouldPersistBoard()
        {
            // Arrange
            var service = CreateService(out var context);
            int[][] board = new int[][]
            {
                new[] { 0, 1, 0 },
                new[] { 1, 1, 1 },
                new[] { 0, 1, 0 }
            };

            // Act
            var id = await service.AddBoard(board);

            // Assert
            var entity = await context.Boards.FindAsync(id);
            entity.Should().NotBeNull();
            entity!.State.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetNextState_ShouldUpdateAndReturnCorrectState()
        {
            // Arrange
            var service = CreateService(out _);
            int[][] initial = new int[][]
            {
                new[] { 0, 1, 0 },
                new[] { 0, 1, 0 },
                new[] { 0, 1, 0 }
            };
            var id = await service.AddBoard(initial);

            // Act
            var next = await service.GetNStatesAway(id);

            // Assert
            next.Should().BeEquivalentTo(new int[][]
            {
                new[] { 0, 0, 0 },
                new[] { 1, 1, 1 },
                new[] { 0, 0, 0 }
            });
        }

        [Fact]
        public async Task GetFinalState_ShouldReturnDeadBoard()
        {
            // Arrange
            var service = CreateService(out _);
            int[][] board = new int[][]
            {
                new[] { 1, 0, 0 },
                new[] { 0, 1, 0 },
                new[] { 0, 0, 1 }
            };

            var id = await service.AddBoard(board);

            // Act
            var result = await service.GetFinalState(id, maxAttempts: 2);

            // Assert
            result.Should().BeEquivalentTo(new int[][]
            {
                new[] { 0, 0, 0 },
                new[] { 0, 0, 0 },
                new[] { 0, 0, 0 }
            });
        }

        [Fact]
        public async Task GetFinalState_RPentominoBoard_ShouldReturnNull()
        {
            // Arrange
            var service = CreateService(out _);

            // R-pentomino pattern is known to stabilize after many generations
            int[][] board = new int[][]
            {
                new[] { 0, 0, 0, 0, 0, 0 },
                new[] { 0, 0, 1, 1, 0, 0 },
                new[] { 0, 1, 1, 0, 0, 0 },
                new[] { 0, 0, 1, 0, 0, 0 },
                new[] { 0, 0, 0, 0, 0, 0 },
                new[] { 0, 0, 0, 0, 0, 0 }
            };

            var id = await service.AddBoard(board);

            // Act
            var result = await service.GetFinalState(id, maxAttempts: 10);

            // Assert
            result.Should().BeNull();
        }

    }


}

