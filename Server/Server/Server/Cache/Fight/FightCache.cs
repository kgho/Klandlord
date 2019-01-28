using AhpilyServer;
using AhpilyServer.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cache.Fight
{
    public class FightCache
    {
        private ConcurrentInt id = new ConcurrentInt(-1);

        private Dictionary<int, int> uIddRoomIdDict = new Dictionary<int, int>();

        //room id ,room model objcet
        private Dictionary<int, FightRoom> idRoomDict = new Dictionary<int, FightRoom>();

        private Queue<FightRoom> roomQueue = new Queue<FightRoom>();

        public UserCache userCache = Caches.User;

        /// <summary>
        /// create fight room
        /// </summary>
        /// <param name="uidList"></param>
        /// <returns></returns>
        public FightRoom Create(List<int> uidList)
        {
            FightRoom room = null;
            if (roomQueue.Count > 0)
            {
                room = roomQueue.Dequeue();
                room.Init(uidList);
            }
            else
            {
                room = new FightRoom(id.Add_Get(), uidList);
            }

            foreach (int uid in uidList)
            {
                uIddRoomIdDict.Add(uid, room.Id);
                ClientPeer client = userCache.GetClientPeer(uid);
                room.StartFight(uid, client);
            }
            idRoomDict.Add(room.Id, room);
            return room;
        }

    }
}
