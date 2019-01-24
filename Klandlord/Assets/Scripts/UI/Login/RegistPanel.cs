using Assets.Scripts.Net;
using Protocol.Dto;
using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//class15
public class RegistPanel : UIBase
{
    private Button btnRegist;
    private Button btnClose;
    private InputField inputAcc;
    private InputField inputPwd;
    private InputField inputPwd2;

    private SocketMsg socketMsg;
    private PromptMsg promptMsg;

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REGIST_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    void Start()
    {
        Bind(UIEvent.REGIST_PANEL_ACTIVE);
        btnRegist = transform.Find("BtnRegist").GetComponent<Button>();
        btnClose = transform.Find("BtnClose").GetComponent<Button>();

        inputAcc = transform.Find("InputAcc").GetComponent<InputField>();
        inputPwd = transform.Find("InputPwd").GetComponent<InputField>();
        inputPwd2 = transform.Find("InputPwd2").GetComponent<InputField>();

        btnRegist.onClick.AddListener(RegistClick);
        btnClose.onClick.AddListener(CloseClick);

        socketMsg = new SocketMsg();
        promptMsg = new PromptMsg();
        setPanelActive(false);
    }

    public void RegistClick()
    {
        if (string.IsNullOrEmpty(inputAcc.text))
        {
            promptMsg.Change("Account cannot be empty !", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(inputPwd.text) || string.IsNullOrEmpty(inputPwd2.text))
        {
            promptMsg.Change("Password cannot be empty !", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (inputPwd.text.Length < 4 || inputPwd.text.Length > 16)
        {
            promptMsg.Change("Password is invalid !", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if ((inputPwd2.text != inputPwd.text))
        {
            promptMsg.Change("The two passwords you typed do not match", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }

        AccountDto dto = new AccountDto(inputAcc.text, inputPwd.text);
        socketMsg.Change(OpCode.ACCOUNT, AccountCode.REGIST_CREQ, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);
        CloseClick();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void CloseClick()
    {
        inputAcc.text = null;
        inputPwd.text = null;
        inputPwd2.text = null;
        setPanelActive(false);
    }

}
