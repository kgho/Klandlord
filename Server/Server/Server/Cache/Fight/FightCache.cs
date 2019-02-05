using AhpilyServer;
using AhpilyServer.Concurrent;
using Protocol.Dto.Fight;
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

        private Dictionary<int, int> uIdRoomIdDict = new Dictionary<int, int>();

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
                uIdRoomIdDict.Add(uid, room.Id);
                ClientPeer client = userCache.GetClientPeer(uid);
                room.StartFight(uid, client);
            }
            idRoomDict.Add(room.Id, room);
            return room;
        }

        public FightRoom GetRoom(int id)
        {
            if (idRoomDict.ContainsKey(id) == false)
                throw new Exception("Don't exist room id:" + id);
            return idRoomDict[id];
        }

        public FightRoom GetRoomByUId(int uid)
        {
            if (uIdRoomIdDict.ContainsKey(uid) == false)
            {
                throw new Exception(string.Format("The user id is {0} is not in room", uid));
            }
            int roomId = uIdRoomIdDict[uid];
            FightRoom room = GetRoom(roomId);
            return room;
        }

        public void Destroy(FightRoom room)
        {
            idRoomDict.Remove(room.Id);
            foreach(PlayerDto player in room.PlayerList)
            {
                uIdRoomIdDict.Remove(player.UserId);
            }
            //init room data
            room.PlayerList.Clear();
            room.TableCardList.Clear();
            room.libraryModel.Init();
            roomQueue.Enqueue(room);
        }

    }
}
