using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//class01   baseclass

public class UIBase : MonoBase
{
    public int sceneID;

    /// <summary>
    /// the message ui interested
    /// </summary>
    private List<int> list = new List<int>();

    protected void UnBind()
    {
        
    }

    public virtual void OnDestroy()
    {
        if (list != null)
        {

        }
    }
}

