using Assets.Scripts.Net;
using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UserHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case UserCode.GET_INFO_SRES:
                GetInfoResponse(value as UserDto);
                break;
            case UserCode.CREATE_SRES:
                CreateResponse((int)value);
                break;
            case UserCode.ONLINE_SRES:
                OnlineResponse((int)value);
                break;
            default:
                break;
        }
    }

    private void OnlineResponse(int result)
    {
        if (result == 0)
        {
            Debug.Log("Online Successful");
        }
        else if (result == -1)
        {
            Debug.Log("Illegal Login");
            //TODO 应该强制跳转到主界面或退出游戏
        }
        else if (result == -2)
        {
            Debug.Log("No User");
        }
    }

    private void GetInfoResponse(UserDto user)
    {
        if (user == null)
        {
            //No user data,show CreatePanel
            Debug.Log("No user data,show CreatePanel");
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, true);
        }
        else
        {
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, false);
            //Save the data from server
            Models.GameModel.UserDto = user;

            //update local panel 
            Dispatch(AreaCode.UI, UIEvent.REFRESH_INFO_PANEL, user);
        }
    }

    private SocketMsg socketmsg = new SocketMsg();
    private PromptMsg promptMsg = new PromptMsg();

    private void CreateResponse(int result)
    {
        if (result == -1)
        {
            promptMsg.Change("Illegal Login", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            //TODO 应该强制跳转到登录界面或退出游戏
        }
        else if (result == -2)
        {
            promptMsg.Change("Repeat Create User", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            //TODO 重复创建应该关闭创建面板
        }
        else if (result == -3)
        {
            promptMsg.Change("Name Is Exist", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
        }
        if (result == 0)
        {
            //create successfully
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, false);
            socketmsg.Change(OpCode.USER, UserCode.GET_INFO_CREQ, null);
            Dispatch(AreaCode.NET, 0, socketmsg);
        }
    }
}

