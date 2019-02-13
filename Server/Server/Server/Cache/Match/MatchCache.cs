using AhpilyServer;
using AhpilyServer.Concurrent;
using Server.Cache.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Server.Cache
{
    public class MatchCache
    {
        /// <summary>
        /// Waiting User ID and Waiting Room ID
        /// </summary>
        private Dictionary<int, int> uidRoomIdDict = new Dictionary<int, int>();

        /// <summary>
        /// Waiting Room ID and Room Data Model
        /// </summary>
        private Dictionary<int, MatchRoom> roomIdModelDict = new Dictionary<int, MatchRoom>();

        /// <summary>
        /// room object
        /// </summary>
        Queue<MatchRoom> roomQueue = new Queue<MatchRoom>();

        private ConcurrentInt id = new ConcurrentInt(-1);

        public bool IsMatching(int userId)
        {
            return uidRoomIdDict.ContainsKey(userId);
        }

        /// <summary>
        /// get player room
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MatchRoom GetRoom(int userId)
        {
            int roomId = uidRoomIdDict[userId];
            MatchRoom room = roomIdModelDict[roomId];
            return room;
        }

        //enter match queue
        public MatchRoom Enter(int userId, ClientPeer client)
        {
            //if find empty room
            foreach (MatchRoom mr in roomIdModelDict.Values)
            {
                if (mr.IsFull())
                    continue;
                mr.Enter(userId, client);
                uidRoomIdDict.Add(userId, mr.Id);
                return mr;
            }

            //no empty room,create room
            MatchRoom room = null;
            if (roomQueue.Count > 0)
                room = roomQueue.Dequeue();
            else
                room = new MatchRoom(id.Add_Get());
            room.Enter(userId, client);
            roomIdModelDict.Add(room.Id, room);
            uidRoomIdDict.Add(userId, room.Id);
            return room;
        }

        /// <summary>
        /// destroy room
        /// </summary>
        /// <param name="room"></param>
        public void Destroy(MatchRoom room)
        {
            roomIdModelDict.Remove(room.Id);
            foreach (var userId in room.UserIdClientDict.Keys)
            {
                uidRoomIdDict.Remove(userId);
            }
            room.UserIdClientDict.Clear();
            room.ReadyUserIdList.Clear();
            roomQueue.Enqueue(room);
        }

        //leave match room
        public MatchRoom Leave(int userId)
        {
            int roomId = uidRoomIdDict[userId];
            MatchRoom room = roomIdModelDict[roomId];
            room.Leave(userId);
            uidRoomIdDict.Remove(userId);
            if (room.IsEmpty())
            {
                roomIdModelDict.Remove(roomId);
                roomQueue.Enqueue(room);
            }
            return room;
        }
    }
}
