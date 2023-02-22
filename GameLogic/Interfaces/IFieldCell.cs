namespace Monopoly.Server.GameLogic.Interfaces;

public interface IFieldCell
{
    Type Type { get; set; }
    string Action(Player player);
}