using GameOfLifeAPI.Contracts;
using GameOfLifeAPI.Extensions;
using GameOfLifeAPI.Persistence;
using GameOfLifeAPI.Persistence.Models;

namespace GameOfLifeAPI.Services
{
    public class GameOfLifeService : IGameOfLifeService
    {
        private readonly GameOfLifeDbContext _context;
        public GameOfLifeService(GameOfLifeDbContext context) => _context = context;
        public async Task<string> AddBoard(int[][] board)
        {
            var boardEntity = new Board { State = System.Text.Json.JsonSerializer.Serialize(board) };
            _context.Boards.Add(boardEntity);
            await _context.SaveChangesAsync();
            return boardEntity.Id;
        }

        public async Task<int[][]?> GetNStatesAway(string boardId, int steps = 1)
        {
            var boardEntity = await _context.Boards.FindAsync(boardId);
            if (boardEntity == null) return null;
            var board = System.Text.Json.JsonSerializer.Deserialize<int[][]>(boardEntity.State);
            for (int i = 0; i < steps; i++)
                board = ComputeNextState(board);
            boardEntity.State = System.Text.Json.JsonSerializer.Serialize(board);
            await _context.SaveChangesAsync();
            return board;
        }

        public async Task<int[][]?> GetFinalState(string boardId, int maxAttempts = 10)
        {
            var boardEntity = await _context.Boards.FindAsync(boardId);
            if (boardEntity == null) return null;
            var board = System.Text.Json.JsonSerializer.Deserialize<int[][]>(boardEntity.State);
            var seenStates = new HashSet<string>();
            for (int i = 0; i < maxAttempts; i++)
            {
                board = ComputeNextState(board);
                var boardString = System.Text.Json.JsonSerializer.Serialize(board);

                if (board.IsDead() || !seenStates.Add(boardString))
                {
                    boardEntity.State = boardString;
                    await _context.SaveChangesAsync();
                    return board;
                }
            }
            return null;

        }

        private static int[][] ComputeNextState(int[][] board)
        {
            int rows = board.Length, cols = board[0].Length;
            var newBoard = new int[rows][];
            for (int r = 0; r < rows; r++)
            {
                newBoard[r] = new int[cols];
                for (int c = 0; c < cols; c++)
                {
                    int liveNeighbors = GetCellLiveNeighbors(board, r, c);
                    var shouldSurvive = board[r][c] == 1 && (liveNeighbors == 2 || liveNeighbors == 3); 
                    var shouldComeAlive = board[r][c] == 0 && liveNeighbors == 3;
                    newBoard[r][c] = shouldSurvive || shouldComeAlive ? 1 : 0;
                }
            }
            return newBoard;
        }

        private static int GetCellLiveNeighbors(int[][] board, int row, int col)
        {
            int count = 0, rows = board.Length, cols = board[0].Length;
            for (int r = row - 1; r <= row + 1; r++)
                for (int c = col - 1; c <= col + 1; c++)
                {
                    var isNotSelf = r != row || c != col;
                    var rowWithinBounds = r >= 0 && r < rows;
                    var columnWithinBounds = c >= 0 && c < cols;
                    if (isNotSelf && rowWithinBounds && columnWithinBounds && board[r][c] == 1)
                        count++;
                }    
                    
            return count;
        }

        
    }
}
