using Monopoly.Server.GameLogic.Interfaces;

namespace Monopoly.Server.GameLogic.FieldCells;

public class MetroCell : IFieldCell
{
    public Type Type { get; set; } = Type.Metro;
    public string Action(Player player)
    {
        player.CellNumber = player.CellNumber switch
        {
            4 => 12,
            12 => 24,
            24 => 32,
            32 => 4,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        return player.Name + " вы прокатились на метро";
    }
}