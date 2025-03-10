using GameOfLifeAPI.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace GameOfLifeAPI.Persistence
{
    public class GameOfLifeDbContext : DbContext
    {
        public  DbSet<Board> Boards { get; set; }
        public GameOfLifeDbContext(DbContextOptions<GameOfLifeDbContext> options) : base(options) { }
    }
}
