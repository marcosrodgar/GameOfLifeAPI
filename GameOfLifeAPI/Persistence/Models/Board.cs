namespace GameOfLifeAPI.Persistence.Models
{
    public class Board
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string State { get; set; } = string.Empty;
    }
}
