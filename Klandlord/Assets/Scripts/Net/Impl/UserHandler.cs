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
        throw new NotImplementedException();
    }


    private void GetInfoResponse(UserDto user)
    {
        if (user == null)
            //No user data,show CreatePanel
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, true);

    }
}

