using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//class14

public class NetManager : ManagerBase
{
    private void Start()
    {
        Connected();
    }

    private ClientPeer client = new ClientPeer("127.0.0.1", 6666);
    public void Connected()
    {
        client.Connect();
    }
}

