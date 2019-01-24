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
            default:
                break;
        }
    }

    private void enterResponse(MatchRoomDto matchRoom)
    {
        Models.GameModel.MatchRoomDto = matchRoom;
    }
}

