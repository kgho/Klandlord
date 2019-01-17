using Assets.Scripts.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//class14

public class NetManager : ManagerBase
{
    public static NetManager Instance = null;

    public void Awake()
    {
        Instance = this;
        Add(0, this);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case 0:
                client.Send(message as SocketMsg);
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        Connected();
    }

    private ClientPeer client = new ClientPeer("127.0.0.1", 6666);
    public void Connected()
    {
        client.Connect();
    }

    //private void Update()
    //{
    //    if (client == null)
    //        return;
    //    while(client.SocketMsgQueue)
    //}
}

