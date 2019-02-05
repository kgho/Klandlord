using AhpilyServer;
using Protocol.Constant;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cache.Fight
{
    public class FightRoom
    {
        public int Id { get; private set; }

        //save all player
        public List<PlayerDto> PlayerList { get; private set; }

        //card library
        public LibraryModel libraryModel { get; set; }
        //table card
        public List<CardDto> TableCardList { get; set; }
        //x1
        public int Multiple { get; set; }
        /// <summary>
        /// user id and user's connect object
        /// </summary>
        public Dictionary<int, ClientPeer> UIdClientDict { get; private set; }

        public void Init(List<int> uidList)
        {
            foreach (int uid in uidList)
            {
                PlayerDto player = new PlayerDto(uid);
                this.PlayerList.Add(player);
            }
        }

        /// <summary>
        /// constructor init fight room
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uidList"></param>
        public FightRoom(int id, List<int> uidList)
        {
            this.Id = id;
            this.PlayerList = new List<PlayerDto>();
            foreach (int uid in uidList)
            {
                PlayerDto player = new PlayerDto(uid);
                this.PlayerList.Add(player);
            }
            this.libraryModel = new LibraryModel();
            this.TableCardList = new List<CardDto>();
            this.Multiple = 1;
            this.UIdClientDict = new Dictionary<int, ClientPeer>();
            this.roundModel = new RoundModel();
        }

        /// <summary>
        /// start fight
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="client"></param>
        public void StartFight(int userId, ClientPeer client)
        {
            UIdClientDict.Add(userId, client);
        }

        /// <summary>
        /// deal cars,init user hands
        /// </summary>
        public void InitPlayerCards()
        {
            for (int i = 0; i < 17; i++)
            {
                CardDto card = libraryModel.Deal();
                PlayerList[0].Add(card);
            }
            for (int i = 0; i < 17; i++)
            {
                CardDto card = libraryModel.Deal();
                PlayerList[1].Add(card);
            }
            for (int i = 0; i < 17; i++)
            {
                CardDto card = libraryModel.Deal();
                PlayerList[2].Add(card);
            }
            for (int i = 0; i < 3; i++)
            {
                CardDto card = libraryModel.Deal();
                TableCardList.Add(card);
            }
        }

        /// <summary>
        /// sort hands
        /// </summary>
        /// <param name="cardList"></param>
        /// <param name="asc"></param>
        public void sortCard(List<CardDto> cardList, bool asc = true)
        {
            cardList.Sort(
                delegate (CardDto a, CardDto b)
                {
                    if (asc)
                        return a.Weight.CompareTo(b.Weight);
                    else
                        return a.Weight.CompareTo(b.Weight) * -1;
                });
        }

        /// <summary>
        /// default:Ascending
        /// </summary>
        /// <param name="asc"></param>
        public void Sort(bool asc = true)
        {
            sortCard(PlayerList[0].CardList, asc);
            sortCard(PlayerList[1].CardList, asc);
            sortCard(PlayerList[2].CardList, asc);
        }

        /// <summary>
        /// get user's hands by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CardDto> getUserCards(int userId)
        {
            foreach (PlayerDto player in PlayerList)
            {
                if (player.UserId == userId)
                    return player.CardList;
            }
            throw new Exception("Don't exist player id:" + userId);
        }

        public int GetFirstUserId()
        {
            return PlayerList[0].UserId;
        }

        public void SetLandlord(int userId)
        {
            foreach (PlayerDto player in PlayerList)
            {
                if (player.UserId == userId)
                {
                    //set this user to landlord
                    player.Identity = Identity.LANDLORD;
                    //give table cards to landlord
                    for (int i = 0; i < TableCardList.Count; i++)
                    {
                        player.Add(TableCardList[i]);
                    }
                    //sort hands after get new cards
                    this.Sort();
                }
            }
        }

        public int GetNextUserId(int currentUserId)
        {
            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (PlayerList[i].UserId == currentUserId)
                {
                    if (i == 2)
                        return PlayerList[0].UserId;
                    else
                        return PlayerList[i + 1].UserId;
                }
            }
            throw new Exception("No user to deal");
        }


        public RoundModel roundModel { get; set; }
        /// <summary>
        /// 判断能不能压上一回合的牌
        /// </summary>
        public bool DeadCard(int type, int weight, int length, int userId, List<CardDto> cardList)
        {
            bool canDeal = false;
            if (type == roundModel.LastCardType && weight > roundModel.LastWeight)
            {
                if (type == CardType.STRAIGHT || type == CardType.DOUBLE_STRAIGHT || type == CardType.TRIPLE_STRAIGHT)
                {
                    //length limit
                    if (length == roundModel.LastLength)
                    {
                        canDeal = true;
                    }
                }
                else
                {
                    canDeal = true;
                }
            }
            else if (type == CardType.BOOM && roundModel.LastCardType != CardType.BOOM)
            {
                canDeal = true;
            }
            else if (type == CardType.JOKER_BOOM)
            {
                canDeal = true;
            }
            else if (userId == roundModel.BiggestUserId)
            {
                canDeal = true;
            }

            if (canDeal)
            {
                //remove hands
                RemoveCards(userId, cardList);
                if (type == CardType.BOOM)
                    Multiple *= 2;
                else if (type == CardType.JOKER_BOOM)
                    Multiple *= 4;

                //save round infomation
                roundModel.Change(length, type, weight, userId);
            }
            return canDeal;
        }

        public void RemoveCards(int userId, List<CardDto> cardList)
        {
            //get player hands
            List<CardDto> currList = getUserCards(userId);

            List<CardDto> list = new List<CardDto>();
            foreach (var select in cardList)
            {
                for (int i = currList.Count - 1; i >= 0; i--)
                {
                    if (currList[i].Name == select.Name)
                    {
                        list.Add(currList[i]);
                        break;
                    }
                }
            }
            foreach (var card in list)
            {
                currList.Remove(card);
            }
        }

        /// <summary>
        /// get player data model
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PlayerDto GetPlayerModel(int userId)
        {
            foreach (PlayerDto player in PlayerList)
            {
                if (player.UserId == userId)
                {
                    return player;
                }
            }
            throw new Exception("No this player!Can't get data!");
        }

        /// <summary>
        /// change deal player
        /// </summary>
        /// <returns></returns>
        public int Turn()
        {
            int currentUserId = roundModel.CurrentUserId;
            int nextUserId = GetNextUserId(currentUserId);
            //change round information
            roundModel.CurrentUserId = nextUserId;
            return nextUserId;
        }
    }
}
