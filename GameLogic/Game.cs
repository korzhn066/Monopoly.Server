using System.Numerics;

namespace Monopoly.Server.GameLogic;

public static class Game
{
    public static readonly Dictionary<string, Room> Rooms = new Dictionary<string, Room>();
    private static readonly List<string> AllKeys = new List<string>();

    public static string AddRoom()
    {
        var rnd = new Random();
        string key;
        
        while (true)
        {
            key = rnd.Next(100000, 999999).ToString();
            if (AllKeys.Contains(key)) continue;
            AllKeys.Add(key);
            break;
        }

        Rooms.Add(key, new Room());
        return key;
    }

    public static bool IsRoomExist(string key)
    {
        return Rooms.ContainsKey(key);
    }

    public static void RemoveRoom(string key)
    {
        Rooms.Remove(key);
    }
}