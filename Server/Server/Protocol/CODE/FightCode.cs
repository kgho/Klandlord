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


        /// <summary>
        /// //server send client get hands
        /// </summary>
        public const int GET_CARD_SRES = 11;
    }
}
