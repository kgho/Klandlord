using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    [Serializable]
    public class MatchRoomDto
    {
        public Dictionary<int, UserDto> UserIdUserDtoDict;

        public List<int> ReadyUserIdList;

        //save the order of palyer enter
        public List<int> UserIdList;

        public MatchRoomDto()
        {
            this.UserIdUserDtoDict = new Dictionary<int, UserDto>();
            this.ReadyUserIdList = new List<int>();
            this.UserIdList = new List<int>();
        }

        public int LeftId;//left player id
        public int RightId;

        /// <summary>
        /// reset position
        /// when player enter or leave room
        /// </summary>
        /// <param name="myUserId"></param>
        public void ResetPosition(int myUserId)
        {
            LeftId = -1;
            RightId = -1;

            if (UserIdList.Count == 1)
            {

            }

            else if (UserIdList.Count == 2)
            {
                // x a
                if (UserIdList[0] == myUserId)
                {
                    RightId = UserIdList[1];
                }
                // x a
                if (UserIdList[1] == myUserId)
                {
                    LeftId = UserIdList[0];
                }
            }
            else if (UserIdList.Count == 3)
            {
                // x a b
                if (UserIdList[0] == myUserId)
                {
                    LeftId = UserIdList[2];
                    RightId = UserIdList[1];
                }
                // b x a
                if (UserIdList[1] == myUserId)
                {
                    LeftId = UserIdList[0];
                    RightId = UserIdList[2];
                }
                // a b x
                if (UserIdList[2] == myUserId)
                {
                    LeftId = UserIdList[1];
                    RightId = UserIdList[0];
                }
            }
        }
    }
}
