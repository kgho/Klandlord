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
        setPanelActive(false);
    }
    
    public void RegistClick()
    {
        AccountDto dto = new AccountDto(inputAcc.text, inputPwd.text);
        socketMsg.Change(OpCode.ACCOUNT, AccountCode.REGIST_CREQ, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void CloseClick()
    {

    }

}
