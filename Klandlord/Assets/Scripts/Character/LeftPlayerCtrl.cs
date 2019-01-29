using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LeftPlayerCtrl : CharacterBase
{
    private void Awake()
    {
        Bind(CharacterEvent.INIT_LEFT_CARD,
            CharacterEvent.ADD_LEFT_CARD);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.INIT_LEFT_CARD:
                StartCoroutine(initCardList());
                break;
            case CharacterEvent.ADD_LEFT_CARD:
                addTableCard();
                break;
            default:
                break;
        }
    }

    private List<GameObject> cardObjectList;
    private Transform cardParent;
    private void Start()
    {
        cardParent = transform.Find("CardPoint");
        cardObjectList = new List<GameObject>();
    }

    private IEnumerator initCardList()
    {
        GameObject cardPrefab = Resources.Load<GameObject>("Card/OtherCard");
        for (int i = 0; i < 17; i++)
        {
            createGo(i, cardPrefab);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void createGo(int index, GameObject cardPrefab)
    {
        GameObject cardGo = Instantiate(cardPrefab, cardParent) as GameObject;
        cardGo.transform.localPosition = new Vector2((-0.15f * index), 0);
        cardGo.GetComponent<SpriteRenderer>().sortingOrder = index;
        this.cardObjectList.Add(cardGo);
    }

    private void addTableCard()
    {
        GameObject cardPrefab = Resources.Load<GameObject>("Card/OtherCard");
        for (int i = 0; i < 3; i++)
        {
            createGo(i, cardPrefab);
        }
    }
}

