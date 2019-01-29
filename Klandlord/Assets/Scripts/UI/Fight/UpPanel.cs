using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpPanel : UIBase
{

    private void Awake()
    {
        Bind(UIEvent.SET_TABLE_CARD);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SET_TABLE_CARD:
                SetTableCards(message as List<CardDto>);
                break;
            default:
                break;
        }
    }

    private Image[] imgCards = null;
    // Use this for initialization
    void Start()
    {
        imgCards = new Image[3];
        imgCards[0] = transform.Find("ImageCard1").GetComponent<Image>();
        imgCards[1] = transform.Find("ImageCard2").GetComponent<Image>();
        imgCards[2] = transform.Find("ImageCard3").GetComponent<Image>();

    }

    private void SetTableCards(List<CardDto> cards)
    {
        imgCards[0].sprite = Resources.Load<Sprite>("Poker/" + cards[0].Name);
        imgCards[1].sprite = Resources.Load<Sprite>("Poker/" + cards[1].Name);
        imgCards[2].sprite = Resources.Load<Sprite>("Poker/" + cards[2].Name);
    }
}
