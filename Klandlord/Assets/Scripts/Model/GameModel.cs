using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// sava class to save game data
/// </summary>
public class GameModel
{
    //login user data
    public UserDto UserDto { get; set; }

    public int Id { get { return UserDto.Id; } }

    public MatchRoomDto MatchRoomDto { get; set; }

    public UserDto GetUserDto(int userId)
    {
        return MatchRoomDto.UserIdUserDtoDict[userId];
    }

}

