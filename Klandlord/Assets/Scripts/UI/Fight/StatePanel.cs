using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StatePanel : UIBase
{
    protected virtual void Awake()
    {
        Bind(UIEvent.PLAYER_HIDE_STATE);
        Bind(UIEvent.PLAYER_READY);
        Bind(UIEvent.PLAYER_LEAVE);
        Bind(UIEvent.PLAYER_ENTER);
        Bind(UIEvent.PLAY_CHANGE_IDENTITY);
        Bind(UIEvent.SHOW_TIMER_PANEL);
        Bind(UIEvent.HIDE_TIMER_PANEL);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.PLAYER_HIDE_STATE:
                {
                    textReady.gameObject.SetActive(false);
                }
                break;
            case UIEvent.PLAYER_ENTER:
                {
                    if (userDto == null)
                        break;
                    int userId = (int)message;
                    if (userDto.Id == userId)
                    {
                        setPanelActive(true);
                        SetName(userDto.Name);
                    }
                    break;
                }
            case UIEvent.PLAYER_READY:
                {
                    if (userDto == null)
                        break;
                    int userId = (int)message;
                    if (userDto.Id == userId)
                        ReadyState();
                    break;
                }
            default:
                break;
        }
    }

    //user data
    protected UserDto userDto;

    protected Image imgIdentity;
    protected Text textReady;
    protected Text textTime;
    protected Text textName;

    protected virtual void Start()
    {
        imgIdentity = transform.Find("ImageIdentity").GetComponent<Image>();
        textReady = transform.Find("TextReady").GetComponent<Text>();
        textTime = transform.Find("TextTime").GetComponent<Text>();
        textName = transform.Find("TextName").GetComponent<Text>();

        textReady.gameObject.SetActive(false);
        textTime.gameObject.SetActive(false);
    }


    protected virtual void ReadyState()
    {
        textReady.gameObject.SetActive(true);
    }

    protected virtual void SetName(string name)
    {
        textName.text = name;
    }

    /// <summary>
    /// 0:farmer    1:landlord
    /// </summary>
    /// <param name="identity"></param>
    protected void setIdentity(int identity)
    {
        if (identity == 0)
        {
            imgIdentity.sprite = Resources.Load<Sprite>("Identity/Farmer");
        }
        else if (identity == 1)
        {
            imgIdentity.sprite = Resources.Load<Sprite>("Identity/Landlord");
        }
    }

    protected float timer1 = 0f;

    protected bool isTimerShow = false;

    /// <summary>
    /// the time the timer showed
    /// </summary>
    protected float TimerShowTime;

    /// <summary>
    /// save the time last update panel
    /// </summary>
    private int textTImer = -1;


}

