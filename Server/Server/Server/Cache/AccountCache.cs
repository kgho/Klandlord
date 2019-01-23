using AhpilyServer;
using AhpilyServer.Concurrent;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cache
{
    /// <summary>
    /// 账号缓存
    /// </summary>
    public class AccountCache
    {
        //账号的数据模型
        private Dictionary<string, AccountModel> accModelDict = new Dictionary<string, AccountModel>();
        /// <summary>
        /// 存储账号的ID
        /// 数据库用于自增处理
        /// </summary>
        private ConcurrentInt id = new ConcurrentInt(-1);

        //账号对应连接对象
        private Dictionary<string, ClientPeer> accClientDict = new Dictionary<string, ClientPeer>();
        private Dictionary<ClientPeer, string> clientAccDict = new Dictionary<ClientPeer, string>();

        /// <summary>
        /// whether Account is online
        /// </summary>
        public bool IsOnline(string account)
        {
            return accClientDict.ContainsKey(account);
        }

        /// <summary>
        /// whether Server Connect Object is online
        /// </summary>
        public bool IsOnline(ClientPeer client)
        {
            return clientAccDict.ContainsKey(client);
        }

        /// <summary>
        /// player online
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        public void OnLine(ClientPeer client, string account)
        {
            accClientDict.Add(account, client);
            clientAccDict.Add(client, account);
        }

        /// <summary>
        /// player offline
        /// </summary>
        /// <param name="client"></param>
        public void OffLine(ClientPeer client)
        {
            string account = clientAccDict[client];
            clientAccDict.Remove(client);
            accClientDict.Remove(account);
        }

        public void OffLine(string account)
        {
            ClientPeer client = accClientDict[account];
            clientAccDict.Remove(client);
            accClientDict.Remove(account);
        }

        public void Create(string account, string password)
        {
            AccountModel model = new AccountModel(id.Add_Get(), account, password);
            accModelDict.Add(model.Account, model);
        }

        public bool IsExist(string account)
        {
            return accModelDict.ContainsKey(account);
        }

        public bool IsMatch(string account, string password)
        {
            AccountModel model = accModelDict[account];
            return model.Password == password;
        }

        /// <summary>
        /// Get online-player ID
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetId(ClientPeer client)
        {
            string account = clientAccDict[client];
            AccountModel model = accModelDict[account];
            return model.Id;
        }

        /// <summary>
        /// Get online-player number
        /// </summary>
        /// <returns></returns>
        public int GetOnlineNum()
        {
            return accClientDict.Count();
        }
    }
}
