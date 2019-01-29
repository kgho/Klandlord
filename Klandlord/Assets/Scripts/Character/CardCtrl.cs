using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCtrl : MonoBehaviour
{

    //card data
    public CardDto cardDto { get; private set; }

    private SpriteRenderer spriteRenderer;

    private bool isMine;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// init card data
    /// </summary>
    /// <param name="card"></param>
    /// <param name="index"></param>
    /// <param name="isMine"></param>
    public void Init(CardDto card, int index, bool isMine)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.cardDto = card;
        this.isMine = isMine;

        string resPath = string.Empty;
        if (isMine)
        {
            resPath = "Pocker/" + card.Name;
        }
        else
        {
            resPath = "Poker/CardBack";
        }
        Sprite sp = Resources.Load<Sprite>(resPath);
        spriteRenderer.sprite = sp;
        //spriteRenderer.sprite = Resources.Load(resPath, typeof(Sprite)) as Sprite;
        spriteRenderer.sortingOrder = index;
    }
}
