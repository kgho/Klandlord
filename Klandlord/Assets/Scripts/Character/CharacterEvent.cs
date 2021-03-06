﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEvent
{
    public const int INIT_MY_CARD = 0;
    public const int INIT_LEFT_CARD = 1;
    public const int INIT_RIGHT_CARD = 2;

    public const int ADD_MY_CARD = 3;//give table to current player
    public const int ADD_LEFT_CARD = 4;//to left player
    public const int ADD_RIGHT_CARD = 5;//to right player

    public const int DEAL_CARD = 6;

    public const int REMOVE_MY_CARD = 7;
    public const int REMOVE_LEFT_CARD = 8;
    public const int REMOVE_RIGHT_CARD = 9;

    public const int UPDATE_SHOW_DESK = 10;
}
