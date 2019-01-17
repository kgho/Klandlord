using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    public class AccountCode
    {
        //register  opreate code
        public const int REGIST_CREQ = 0; //client request ，account password
        public const int REGIST_SRES = 1;//server response

        //login opreate code
        public const int LOGIN = 2; //account password
    }
}
