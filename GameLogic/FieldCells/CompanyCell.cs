using Monopoly.Server.GameLogic.Interfaces;

namespace Monopoly.Server.GameLogic.FieldCells;

public class CompanyCell : IFieldCell
{
    public CompanyCell(int price, int branchPrice, int rent, int rentWithMonopoly, int rentWithBranch)
    {
        Price = price;
        BranchPrice = branchPrice;
        _rent = rent;
        _rentWithMonopoly = rentWithMonopoly;
        _rentWithBranch = rentWithBranch;
        _isMonopoly = false;
        _isCompanyWithBranch = false;
    }

    private Player? Owner { get; set; } = null;
    public int Price { get; set; }
    private int BranchPrice { get; set; }
    private readonly int _rent;
    private readonly int _rentWithMonopoly;
    private readonly int _rentWithBranch;
    private readonly bool _isMonopoly;
    private readonly bool _isCompanyWithBranch;

    public string BuyCompany(Player player)
    {
        Owner = player;
        player.Money -= Price;
        return player.Name + " вы купили компанию";
    }
    
    public Type Type { get; set; } = Type.Company;
    public string? Action(Player player)
    {
        if (Owner is null)
            return null;
        
        int rent = _rent;
        if (_isMonopoly) rent = _rentWithMonopoly;
        if (_isCompanyWithBranch) rent = _rentWithBranch;

        Owner.Money += rent;
        player.Money -= rent;
        return player.Name + " вы заплатитли ренту";
    }
}