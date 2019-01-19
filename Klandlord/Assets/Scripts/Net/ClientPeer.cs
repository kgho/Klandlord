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

            startReceive();

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }


    #region Receive Data

    //received data Buffer
    private byte[] receiveBuffer = new byte[1024];

    /// <summary>
    /// once reveive data ,save to the cache
    /// </summary>
    private List<byte> dataCache = new List<byte>();

    private bool isProcessReceive = false;

    public Queue<SocketMsg> SocketMsgQueue = new Queue<SocketMsg>();

    private void startReceive()
    {
        if (socket == null && socket.Connected == false)
        {
            Debug.LogError("no connect,can't send message");
            return;
        }
        socket.BeginReceive(receiveBuffer, 0, 1024, SocketFlags.None, receiveCallBack, socket);
    }

    /// <summary>
    /// receive data callback
    /// </summary>
    /// <param name="ar"></param>
    private void receiveCallBack(IAsyncResult ar)
    {
        try
        {
            int length = socket.EndReceive(ar);
            byte[] tempByteArray = new byte[length];
            Buffer.BlockCopy(receiveBuffer, 0, tempByteArray, 0, length);


            dataCache.AddRange(tempByteArray);
            if (isProcessReceive == false)
                processReceive();

            startReceive();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// process the receive data
    /// </summary>
    private void processReceive()
    {
        isProcessReceive = true;
        //decode data
        byte[] data = EncodeTool.DecodePacket(ref dataCache);

        if (data == null)
        {
            isProcessReceive = false;
            return;
        }

        SocketMsg msg = EncodeTool.DecodeMsg(data);
        //save data ,wait process
        SocketMsgQueue.Enqueue(msg);
        Debug.Log(msg.Value);

        //尾递归
        processReceive();
    }

    #endregion

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
