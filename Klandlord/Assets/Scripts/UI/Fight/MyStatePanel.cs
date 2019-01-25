using Assets.Scripts.Net;
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
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
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

        btnReady.onClick.AddListener(ReadyState);

        btnGrab.gameObject.SetActive(false);
        btnNGrab.gameObject.SetActive(false);
        btnDeal.gameObject.SetActive(false);
        btnNDeal.gameObject.SetActive(false);

        socketMsg = new SocketMsg();

        UserDto myUserDto = Models.GameModel.MatchRoomDto.UserIdUserDtoDict[Models.GameModel.UserDto.Id];
        this.userDto = myUserDto;
        Debug.Log(userDto.Name);
        SetName(userDto.Name);
    }

    protected override void ReadyState()
    {
        base.ReadyState();
        btnReady.gameObject.SetActive(false);
    }

    void DealClick()
    {

    }

    void NDealClick()
    {

    }

    void GrabOnCliek(bool result)
    {

    }
}
