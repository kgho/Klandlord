using AhpilyServer.Concurrent;
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

        public void Create(string account, string password)
        {
            AccountModel model = new AccountModel(id.Add_Get(), account, password);
            accModelDict.Add(model.Account, model);
        }
    }
}
