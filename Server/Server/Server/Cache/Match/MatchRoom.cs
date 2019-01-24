using AhpilyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cache.Match
{
    public class MatchRoom
    {
        public int Id { get; private set; }

        //the ID of user in room
        public Dictionary<int, ClientPeer> UserIdClientDict { get; private set; }

        public List<int> GetUIdList
        {
            get
            {
                return UserIdClientDict.Keys.ToList();
            }
        }

        //id list of user was ready
        public List<int> ReadyUserIdList { get; private set; }

        public MatchRoom(int id)
        {
            this.Id = id;
            this.UserIdClientDict = new Dictionary<int, ClientPeer>();
            this.ReadyUserIdList = new List<int>();
        }

        public bool IsFull()
        {
            return UserIdClientDict.Count == 3;
        }

        public bool IsEmpty()
        {
            return UserIdClientDict.Count == 0;
        }

        public bool IsAllReady()
        {
            return ReadyUserIdList.Count == 3;
        }

        public void Enter(int userId, ClientPeer client)
        {
            UserIdClientDict.Add(userId, client);
        }

        public void Leave(int userId)
        {
            UserIdClientDict.Remove(userId);
        }

        public void Ready(int userId)
        {
            ReadyUserIdList.Add(userId);
        }

        /// <summary>
        /// broadcast to all palyer in room
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="subCode"></param>
        /// <param name="value"></param>
        /// <param name="exClient"></param>
        public void Broadcast(int opCode, int subCode, object value, ClientPeer currentClient = null)
        {
            SocketMessage msg = new SocketMessage(opCode, subCode, value);
            byte[] data = EncodeTool.EncodeMsg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);

            foreach (var client in UserIdClientDict.Values)
            {
                if (client == currentClient)
                    continue;
                client.Send(packet);
            }
        }
    }
}
