using MagicOnion;
using UnityEngine;
using MessagePack;
using System.Threading.Tasks;

public interface IGamingHubReceiver
{
    void OnJoin(Player player);
    void OnLeave(Player player);
    void OnMove(Player player);
}

public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
{
    Task<Player[]> JoinAsync(string roomName, string userName, Vector3 position, Quaternion rotation);
    Task LeaveAsync();
    Task MoveAsync(Vector3 position, Quaternion rotation);
}

[MessagePackObject]
public class Player
{
    [Key(0)]
    public string Name { get; set; }
    [Key(1)]
    public Vector3 Position { get; set; }
    [Key(2)]
    public Quaternion Rotation { get; set; }
}
