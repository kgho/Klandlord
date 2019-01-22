using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol.Code;
using Server.Cache;

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
                case UserCode.CREATE_CREQ:
                    Create
            }
        }

        private AccountCache accountCache;
        private UserCache userCache;

        private void Create(ClientPeer client,string name)
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
                if(userCache)
            });
        }
    }
}
