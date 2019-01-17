﻿using AhpilyServer;
using Protocol.Code;
using Server.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class NetMsgCenter : IApplication
    {
        IHandler account = new AccountHandler();


        public void OnDisconnect(ClientPeer client)
        {
            throw new NotImplementedException();
        }

        public void OnRecive(ClientPeer client, SocketMessage msg)
        {
            switch (msg.OpCode)
            {
                case OpCode.ACCOUNT:
                    account.OnRecive(client, msg.SubCode, msg.Value);
                    break;
                default:
                    break;
            }
        }
    }
}