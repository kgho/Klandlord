using Assets.Scripts.Net;
using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : UIBase
{
    private InputField inputAccount;
    private InputField inputPassword;
    private Button btnLogin;
    private Button btnClose;

    private SocketMsg socketMsg;
    private PromptMsg promptMsg;

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Login_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start()
    {
        Bind(UIEvent.Login_PANEL_ACTIVE);
        inputAccount = transform.Find("InputFieldAccount").GetComponent<InputField>();
        inputPassword = transform.Find("InputFieldPassword").GetComponent<InputField>();
        btnLogin = transform.Find("ButtonLogin").GetComponent<Button>();
        btnClose = transform.Find("ButtonClose").GetComponent<Button>();

        btnLogin.onClick.AddListener(Login);
        btnClose.onClick.AddListener(Close);

        socketMsg = new SocketMsg();
        promptMsg = new PromptMsg();
        setPanelActive(false);
    }

    public void Login()
    {
        if (string.IsNullOrEmpty(inputAccount.text))
        {
            promptMsg.Change("Username cannot be empty!", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(inputPassword.text))
        {
            promptMsg.Change("Password cannot be empty!", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (inputPassword.text.Length < 4 || inputPassword.text.Length > 16)
        {
            promptMsg.Change("Password is invalid", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }

        AccountDto dto = new AccountDto(inputAccount.text, inputPassword.text);
        socketMsg.Change(OpCode.ACCOUNT, AccountCode.LOGIN, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    public void Close()
    {
        inputAccount.text = null;
        inputPassword.text = null;
        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        btnLogin.onClick.RemoveAllListeners();
        btnClose.onClick.RemoveAllListeners();
    }
}
