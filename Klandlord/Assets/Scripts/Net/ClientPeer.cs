using Assets.Scripts.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/// <summary>
///class13 encapsulate Server Socket
/// </summary>
public class ClientPeer
{
    private Socket socket;

    private string ip;
    private int port;

    //construct Connect Object
    public ClientPeer(string ip, int port)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ip = ip;
            this.port = port;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void Connect()
    {
        try
        {
            socket.Connect(ip, port);
            Debug.Log("connect server successful!");

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }



    /// <summary>
    /// send message
    /// </summary>
    /// <param name="opCode"></param>
    /// <param name="subCode"></param>
    /// <param name="value"></param>
    public void Send(int opCode, int subCode, object value)
    {
        SocketMsg msg = new SocketMsg(opCode, subCode, value);

        Send(msg);
    }

    public void Send(SocketMsg msg)
    {
        byte[] data = EncodeTool.EncodeMsg(msg);
        byte[] packet = EncodeTool.EncodePacket(data);

        try
        {
            socket.Send(packet);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
