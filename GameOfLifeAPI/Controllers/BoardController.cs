using GameOfLifeAPI.Contracts;
using GameOfLifeAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLifeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly ILogger<BoardController> _logger;
        private readonly IGameOfLifeService _gameOfLifeService;

        public BoardController(ILogger<BoardController> logger, IGameOfLifeService gameOfLifeService)
        {

            _logger = logger;
            _gameOfLifeService = gameOfLifeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBoard(int[][] board)
        {
            if (!board.IsValidBoard())
            {
                return BadRequest("Invalid board: must be a non-empty grid of 0s and 1s with equal-length rows.");
            }

            var boardId = await _gameOfLifeService.AddBoard(board);
            return Ok(boardId);
        }

        [HttpGet("{boardId}/next")]
        public async Task<IActionResult> GetNextState(string boardId)
        {
            var nextState = await _gameOfLifeService.GetNStatesAway(boardId);
            return Ok(nextState);
        }

        [HttpGet("{boardId}/next/{steps}")]
        public async Task<IActionResult> GetNStatesAway(string boardId, int steps)
        {
            var nStatesAway = await _gameOfLifeService.GetNStatesAway(boardId, steps);
            return Ok(nStatesAway);
        }

        [HttpGet("{boardId}/final")]
        public async Task<IActionResult> GetFinalState(string boardId, [FromQuery] int maxAttempts = 10)
        {
            var finalState = await _gameOfLifeService.GetFinalState(boardId, maxAttempts);
            return Ok(finalState);
        }
    }
}
