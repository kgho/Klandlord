using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Net
{
    //network message
    public class SocketMsg
    {
        //opreate code
        public int OpCode { get; set; }
        //sub operate
        public int SubCode { get; set; }
        //parameter
        public object Value { get; set; }

        public SocketMsg()
        {

        }

        public SocketMsg(int opCode, int subCode, object value)
        {
            this.OpCode = opCode;
            this.SubCode = subCode;
            this.Value = value;
        }

        public void Change(int opCode, int subCode, object value)
        {
            this.OpCode = opCode;
            this.SubCode = subCode;
            this.Value = value;
        }

    }
}
