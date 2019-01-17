using AhpilyServer;
using System;

namespace Server
{
    //class01
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer server = new ServerPeer();
            server.Start(6666, 10);
            Console.ReadKey();
        }
    }
}
