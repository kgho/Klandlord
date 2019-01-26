using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightStatePanel : StatePanel
{
    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SET_RIGHT_PLAYER_DATA);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.SET_RIGHT_PLAYER_DATA:
                this.userDto = message as UserDto;
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        MatchRoomDto room = Models.GameModel.MatchRoomDto;
        int rightId = room.RightId;
        if (rightId != -1)
        {
            this.userDto = room.UserIdUserDtoDict[rightId];
            if (room.ReadyUserIdList.Contains(rightId))
            {
                ReadyState();
                SetName(userDto.Name);
                Debug.Log("RightPanelStart");
            }
        }
        else
        {
            setPanelActive(false);
        }
    }
    
}
