namespace GameOfLifeAPI.Contracts
{
    public interface IGameOfLifeService
    {
        Task<string> AddBoard(int[][] board);
        Task<int[][]?> GetNStatesAway(string boardId, int steps = 1);
        Task<int[][]?> GetFinalState(string boardId, int maxAttempts = 10);
    }
}
