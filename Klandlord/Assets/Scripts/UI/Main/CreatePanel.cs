using Assets.Scripts.Net;
using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanel : UIBase
{
    private InputField InputName;
    private Button BtnCreate;

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;

    private void Awake()
    {
        Bind(UIEvent.CREATE_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.CREATE_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start()
    {
        InputName = transform.Find("InputFieldName").GetComponent<InputField>();
        BtnCreate = transform.Find("ButtonCreate").GetComponent<Button>();
        BtnCreate.onClick.AddListener(CreateClick);

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        BtnCreate.onClick.RemoveAllListeners();
    }

    void CreateClick()
    {
        if (string.IsNullOrEmpty(InputName.text))
        {
            promptMsg.Change("Nickname cannot be empty", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (InputName.text.Length > 8)
        {
            promptMsg.Change("Nickname length cannot longer than 8 !", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }

        socketMsg.Change(OpCode.USER, UserCode.CREATE_CREQ, InputName.text);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
}
