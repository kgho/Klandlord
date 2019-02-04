using Assets.Scripts.Net;
using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyStatePanel : StatePanel
{
    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.PLAYER_HIDE_READY_BUTTON,
            UIEvent.SHOW_GRAB_BUTTON,
            UIEvent.SHOW_DEAL_BUTTON);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.PLAYER_HIDE_READY_BUTTON:
                {
                    btnReady.gameObject.SetActive(false);
                    break;
                }
            case UIEvent.SHOW_GRAB_BUTTON:
                {
                    bool active = (bool)message;
                    btnGrab.gameObject.SetActive(active);
                    btnNGrab.gameObject.SetActive(active);
                    break;
                }
            case UIEvent.SHOW_DEAL_BUTTON:
                {
                    bool active = (bool)message;
                    btnDeal.gameObject.SetActive(active);
                    btnNDeal.gameObject.SetActive(active);
                    break;
                }
            default:
                break;
        }
    }

    private Button btnDeal;
    private Button btnNDeal;
    private Button btnGrab;
    private Button btnNGrab;
    private Button btnReady;

    private SocketMsg socketMsg;

    protected override void Start()
    {
        base.Start();

        btnDeal = transform.Find("ButtonDeal").GetComponent<Button>();
        btnNDeal = transform.Find("ButtonNDeal").GetComponent<Button>();
        btnGrab = transform.Find("ButtonGrab").GetComponent<Button>();
        btnNGrab = transform.Find("ButtonNGrab").GetComponent<Button>();
        btnReady = transform.Find("ButtonReady").GetComponent<Button>();

        btnDeal.onClick.AddListener(DealClick);
        btnNDeal.onClick.AddListener(NDealClick);

        btnGrab.onClick.AddListener(() =>
        {
            GrabOnCliek(true);
        });

        btnNGrab.onClick.AddListener(() =>
        {
            GrabOnCliek(false);
        });

        btnReady.onClick.AddListener(ReadyClick);

        btnGrab.gameObject.SetActive(false);
        btnNGrab.gameObject.SetActive(false);
        btnDeal.gameObject.SetActive(false);
        btnNDeal.gameObject.SetActive(false);

        socketMsg = new SocketMsg();

        UserDto myUserDto = Models.GameModel.MatchRoomDto.UserIdUserDtoDict[Models.GameModel.UserDto.Id];
        this.userDto = myUserDto;
        SetName(userDto.Name);
    }

    protected override void ReadyState()
    {
        base.ReadyState();
        btnReady.gameObject.SetActive(false);
    }

    void DealClick()
    {
        Dispatch(AreaCode.CHARACTER, CharacterEvent.DEAL_CARD, null);
    }

    void NDealClick()
    {
        socketMsg.Change(OpCode.FIGHT, FightCode.PASS_CREQ, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    void GrabOnCliek(bool result)
    {
        socketMsg.Change(OpCode.FIGHT, FightCode.GRAB_LANDLORD_CREQ, result);
        Dispatch(AreaCode.NET, 0, socketMsg);
        btnGrab.gameObject.SetActive(false);
        btnNGrab.gameObject.SetActive(false);
    }

    void ReadyClick()
    {
        socketMsg.Change(OpCode.MATCH, MatchCode.READY_CREQ, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
}
