﻿using System;
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
            switch (subCode)
            {
                case FightCode.GRAB_LANDLORD_CREQ:
                    bool result = (bool)value;
                    grabLandlord(client, result);
                    break;
                default:
                    break;
            }
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

                //start grab landlord
                int firstUserId = room.GetFirstUserId();
                //tell all client firstUserId user to grab landlord
                Broadcast(room, OpCode.FIGHT, FightCode.TURN_GRAB_BROADCAST, firstUserId, null);
            });
        }

        private void grabLandlord(ClientPeer client, bool result)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!userCache.IsOnline(client))
                    return;
                int userId = userCache.GetId(client);
                FightRoom fightRoom = fightCache.GetRoomByUId(userId);

                if (result == true)
                {
                    fightRoom.SetLandlord(userId);
                    //send message to all client who was landlord
                    GrabDto dto = new GrabDto(userId, fightRoom.TableCardList, fightRoom.getUserCards(userId));
                    Broadcast(fightRoom, OpCode.FIGHT, FightCode.GRAB_LANDLORD_BROADCAST, dto);

                    //enter deal phase
                }
                else
                {
                    //don't grab 
                    int nextUserId = fightRoom.GetNextUserId(userId);
                    Broadcast(fightRoom, OpCode.FIGHT, FightCode.TURN_GRAB_BROADCAST, nextUserId);
                }
            });
        }

        public void Broadcast(FightRoom room, int opCode, int subCode, object value, ClientPeer currentClient = null)
        {
            SocketMessage msg = new SocketMessage(opCode, subCode, value);
            byte[] data = EncodeTool.EncodeMsg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);

            foreach (var player in room.PlayerList)
            {
                if (userCache.IsOnline(player.UserId))
                {
                    ClientPeer client = userCache.GetClientPeer(player.UserId);
                    if (client == currentClient)
                        continue;
                    client.Send(packet);
                }
            }
        }
    }
}