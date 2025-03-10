namespace GameOfLifeAPI.Extensions
{
    public static class BoardExtensions
    {
        public static bool IsValidBoard(this int[][] board)
        {
            if (board == null || board.Length == 0 || board.Any(row => row == null || row.Length != board[0].Length))
                return false;

            foreach (var row in board)
                foreach (var cell in row)
                    if (cell != 0 && cell != 1)
                        return false;

            return true;
        }
    }
}
