using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol.Code;
using Protocol.Dto;
using Server.Cache;

namespace Server.Logic
{
    public class AccountHandler : IHandler
    {
        AccountCache accountCache = Caches.Account;

        public void OnDisconnect(ClientPeer client)
        {
            throw new NotImplementedException();
        }

        public void OnRecive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case AccountCode.REGIST_CREQ:
                    {
                        AccountDto dto = value as AccountDto;
                        Console.Write(string.Format("Player register request... Account:{0},Password:{1}.", dto.Account, dto.Password));
                        Regist(client, dto.Account, dto.Password);
                    }
                    break;
                case AccountCode.LOGIN:
                    {
                        AccountDto dto = value as AccountDto;
                        Console.Write(string.Format("Player login request... Account:{0},Password:{1}.", dto.Account, dto.Password));
                        Login(client, dto.Account, dto.Password);
                    }
                    break;
                default:
                    break;
            }
        }

        private void Regist(ClientPeer client, string account, string password)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (accountCache.IsExist(account))
                {
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -1);
                    Console.WriteLine(string.Format("Error:Account already exist!"));
                    return;
                }
                if (string.IsNullOrEmpty(account))
                {
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -2);
                    Console.WriteLine(string.Format("Error:Account is invalid"));
                    return;
                }
                if (string.IsNullOrEmpty(password) || password.Length < 4 || password.Length > 16)
                {
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -3);
                    Console.WriteLine(string.Format("Error:Password is invalid"));
                    return;
                }

                accountCache.Create(account, password);
                client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, 0);
                Console.WriteLine(string.Format("Sign Up Successfully!"));
            });
        }

        private void Login(ClientPeer client, string account, string password)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!accountCache.IsExist(account))
                {
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -1);
                    Console.WriteLine(string.Format("Error:Account doesn't exist!"));
                    return;
                }
                if (!accountCache.IsMatch(account, password))
                {
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -2);
                    Console.WriteLine(string.Format("Error:Password dosen't match!"));
                    return;
                }
                if (accountCache.IsOnline(account))
                {
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -3);
                    Console.WriteLine(string.Format("Error:Account is online!"));
                    return;
                }

                accountCache.OnLine(client, account);
                client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, 0);
                Console.WriteLine(string.Format("Sign in Successfully!"));
            });
        }
    }
}
