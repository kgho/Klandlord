using Protocol.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class DealDto
    {
        public List<CardDto> selectCardList;

        public int Length;

        public int Weight;

        public int Type;

        public int UserId;

        public bool IsRegular;

        public List<CardDto> RemainCardList;

        public DealDto()
        {

        }

        public DealDto(List<CardDto> cardList, int UserId)
        {
            this.selectCardList = cardList;
            this.Length = cardList.Count;
            this.Type = CardType.GetCardType(cardList);
            this.Weight = CardWeight.GetWeight(cardList, this.Type);
            this.UserId = UserId;
            this.IsRegular = (this.Type != CardType.NONE);
            this.RemainCardList = new List<CardDto>();
        }
    }
}
