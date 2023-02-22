using Monopoly.Server.GameLogic.Interfaces;

namespace Monopoly.Server.GameLogic.FieldCells;

public class CasinoCell : IFieldCell
{
    public Type Type { get; set; } = Type.Casino;
    public string Action(Player player)
    {
        if (new Random().Next(1,2) == 1)
        {
            player.Money = (int) (player.Money * 0.9);
            return player.Name + " упс вы проиграли";
        }
        
        player.Money = (int) (player.Money * 1.1);
        return player.Name + " победа";
    }
}