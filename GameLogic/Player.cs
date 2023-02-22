namespace Monopoly.Server.GameLogic;

public class Player
{
    public Player(string connectionId, string name, string color, string userId)
    {
        Name = name;
        Color = color;
        ConnectionId = connectionId;
        UserId = userId;
    }
    
    public string Name { get; set; }
    public string ConnectionId { get; set; }
    public string UserId { get; set; }

    public int Money { get; set; } = 15000;
    public int CellNumber { get; set; } = 0;
    
    public string Color { get; set; }
}