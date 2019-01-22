using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class UserModel
    {
        public int ID;
        public string Name;
        public int Been;

        public int WinCount;
        public int LoseCount;
        public int RunCount;

        public int LV;
        public int Exp;

        public int AccountModel;//Foreign Key:associate with account

        public UserModel(int id, string name, int accountId)
        {
            this.ID = id;
            this.Name = name;
            this.Been = 10000;
            this.WinCount = 0;
            this.LoseCount = 0;
            this.RunCount = 0;
            this.LV = 1;
            this.Exp = 0;
            this.AccountModel = accountId;
        }
    }
}
