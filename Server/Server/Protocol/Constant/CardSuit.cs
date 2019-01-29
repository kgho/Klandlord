using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{
    public class CardSuit
    {
        public const int NONE = 0;
        public const int CLUB = 1;
        public const int HEART = 2;
        public const int SPADE = 3;
        public const int DIAMOND = 4;

        public static string GetString(int suit)
        {
            switch (suit)
            {
                case CLUB:
                    return "Club";
                case HEART:
                    return "Heart";
                case SPADE:
                    return "Spade";
                case DIAMOND:
                    return "Diamond";
                default:
                    throw new Exception("Don't exist:" + "suit");
            }
        }
    }
}
