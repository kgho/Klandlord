using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//class 11
public class UIManager : ManagerBase
{
    public static UIManager Instance = null;

    void Awake()
    {
        Instance = this;
    }
}

