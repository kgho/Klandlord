using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CharacterManager : ManagerBase
{
    public static CharacterManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }
}

