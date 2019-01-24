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
    public class UserCache
    {
        //User ID - UserModel
        private Dictionary<int, UserModel> idModelDict = new Dictionary<int, UserModel>();

        //Account ID - User ID
        private Dictionary<int, int> accIdUserIdDict = new Dictionary<int, int>();

        ConcurrentInt id = new ConcurrentInt(-1);

        public void Create(string name, int accountId)
        {
            UserModel model = new UserModel(id.Add_Get(), name, accountId);
            idModelDict.Add(model.ID, model);
            accIdUserIdDict.Add(model.AccountModel, model.ID);
        }


        public bool isRepeatName(string name)
        {
            foreach (UserModel um in idModelDict.Values)
            {
                if (um.Name == name)
                    return true;
            }
            return false;
        }

        public bool isExist(int accountId)
        {
            return accIdUserIdDict.ContainsKey(accountId);
        }

        //根据账号ID后去角色数据模型
        public UserModel GetModelByAccountId(int accountId)
        {
            int userId = accIdUserIdDict[accountId];
            UserModel model = idModelDict[userId];
            return model;
        }

        //根据账号ID获取角色ID
        public int GetId(int accountId)
        {
            return accIdUserIdDict[accountId];
        }

        public int GetId(ClientPeer client)
        {
            return clientIdDict[client];
        }

        //save online-palyer,only online-player have Client object
        private Dictionary<int, ClientPeer> idClientDict = new Dictionary<int, ClientPeer>();
        private Dictionary<ClientPeer, int> clientIdDict = new Dictionary<ClientPeer, int>();

        //weheather player is online
        public bool IsOnline(ClientPeer client)
        {
            return clientIdDict.ContainsKey(client);
        }
        public bool IsOnline(int id)
        {
            return idClientDict.ContainsKey(id);
        }

        //player online
        public void Online(ClientPeer client, int id)
        {
            idClientDict.Add(id, client);
            clientIdDict.Add(client, id);
        }
    }
}
