using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Assets.Scripts.Net
{
    public class EncodeTool
    {
        /// <summary>
        /// 粘包
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] EncodePacket(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(data.Length);
                    bw.Write(data);

                    byte[] byteArrary = new byte[(int)ms.Length];
                    Buffer.BlockCopy(ms.GetBuffer(), 0, byteArrary, 0, (int)ms.Length);

                    return byteArrary;
                }
            }
        }

        public static byte[] DecodePacket(ref List<byte> dataCache)
        {
            if (dataCache.Count < 4)
                return null;
            using (MemoryStream ms = new MemoryStream(dataCache.ToArray()))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int length = br.ReadInt32();
                    int dataRemainLength = (int)(ms.Length - ms.Position);
                    if (length > dataRemainLength)
                        return null;
                    //throw new Exception("数据长度不够约定的长度 不能构成一个完整的消息");

                    byte[] data = br.ReadBytes(length);

                    //update data cache
                    dataCache.Clear();
                    dataCache.AddRange(br.ReadBytes(dataRemainLength));

                    return data;

                }
            }
        }

        /// <summary>
        /// 构造发送的SocketMessage类
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] EncodeMsg(SocketMsg msg)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(msg.OpCode);
            bw.Write(msg.SubCode);
            if (msg.Value != null)
            {
                byte[] valueBytes = EncodeObj(msg.Value);
                bw.Write(valueBytes);
            }

            byte[] data = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, (int)ms.Length);

            bw.Close();
            ms.Close();
            return data;
        }

        public static SocketMsg DecodeMsg(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryReader br = new BinaryReader(ms);
            SocketMsg msg = new SocketMsg();
            msg.OpCode = br.ReadInt32();
            msg.SubCode = br.ReadInt32();
            //remain data
            if (ms.Length > ms.Position)
            {
                byte[] valueBytes = br.ReadBytes((int)(ms.Length - ms.Position));
                object value = DecodeObj(valueBytes);
                msg.Value = value;
            }
            br.Close();
            ms.Close();
            return msg;
        }

        /// <summary>
        /// 把一个类转换为byte[]
        /// </summary>
        public static byte[] EncodeObj(object value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, value);
                byte[] valueBytes = new byte[ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, valueBytes, 0, (int)ms.Length);
                return valueBytes;
            }
        }

        public static object DecodeObj(byte[] valueBytes)
        {
            using (MemoryStream ms = new MemoryStream(valueBytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object value = bf.Deserialize(ms);
                return value;
            }
        }
    }
}
