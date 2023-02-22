using Microsoft.AspNetCore.Mvc;
using Monopoly.Server.GameLogic;
using Monopoly.Server.Models;

namespace Monopoly.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    [HttpGet]
    [Route("GetRoomId")]
    public string GetRoomId()
    {
        return Game.AddRoom();
    }

    [HttpPost]
    [Route("GoToRoom")]
    public Response GoToRoom(string key, string playerName)
    {
        if (!Game.IsRoomExist(key))
            return new Response() {IsDone = false, Message = "Такой комнаты не существует"};
        
        return new Response() {IsDone = true, Message = "Ваш ключ: " + key.ToString()};
    }
}