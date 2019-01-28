using Protocol.Constant;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cache.Fight
{
    public class LibraryModel
    {
        /// <summary>
        /// car library,all cards
        /// </summary>
        public Queue<CardDto> CardQueue { get; set; }

        public LibraryModel()
        {
            Create();
            Shuffle();
        }

        public void Init()
        {
            Create();
            Shuffle();
        }

        public void Create()
        {
            CardQueue = new Queue<CardDto>();
            //create 3 to 2 ,no jocker
            for (int suit = CardSuit.CLUB; suit <= CardSuit.DIAMOND; suit++)
            {
                for (int weight = CardWeight.THREE; weight <= CardWeight.TWO; weight++)
                {
                    string cardName = CardSuit.GetString(suit) + CardWeight.GetString(weight);
                    CardDto card = new CardDto(cardName, suit, weight);
                    CardQueue.Enqueue(card);
                }
                CardDto sJoker = new CardDto("SJoker", CardSuit.NONE, CardWeight.SJOKER);
                CardDto lJoker = new CardDto("LJoker", CardSuit.NONE, CardWeight.LJOKER);
                CardQueue.Enqueue(sJoker);
                CardQueue.Enqueue(lJoker);
            }
        }

        public void Shuffle()
        {
            List<CardDto> cardDto = new List<CardDto>();
            Random r = new Random();
            foreach (CardDto card in CardQueue)
            {
                int index = r.Next(0, cardDto.Count + 1);
                cardDto.Insert(index, card);
            }
            CardQueue.Clear();
            foreach (CardDto card in cardDto)
            {
                CardQueue.Enqueue(card);
            }
        }

        public CardDto Deal()
        {
            return CardQueue.Dequeue();
        }
    }
}
