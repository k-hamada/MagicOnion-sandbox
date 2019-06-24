using System;
using System.Threading.Tasks;
using System.Linq;
using MagicOnion.Server.Hubs;
using UnityEngine;
using Shared.Hubs;
using Shared.MessagePackObjects;

namespace Server
{
    public class GamingHub : StreamingHubBase<IGamingHub, IGamingHubReceiver>, IGamingHub
    {
        IGroup room;
        Player self;
        IInMemoryStorage<Player> storage;

        public async Task<Player[]> JoinAsync(string roomName, string userName, Vector3 position, Quaternion rotation)
        {
            Console.WriteLine($"{DateTime.UtcNow.ToShortTimeString()}: {roomName} - {userName}");
            self = new Player() { Name = userName, Position = position, Rotation = rotation };

            (room, storage) = await Group.AddAsync(roomName, self);

            Broadcast(room).OnJoin(self);

            return storage.AllValues.ToArray();
        }

        public async Task LeaveAsync()
        {
            await room.RemoveAsync(this.Context);
            Broadcast(room).OnLeave(self);
        }

        public async Task MoveAsync(Vector3 position, Quaternion rotation)
        {
            self.Position = position;
            self.Rotation = rotation;
            Broadcast(room).OnMove(self);
        }
    }
}