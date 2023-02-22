using Monopoly.Server.GameLogic.Interfaces;

namespace Monopoly.Server.GameLogic.FieldCells;

internal class Chance
{
    public string Description { get; }
    public int Prize { get; }

    public Chance(int prize, string description)
    {
        Prize = prize;
        Description = description;
    }
}

public class ChanceCell : IFieldCell
{
    private readonly List<Chance> _chances = new List<Chance>(5)
    {
        new Chance(200,"Вам подарок 200"),
        new Chance(200,"Вам подарок 200"),
        new Chance(200,"Вам подарок 200"),
        new Chance(200,"Вам подарок 200"),
        new Chance(200,"Вам подарок 200"),
    };

    public Type Type { get; set; } = Type.Chance;
    public string Action(Player player)
    {
        var i = new Random().Next(0, 4);
        player.Money += _chances[i].Prize;
        return player.Name + _chances[i].Description;
    }
}