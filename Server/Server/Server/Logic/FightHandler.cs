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
using Server.Model;

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
                case FightCode.DEAL_CREQ:
                    deal(client, value as DealDto);
                    break;
                case FightCode.PASS_CREQ:
                    pass(client);
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
                    Broadcast(fightRoom, OpCode.FIGHT, FightCode.TURN_DEAL_BROADCAST, userId);
                }
                else
                {
                    //don't grab 
                    int nextUserId = fightRoom.GetNextUserId(userId);
                    Broadcast(fightRoom, OpCode.FIGHT, FightCode.TURN_GRAB_BROADCAST, nextUserId);
                }
            });
        }

        private void deal(ClientPeer client, DealDto dto)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!userCache.IsOnline(client))
                    return;
                int userId = userCache.GetId(client);
                if (userId != dto.UserId)
                {
                    return;
                }
                FightRoom fightRoom = fightCache.GetRoomByUId(userId);

                bool canDeal = fightRoom.DeadCard(dto.Type, dto.Weight, dto.Length, dto.UserId, dto.selectCardList);
                if (canDeal == false)
                {
                    client.Send(OpCode.FIGHT, FightCode.DEAL_SRES, -1);
                    return;
                }
                else
                {
                    //send deal successful message
                    client.Send(OpCode.FIGHT, FightCode.DEAL_SRES, 0);

                    //check reamin hands
                    List<CardDto> remainCard = fightRoom.GetPlayerModel(userId).CardList;

                    dto.RemainCardList = remainCard;
                    Broadcast(fightRoom, OpCode.FIGHT, FightCode.DEAL_BROADCAST, dto);
                    if (remainCard.Count == 0)
                    {
                        Console.WriteLine("Game Over");
                        gameOver(userId, fightRoom);
                    }
                    else
                    {
                        Turn(fightRoom);
                    }
                }
            });
        }

        private void pass(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!userCache.IsOnline(client))
                    return;
                int userId = userCache.GetId(client);
                FightRoom fightRoom = fightCache.GetRoomByUId(userId);

                if (fightRoom.roundModel.BiggestUserId == userId)
                {
                    client.Send(OpCode.FIGHT, FightCode.PASS_SRES, -1);
                    return;
                }
                client.Send(OpCode.FIGHT, FightCode.PASS_SRES, 0);
                Turn(fightRoom);
            });
        }

        //change deal player
        private void Turn(FightRoom room)
        {
            int nextUserId = room.Turn();
            Broadcast(room, OpCode.FIGHT, FightCode.TURN_DEAL_BROADCAST, nextUserId);
        }

        private void gameOver(int userId, FightRoom room)
        {
            int winIdentity = room.GetPlayerIdentity(userId);
            int winBean = room.Multiple * 1000;
            List<int> winUserIdList = room.GetSameIdentityUserId(winIdentity);

            //win player pluas win count
            for (int i = 0; i < winUserIdList.Count; i++)
            {
                UserModel um = userCache.GetModelByAccountId(winUserIdList[i]);
                um.WinCount++;
                um.Been = winBean;
                um.Exp = um.LV * 100;
                int maxExp = um.LV * 100;
                while (maxExp <= um.Exp)
                {
                    um.LV++;
                    um.Exp -= maxExp;
                }
                userCache.Update(um);
            }
            //add lose count to loser
            List<int> loseUIds = room.GetDifferentIdentityUserId(winIdentity);
            for (int i = 0; i < loseUIds.Count; i++)
            {
                UserModel um = userCache.GetModelById(loseUIds[i]);
                um.LoseCount++;
                um.Been -= winBean;
                um.Exp += 10;
                int maxExp = um.LV * 100;
                while (maxExp <= um.Exp)
                {
                    um.LV++;
                    um.Exp -= maxExp;
                }
                userCache.Update(um);
            }
            OverDto dto = new OverDto();
            dto.WinIdentity = winIdentity;
            dto.WinUserIdList = winUserIdList;
            dto.BeanCount = winBean;
            Broadcast(room, OpCode.FIGHT, FightCode.Over_BROADCAST, dto);

            fightCache.Destroy(room);
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
