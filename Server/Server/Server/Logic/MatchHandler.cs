using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol.Code;
using Protocol.Dto;
using Server.Cache;
using Server.Cache.Match;
using Server.Model;

namespace Server.Logic
{
    public class MatchHandler : IHandler
    {
        private UserCache userCache = Caches.User;
        private MatchCache matchCache = Caches.Match;

        public void OnDisconnect(ClientPeer client)
        {
            throw new NotImplementedException();
        }

        public void OnRecive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case MatchCode.ENTER_CREQ:
                    enter(client);
                    break;
                default:
                    break;
            }
        }

        public void enter(ClientPeer client)
        {
            SingleExecute.Instance.Execute(delegate ()
            {
                if (!userCache.IsOnline(client))
                    return;
                int userId = userCache.GetId(client);
                if (matchCache.IsMatching(userId))
                {
                    //user is matching,matching=already enter room 
                    client.Send(OpCode.MATCH, MatchCode.ENTER_SRES, -1);
                    return;
                }
                MatchRoom room = matchCache.Enter(userId, client);
                //broadcast to all user in room except current client,

                UserModel model = userCache.GetModelByAccountId(userId);
                UserDto userDto = new UserDto(model.ID, model.Name, model.Been, model.WinCount, model.LoseCount, model.RunCount, model.LV, model.Exp);

                room.Broadcast(OpCode.MATCH, MatchCode.ENTER_BORADCAST, userDto, client);


                MatchRoomDto dto = makeRoomDto(room);
                client.Send(OpCode.MATCH, MatchCode.ENTER_SRES, dto);
                Console.WriteLine("Player enter room......");
            });
        }


        private MatchRoomDto makeRoomDto(MatchRoom room)
        {
            MatchRoomDto dto = new MatchRoomDto();
            foreach (var uid in room.UserIdClientDict.Keys)
            {
                UserModel model = userCache.GetModelByAccountId(uid);
                UserDto userDto = new UserDto(model.ID, model.Name, model.Been, model.WinCount, model.LoseCount, model.RunCount, model.LV, model.Exp);
                dto.UserIdUserDtoDict.Add(uid, userDto);
                //fix bug
                dto.UserIdList.Add(uid);
            }
            dto.ReadyUserIdList = room.ReadyUserIdList;
            return dto;
        }
    }
}
