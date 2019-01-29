using Protocol.Code;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FightHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case FightCode.GET_CARD_SRES:
                Debug.Log("Deal");
                getCards(value as List<CardDto>);
                break;
            case FightCode.TURN_GRAB_BROADCAST:
                Debug.Log("Change Landlord");
                turnGrabBroadcast((int)value);
                break;
            case FightCode.GRAB_LANDLORD_BROADCAST:
                Debug.Log("Landlord has benn grabed");
                grabLandlordBroadcast(value as GrabDto);
                break;
            default:
                break;
        }
    }

    private void getCards(List<CardDto> cardList)
    {
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_MY_CARD, cardList);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_RIGHT_CARD, null);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_LEFT_CARD, null);
    }

    //wether the first player is brabing landlord
    private bool isFirst = true;

    //if is this client to grab
    private void turnGrabBroadcast(int userId)
    {
        //if is this client 
        if (userId == Models.GameModel.UserDto.Id)
        {//to grab
            Dispatch(AreaCode.UI, UIEvent.SHOW_GRAB_BUTTON, true);
        }
    }

    private void grabLandlordBroadcast(GrabDto dto)
    {
        //update ui,set avatar to landlord
        Dispatch(AreaCode.UI, UIEvent.PLAY_CHANGE_IDENTITY, dto.userId);
        //show three table cards
        Dispatch(AreaCode.UI, UIEvent.SET_TABLE_CARD, dto.TableCardList);

        int eventCode = -1;
        if (dto.userId == Models.GameModel.MatchRoomDto.LeftId)
        {
            eventCode = CharacterEvent.ADD_LEFT_CARD;
        }
        else if (dto.userId == Models.GameModel.MatchRoomDto.RightId)
        {
            eventCode = CharacterEvent.ADD_RIGHT_CARD;
        }
        else if (dto.userId == Models.GameModel.UserDto.Id)
        {
            eventCode = CharacterEvent.ADD_MY_CARD;
        }
        Dispatch(AreaCode.CHARACTER, eventCode, dto);
    }

}
