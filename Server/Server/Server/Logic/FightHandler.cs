using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol.Code;
using Protocol.Dto.Fight;
using Server.Cache;
using Server.Cache.Fight;

namespace Server.Logic
{
    public class FightHandler : IHandler
    {
        public void OnDisconnect(ClientPeer client)
        {
            throw new NotImplementedException();
        }

        public void OnRecive(ClientPeer client, int subCode, object value)
        {
            throw new NotImplementedException();
        }

        public FightCache fightCache = Caches.Fight;
        public UserCache userCache = Caches.User;

        /// <summary>
        /// start play cards
        /// </summary>
        /// <param name="userIdList"></param>
        public void startFight(List<int> userIdList)
        {
            SingleExecute.Instance.Execute(() =>
            {
                //create fight room
                FightRoom room = fightCache.Create(userIdList);
                room.InitPlayerCards();
                room.Sort();
                //send all client ,what cards he have;
                foreach (int uid in userIdList)
                {
                    ClientPeer client = userCache.GetClientPeer(uid);
                    List<CardDto> cardList = room.getUserCards(uid);
                    client.Send(OpCode.FIGHT, FightCode.GET_CARD_SRES, cardList);
                }
            });
        }
    }
}
