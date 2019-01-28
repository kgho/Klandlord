using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class MatchHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case MatchCode.ENTER_SRES:
                enterResponse(value as MatchRoomDto);
                break;
            case MatchCode.ENTER_BROADCAST:
                enterBro(value as UserDto);
                break;
            case MatchCode.READY_BROADCAST:
                readyBroadcast((int)value);
                break;
            case MatchCode.START_BROADCAST:
                startBroadcast();
                break;
            default:
                break;
        }
    }

    private void startBroadcast()
    {
        Dispatch(AreaCode.UI, UIEvent.PLAYER_HIDE_STATE, null);
    }

    private void enterResponse(MatchRoomDto matchRoom)
    {
        Models.GameModel.MatchRoomDto = matchRoom;
        resetPosition();
    }

    /// <summary>
    /// other palyer enter room
    /// </summary>
    /// <param name="newUser"></param>
    /// 
    private void enterBro(UserDto newUser)
    {
        //update room data
        MatchRoomDto room = Models.GameModel.MatchRoomDto;
        room.Add(newUser);
        resetPosition();

        if (room.LeftId != -1)
        {
            UserDto leftUserDto = room.UserIdUserDtoDict[room.LeftId];
            Dispatch(AreaCode.UI, UIEvent.SET_LEFT_PLAYER_DATA, leftUserDto);
        }
        if (room.RightId != -1)
        {
            UserDto rightUserDto = room.UserIdUserDtoDict[room.RightId];
            Dispatch(AreaCode.UI, UIEvent.SET_RIGHT_PLAYER_DATA, rightUserDto);
        }
        Dispatch(AreaCode.UI, UIEvent.PLAYER_ENTER, newUser.Id);
    }


    private void readyBroadcast(int readyUserId)
    {
        //save data
        Models.GameModel.MatchRoomDto.Ready(readyUserId);
        Dispatch(AreaCode.UI, UIEvent.PLAYER_READY, readyUserId);

        if (readyUserId == Models.GameModel.UserDto.Id)
        {
            Dispatch(AreaCode.UI, UIEvent.PLAYER_HIDE_READY_BUTTON, null);
        }
    }

    //update other player data show
    private void resetPosition()
    {
        GameModel gModel = Models.GameModel;
        MatchRoomDto matchRoom = gModel.MatchRoomDto;

        matchRoom.ResetPosition(gModel.UserDto.Id);
    }
}

