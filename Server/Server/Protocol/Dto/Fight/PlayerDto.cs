using Protocol.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class PlayerDto
    {
        public int UserId;
        public int Idnetity;//farmer or landlord
        public List<CardDto> CardList;//player hands

        public PlayerDto(int userId)
        {
            Idnetity = Identity.FARMER;
            UserId = userId;
            CardList = new List<CardDto>();
        }

        /// <summary>
        /// add card to hands
        /// </summary>
        /// <param name="card"></param>
        public void Add(CardDto card)
        {
            CardList.Add(card);
        }
    }
}
