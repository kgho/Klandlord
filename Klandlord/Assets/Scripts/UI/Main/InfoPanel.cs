﻿using Assets.Scripts.Net;
using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : UIBase
{
    private Text txtName;
    private Text txtLv;
    private Slider sldExp;
    private Text txtExp;
    private Text txtBeen;
    private Button BtnMatch;
    private Button BtnSetting;

    private SocketMsg socketMsg;

    public void Awake()
    {
        Bind(UIEvent.REFRESH_INFO_PANEL);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REFRESH_INFO_PANEL:
                UserDto User = message as UserDto;
                RefreshPanel(User.Name, User.Lv, User.Exp, User.Been);
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start()
    {

        txtName = transform.Find("TextName").GetComponent<Text>();
        txtLv = transform.Find("TextLV").GetComponent<Text>();
        sldExp = transform.Find("SliderExp").GetComponent<Slider>();
        txtExp = transform.Find("TextExp").GetComponent<Text>();
        txtBeen = transform.Find("TextBeen").GetComponent<Text>();
        BtnMatch = transform.Find("ButtonMatch").GetComponent<Button>();
        BtnSetting = transform.Find("ButtonSetting").GetComponent<Button>();

        BtnMatch.onClick.AddListener(BtnMatchClick);
        BtnSetting.onClick.AddListener(ButtonSettingClick);

        socketMsg = new SocketMsg();
    }


    void BtnMatchClick()
    {
        Dispatch(AreaCode.UI, UIEvent.MATCH_PANEL_ACTIVE, true);
        socketMsg.Change(OpCode.MATCH, MatchCode.ENTER_CREQ, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    private void RefreshPanel(string name, int lv, int exp, int been)
    {
        txtName.text = name;
        txtLv.text = string.Format("LV.{0}", lv);
        //等级和经验之间的公式exp=lv*100
        txtExp.text = string.Format("{0}/{1}", exp, lv * 100);
        sldExp.value = (float)exp / (lv * 100);
        txtBeen.text = string.Format("x{0}", been);
    }

    private void ButtonSettingClick()
    {
        Dispatch(AreaCode.UI, UIEvent.SETTING_PANEL_ACTIVE, true);
    }
}
