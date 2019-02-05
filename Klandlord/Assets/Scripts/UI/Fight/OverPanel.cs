using Protocol.Constant;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverPanel : UIBase
{

    private void Awake()
    {
        Bind(UIEvent.SHOW_OVER_PANEL);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SHOW_OVER_PANEL:
                RefreshShow(message as OverDto);
                break;
            default:
                break;
        }
    }

    private Text txtWinIdentity;
    private Text txtWinBeen;

    void Start()
    {
        txtWinIdentity = transform.Find("TextWinIdentity").GetComponent<Text>();
        txtWinBeen = transform.Find("TextWinBean").GetComponent<Text>();

        setPanelActive(false);
    }

    private void RefreshShow(OverDto dto)
    {
        setPanelActive(true);
        //显示谁胜利
        txtWinIdentity.text = Identity.GetString(dto.WinIdentity);
        //判断自己是否胜利
        if (dto.WinUserIdList.Contains(Models.GameModel.Id))
        {
            txtWinIdentity.text += "Win";
            txtWinBeen.text = "Bean：+";
        }
        else
        {
            txtWinIdentity.text += "Lose";
            txtWinBeen.text = "Bean：-";
        }
        //显示豆子数量
        txtWinBeen.text += dto.BeanCount;
    }


}
