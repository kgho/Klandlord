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
    }
}
