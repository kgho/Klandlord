using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//class01   baseclass

public class UIBase : MonoBase
{
    public int sceneID;

    /// <summary>
    ///list storage the message ui interested
    /// </summary>
    private List<int> list = new List<int>();

    /// <summary>
    /// 绑定一个或多个消息
    /// </summary>
    /// <param name="eventCodes"></param>
    protected void Bind(params int[] eventCodes)
    {
        list.AddRange(eventCodes);
        UIManager.Instance.Add(list.ToArray(), this);
    }

    /// <summary>
    /// 解绑消息
    /// </summary>
    protected void UnBind()
    {
        UIManager.Instance.Remove(list.ToArray(), this);
        list.Clear();
    }

    //自动移除绑定的消息
    public virtual void OnDestroy()
    {
        if (list != null)
            UnBind();
    }

    //派遣发送消息
    public void Dispatch(int areaCode, int eventCode, object message)
    {
        MsgCenter.Instance.Dispatch(areaCode, eventCode, message);
    }

    //显示该UI面板
    protected void setPanelActive(bool active)
    {
        if (gameObject.activeSelf != active)
            gameObject.SetActive(active);
    }
}

