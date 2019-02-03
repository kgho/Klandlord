using Assets.Scripts.Net;
using Protocol.Code;
using Protocol.Dto.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MyPlayerCtrl : CharacterBase
{
    private void Awake()
    {
        Bind(CharacterEvent.INIT_MY_CARD,
            CharacterEvent.ADD_MY_CARD,
            CharacterEvent.DEAL_CARD,
            CharacterEvent.REMOVE_MY_CARD);
    }

    private Transform cardParent;

    private List<CardCtrl> cardCtrlList;

    private void Start()
    {
        cardParent = transform.Find("CardPoint");
        cardCtrlList = new List<CardCtrl>();
        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.INIT_MY_CARD:
                print(message.ToString());
                StartCoroutine(initCardList(message as List<CardDto>));
                break;
            case CharacterEvent.ADD_MY_CARD:
                addTableCard(message as GrabDto);
                break;
            case CharacterEvent.DEAL_CARD:
                dealSelectCard();
                break;
            case CharacterEvent.REMOVE_MY_CARD:
                removeCard(message as List<CardDto>);
                break;
            default:
                break;
        }
    }

    private IEnumerator initCardList(List<CardDto> cardList)
    {
        GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
        for (int i = 0; i < cardList.Count; i++)
        {
            createGo(cardList[i], i, cardPrefab);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void createGo(CardDto card, int index, GameObject cardPrefab)
    {
        GameObject cardGo = Instantiate(cardPrefab, cardParent) as GameObject;
        cardGo.name = card.Name;
        cardGo.transform.localPosition = new Vector2(0.2f * index, 0);
        CardCtrl cardCtrl = cardGo.GetComponent<CardCtrl>();
        cardCtrl.Init(card, index, true);

        this.cardCtrlList.Add(cardCtrl);
    }

    private void addTableCard(GrabDto dto)
    {
        List<CardDto> tabelCards = dto.TableCardList;
        List<CardDto> playerCards = dto.PlayerCardList;

        // reuse the cards create before
        int index = 0;
        foreach (var cardCtrl in cardCtrlList)
        {
            cardCtrl.gameObject.SetActive(true);
            cardCtrl.Init(playerCards[index], index, true);
        }

        //create 3 new card
        GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
        for (int i = index; i < playerCards.Count; i++)
        {
            createGo(playerCards[i], i, cardPrefab);
        }
    }

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;
    private void dealSelectCard()
    {
        List<CardDto> selectCardList = getSelectedCard();
        DealDto dto = new DealDto(selectCardList, Models.GameModel.Id);
        //
        if (dto.IsRegular == false)
        {
            promptMsg.Change("Please select right hands", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        else
        {
            socketMsg.Change(OpCode.FIGHT, FightCode.DEAL_CREQ, dto);
            Dispatch(AreaCode.NET, 0, socketMsg);
        }
    }

    private List<CardDto> getSelectedCard()
    {
        List<CardDto> selectCardList = new List<CardDto>();
        foreach (var cardCtrl in cardCtrlList)
        {
            if (cardCtrl.selected == true)
            {
                selectCardList.Add(cardCtrl.cardDto);
            }
        }
        return selectCardList;
    }

    private void removeCard(List<CardDto> remainCardList)
    {
        int index = 0;
        foreach (var cardCtrl in cardCtrlList)
        {
            if (remainCardList.Count == 0)
                break;
            else
            {
                cardCtrl.gameObject.SetActive(true);
                cardCtrl.Init(remainCardList[index], index, true);
                index++;
                if (index == remainCardList.Count)
                {
                    break;
                }
            }
        }
        //hide cards whitch index is bigger than index
        for (int i = index; i < cardCtrlList.Count; i++)
        {
            cardCtrlList[i].selected = false;
            cardCtrlList[i].gameObject.SetActive(false);
        }
    }
}

