using Monopoly.Server.GameLogic.Interfaces;

namespace Monopoly.Server.GameLogic.FieldCells;

public class StartCell : IFieldCell
{
    public Type Type { get; set; } = Type.Start;
    public string Action(Player player)
    {
        player.Money += 200;
        return player.Name + " вы получили 200";
    }
}