using Microsoft.AspNetCore.SignalR;
using Monopoly.Server.GameLogic;

namespace Monopoly.Server.Hubs;


class Response<T>
{ 
    public T? Data { get; set; }
    public string? ResponseType{ get; set; }
}
public class GameLogMessageHub : Hub
{
    public async Task StartGame(string groupName)
    {
        await SendMessage("GameStart", groupName);
        
        var status = Game.Rooms[groupName].MakeAMove();
        if (status.isTurnEnded)
        {
            await SendMessage(status.response, groupName);
            await SendPlayers(groupName);
        }
        else
        {
            await SendMessage("Хотите ли вы купить компанию", groupName);
            await SendPlayers(groupName);
            var response = new Response<string?>()
            {
                ResponseType = "-buyCompany"
            };

            await Clients.User(Game.Rooms[groupName].GetCurrentUserConnectionId())
                .SendAsync("ReceiveGameLogMessage", response);
        }
    }

    public async Task BuyCompany(string groupName, bool isBuyCompany)
    {
        if (isBuyCompany)
        {
            await SendMessage(Game.Rooms[groupName].BuyCompany(), groupName);
            await SendPlayers(groupName);
        }
        else
        {
            await SendMessage($"компания выставляется на аукцион начальная цена: {Game.Rooms[groupName].GetCompanyPrice()}", groupName);
            
            var response = new Response<string?>()
            {
                ResponseType = "-startAuction"
            };

            await Clients.OthersInGroup(groupName).SendAsync("ReceiveGameLogMessage", response);
            Game.Rooms[groupName].SetAuctionParticipants();
        }
    }

    public async Task IncreasePrice(string groupName, int price)
    {
        Game.Rooms[groupName].IncreaseAuctionPrice(Context.ConnectionId, price);
        await SendMessage($"цена увеличена до {price}", groupName);
    }

    public async Task LeaveTheAuction(string groupName)
    {
        await SendMessage(Game.Rooms[groupName].RemoveAuctionParticipant().ToString(), groupName);
    }

    public async Task AddToGroup(string groupName, string playerName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        
        Game.Rooms[groupName].AddPlayer(Context.ConnectionId, playerName, Context.UserIdentifier);
        await SendPlayers(groupName);
        await SendMessage($"{playerName} присоеденился", groupName);
    }
    
    private async Task SendMessage(string? message, string groupName)
    {
        var response = new Response<string?>()
        {
            Data = message,
            ResponseType = "-message"
        };
        
        await Clients.Group(groupName)
            .SendAsync("ReceiveGameLogMessage", response);
    }
    
    private async Task SendPlayers(string groupName)
    {
        var response = new Response<IEnumerable<Player>>()
        {
            Data = Game.Rooms[groupName].Players,
            ResponseType = "-players"
        };
        
        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveGameLogMessage", 
            new Response<IEnumerable<Player>>()
            {
                Data = Game.Rooms[groupName].Players,
                ResponseType = "-some"
            });
        
        await Clients.Group(groupName)
            .SendAsync("ReceiveGameLogMessage", response);
    }
}