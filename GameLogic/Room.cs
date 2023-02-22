using Monopoly.Server.GameLogic.FieldCells;
using Monopoly.Server.GameLogic.Interfaces;

namespace Monopoly.Server.GameLogic;

// check money in company

public class Room
{
    public readonly List<Player> Players = new List<Player>();
    private readonly Dictionary<int, IFieldCell> _field = new Dictionary<int, IFieldCell>();

    public Room()
    {
        _field.Add(0, new StartCell());
        _field.Add(1, new CompanyCell(60,150,30,60,120));
        _field.Add(2, new ChanceCell());
        _field.Add(3, new CompanyCell(60,150,30,60,120));
        _field.Add(4, new MetroCell());
        _field.Add(5, new CompanyCell(100,250,50,100,200));
        _field.Add(6, new ChanceCell());
        _field.Add(7, new CompanyCell(100,250,50,100,200));
        _field.Add(8, new CompanyCell(120,250,50,100,200));
        _field.Add(9, new JailCell());
        
        _field.Add(10, new CompanyCell(140,350,70,140,280));
        _field.Add(11, new CompanyCell(20,350,70,140,280));
        _field.Add(12, new MetroCell());
        _field.Add(13, new CompanyCell(160,400,80,160,320));
        _field.Add(14, new CompanyCell(180, 450, 90,180,360));
        _field.Add(15, new ChanceCell());
        _field.Add(16, new CompanyCell(180, 450, 90,180,360));
        _field.Add(17, new CompanyCell(200, 500, 100,200,400));
        
        _field.Add(18, new CasinoCell());
        _field.Add(19, new CompanyCell(220, 550, 110,220,440));
        _field.Add(20, new CompanyCell(220, 550, 110,220,440));
        _field.Add(21, new ChanceCell());
        _field.Add(22, new CompanyCell(240, 600, 120,240,480));
        _field.Add(23, new CompanyCell(130,650,130,260,520));
        _field.Add(24, new CompanyCell(260,650,130,260,520));
        _field.Add(25, new MetroCell());
        _field.Add(26, new CompanyCell(280,700,140,280,560));
        _field.Add(27, new JailCell());
        
        _field.Add(28, new CompanyCell(300, 750,150,300,600));
        _field.Add(29, new CompanyCell(300, 750,150,300,600));
        _field.Add(30, new ChanceCell());
        _field.Add(31, new CompanyCell(0, 800,160,320,640));
        _field.Add(32, new MetroCell());
        _field.Add(33, new ChanceCell());
        _field.Add(34, new CompanyCell(350, 800, 180,350,700));
        _field.Add(35, new CompanyCell(400, 1000, 200,400,800));
    }
    
    private Player? _currentPlayer;
    public void AddPlayer(string connectionId, string playerName, string userId)
    {
        var color = Players.Count switch
        {
            0 => "#f64327",
            1 => "#B3C100",
            2 => "#524A3A",
            3 => "#34675C",
            4 => "#B7B8B6",
            5 => "#4CB5F5",
            _ => "#"
        };
        
        Players.Add(new Player(connectionId, playerName, color, userId));
        _currentPlayer ??= Players[0];
    }

    public IEnumerable<string> GetAllWithoutOne()
    {
        var players = new List<string>();
        foreach (var player in Players)
        {
            if (player.ConnectionId != _currentPlayer!.ConnectionId)
            {
                players.Add(player.ConnectionId);
            }
        }

        return players;
    }

    private void RemovePlayer(string connectionId)
    {
        var player = Players.Find(player => player.ConnectionId == connectionId);
        if (player != null) Players.Remove(player);
    }

    private int _auctionCountParticipants;
    private KeyValuePair<Player, int> _theBiggestLot = new KeyValuePair<Player, int>();

    public bool IncreaseAuctionPrice(string connectionId, int price)
    {
        if (price <= _theBiggestLot.Value || price < GetCompanyPrice())
        {
            return false;
        }
        
        _theBiggestLot = new KeyValuePair<Player, int>(Players.FindLast(p => p.ConnectionId == connectionId)!, price);

        return true;
    }

    public int RemoveAuctionParticipant()
    {
        return _auctionCountParticipants--;
    }

    public void SetAuctionParticipants()
    {
        _auctionCountParticipants = Players.Count - 1;
    }

    public int GetCompanyPrice()
    {
        return ((CompanyCell) _field[_currentPlayer!.CellNumber]).Price;
    }

    private void RollTheDice()
    {
        _currentPlayer!.CellNumber += new Random().Next(2, 12);
        if (_currentPlayer.CellNumber > 36)
        {
            _currentPlayer.CellNumber -= 36;
        }
    }
    public (bool isTurnEnded, string? response) MakeAMove()
    {
        RollTheDice();
        var cell = _field[_currentPlayer!.CellNumber];
        string? response;
        var isTurnEnded = true;
        if (cell.Type != Type.Company)
        {
            response = cell.Action(_currentPlayer);
        }
        else
        {
            var companyCell = (CompanyCell) cell;
            response = companyCell.Action(_currentPlayer);
            if (response is null) 
                isTurnEnded = false;
        }
        
        return (isTurnEnded, response);
    }

    public string BuyCompany()
    {
        return ((CompanyCell)_field[_currentPlayer!.CellNumber]).BuyCompany(_currentPlayer);
    }

    private void NextPlayer()
    {
        var currentPlayerIndex = Players.IndexOf(_currentPlayer!);
        _currentPlayer = currentPlayerIndex + 1 != Players.Count ? Players[currentPlayerIndex + 1] : Players[0];
    }

    public string GetCurrentUserConnectionId()
    {
        return _currentPlayer!.ConnectionId;
    }
}