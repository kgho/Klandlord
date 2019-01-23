using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol.Code;
using Protocol.Dto;
using Server.Cache;
using Server.Model;

namespace Server.Logic
{
    public class UserHandler : IHandler
    {

        public void OnDisconnect(ClientPeer client)
        {
            throw new NotImplementedException();
        }

        public void OnRecive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case UserCode.GET_INFO_CREQ:
                    GetInfo(client);
                    break;
                case UserCode.CREATE_CREQ:
                    Create(client, value.ToString());
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get User Info
        /// </summary>
        /// <param name="client"></param>
        private void GetInfo(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!accountCache.IsOnline(client))
                {
                    client.Send(OpCode.USER, UserCode.GET_INFO_SRES, null);
                    Console.WriteLine("Get User Info---User Online");
                    return;
                }
                int accountId = accountCache.GetId(client);
                if ((userCache.isExist(accountId)) == false)
                {
                    client.Send(OpCode.USER, UserCode.GET_INFO_SRES, null);
                    Console.WriteLine("Get User Info---No User Info");
                    return;
                }
                //user online
                if (userCache.IsOnline(client) == false)
                    Online(client);
                UserModel model = userCache.GetModelByAccountId(accountId);
                UserDto dto = new UserDto(model.ID, model.Name, model.Been, model.WinCount, model.LoseCount, model.RunCount, model.LV, model.Exp);
                client.Send(OpCode.USER, UserCode.GET_INFO_SRES, dto);
            });
        }

        /// <summary>
        /// online
        /// </summary>
        /// <param name="client"></param>
        private void Online(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!accountCache.IsOnline(client))
                {
                    client.Send(OpCode.USER, UserCode.GET_INFO_SRES, -1);
                    Console.WriteLine("Online----Alreay Online");
                    return;
                }
                int accountId = accountCache.GetId(client);
                if (userCache.isExist(accountId) == false)
                {
                    client.Send(OpCode.USER, UserCode.ONLINE_SRES, -2);
                    Console.WriteLine("Online----User Don't Exist");
                    return;
                }
                int userId = userCache.GetId(accountId);
                userCache.Online(client, userId);
                client.Send(OpCode.USER, UserCode.ONLINE_SRES, 0);
                Console.WriteLine(string.Format("Online----Successfully,current online-player number:{0}", accountCache.GetOnlineNum()));
            });
        }

        private AccountCache accountCache = Caches.Account;
        private UserCache userCache = Caches.User;

        private void Create(ClientPeer client, string name)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!accountCache.IsOnline(client))
                {
                    client.Send(OpCode.USER, UserCode.CREATE_SRES, -1);
                    Console.WriteLine("Create User--Illegal Login");
                    return;
                }
                int accountId = accountCache.GetId(client);
                if (userCache.isExist(accountId))
                {
                    client.Send(OpCode.USER, UserCode.CREATE_SRES, -2);
                    Console.WriteLine("Create User--Repeat Create");
                    return;
                }
                if (userCache.isRepeatName(name))
                {
                    client.Send(OpCode.USER, UserCode.CREATE_SRES, -3);
                    Console.WriteLine("Create User--Name Is Exist");
                    return;
                }
                userCache.Create(name, accountId);
                client.Send(OpCode.USER, UserCode.CREATE_SRES, 0);
                Console.WriteLine("Create User--Create Successfully");
            });
        }
    }
}
