using GameServer.Server.Cache;

namespace Server.Cache
{
    public class Caches
    {
        public static AccountCache Account { get; set; }
        public static UserCache User { get; set; }
        public static MatchCache Match { get; set; }

        static Caches()
        {
            Account = new AccountCache();
            User = new UserCache();
            Match = new MatchCache();
        }
    }
}
