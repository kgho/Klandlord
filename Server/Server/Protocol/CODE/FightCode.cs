using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    public class FightCode
    {

        public const int TURN_GRAB_BROADCAST = 0;
        public const int GRAB_LANDLORD_CREQ = 1;//grab landlord
        public const int GRAB_LANDLORD_BROADCAST = 2;//broadcast grab landlord result
        public const int TURN_DEAL_BROADCAST = 3;
        public const int DEAL_CREQ = 4;
        public const int DEAL_SRES = 5;
        public const int DEAL_BROADCAST = 6;//broadcast deal result
        public const int PASS_CREQ = 7;
        public const int PASS_SRES = 8;
        public const int Over_BROADCAST = 9;

        /// <summary>
        /// //server send client get hands
        /// </summary>
        public const int GET_CARD_SRES = 11;
    }
}
