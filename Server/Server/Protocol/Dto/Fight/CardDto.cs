using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class CardDto
    {
        public string Name;
        public int Suit;//spade heart diamond club
        public int Weight; //3 J Q

        public CardDto()
        {

        }

        public CardDto(string name, int suit, int weight)
        {
            this.Name = name;
            this.Suit = suit;
            this.Weight = weight;
        }
    }
}
