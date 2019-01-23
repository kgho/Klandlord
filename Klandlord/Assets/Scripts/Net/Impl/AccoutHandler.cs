using Assets.Scripts.Net;
using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AccoutHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccountCode.REGIST_SRES:
                registResponse((int)value);
                break;
            case AccountCode.LOGIN:
                loginResponse((int)value);
                break;
            default:
                break;
        }
    }


    public PromptMsg promptMsg = new PromptMsg();


    private void loginResponse(int result)
    {
        switch (result)
        {
            case 0:
                //load Main scene
                LoadSceneMsg msg = new LoadSceneMsg(1, () =>
                {
                    //向服务器获取该账号下是否有角色
                    SocketMsg socketMsg = new SocketMsg(OpCode.USER, UserCode.GET_INFO_CREQ, null);
                    Dispatch(AreaCode.NET, 0, socketMsg);
                });
                Dispatch(AreaCode.SCENE, SceneEvent.LOAD_SCENE, msg);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// register response
    /// </summary>
    /// <param name="result"></param>
    private void registResponse(int result)
    {
        switch (result)
        {
            case 0:
                promptMsg.Change("Sign Up Successful!", Color.green);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -1:
                promptMsg.Change("Error:Account already exist!", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -2:
                promptMsg.Change("Error:Account is invalid!", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -3:
                promptMsg.Change("Error:Passord is invalid", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            default:
                break;
        }
    }

  
}
