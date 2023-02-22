using Monopoly.Server.GameLogic.Interfaces;

namespace Monopoly.Server.GameLogic.FieldCells;

public class JailCell : IFieldCell
{
    public Type Type { get; set; } = Type.Jail;
    public string Action(Player player)
    {
        throw new NotImplementedException();
    }

    public int Action()
    {
        throw new NotImplementedException();
    }
}