using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CharacterBase : MonoBase
{
    /// <summary>
    /// save the message the Mono listen in
    /// </summary>
    private List<int> list = new List<int>();

    /// <summary>
    /// bind the message
    /// </summary>
    /// <param name="eventCodes"></param>
    protected void Bind(params int[] eventCodes)
    {
        list.AddRange(eventCodes);
        CharacterManager.Instance.Add(list.ToArray(), this);
    }

    protected void UnBind()
    {
        CharacterManager.Instance.Remove(list.ToArray(), this);
        list.Clear();
    }

    public virtual void OnDestroy()
    {
        if (list != null)
            UnBind();
    }


    public void Dispatch(int areaCode, int eventCode, object message)
    {
        MsgCenter.Instance.Dispatch(areaCode, eventCode, message);
    }
}
