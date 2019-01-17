using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//class08
/// <summary>
///  每个模块的基类
///  保存自身注册的一系列消息
/// </summary>
public class ManagerBase : MonoBase
{
    /// <summary>
    /// 处理自身的消息
    /// </summary>
    /// <param name="eventCode"></param>
    /// <param name="message"></param>
    public override void Execute(int eventCode, object message)
    {
        if (!dict.ContainsKey(eventCode))
        {
            Debug.LogWarning("doesn't regist:" + eventCode);
            return;
        }

        //if has regist this message,send this message to all scripts
        List<MonoBase> list = dict[eventCode];
        for (int i = 0; i < list.Count; i++)
        {
            list[i].Execute(eventCode, message);
        }
    }

    /// <summary>
    /// 用于存储 消息的事件码 和 哪个脚本 关联
    /// 
    /// 角色模块 有一个动作是 移动
    ///             移动模块需要关心这个事件 控制角色位置 进行移动
    ///             动画模块 也需要关心  控制角色播放动画    
    ///             音效模块 也需要关心  控制角色移动的音效播放 走路声
    /// </summary>
    private Dictionary<int, List<MonoBase>> dict = new Dictionary<int, List<MonoBase>>();

    /// <summary>
    /// add event 
    /// </summary>
    /// <param name="eventCode"></param>
    /// <param name="mono"></param>
    public void Add(int eventCode,MonoBase mono)
    {
        List<MonoBase> list = null;

        //之前未注册
        if (!dict.ContainsKey(eventCode))
        {
            list = new List<MonoBase>();
            list.Add(mono);
            dict.Add(eventCode, list);
            return;
        }

        list = dict[eventCode];
        list.Add(mono);
    }

    /// <summary>
    /// 添加多个事件
    ///         一个脚本关心多个事件
    /// </summary>
    /// <param name="eventCodes"></param>
    /// <param name="mono"></param>
    public void Add(int[] eventCodes,MonoBase mono)
    {
        for(int i = 0; i < eventCodes.Length; i++)
        {
            Add(eventCodes[i], mono);
        }
    }

    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="eventCode"></param>
    /// <param name="mono"></param>
    public void Remove(int eventCode, MonoBase mono)
    {
        if (!dict.ContainsKey(eventCode))
        {
            Debug.LogWarning("doesn't exist this event" + eventCode);
            return;
        }

        List<MonoBase> list = dict[eventCode];
        if (list.Count == 1)
            dict.Remove(eventCode);
        else
            list.Remove(mono);
    }

    /// <summary>
    /// 移除多个
    /// </summary>
    /// <param name="eventCodes"></param>
    /// <param name="mono"></param>
    public void Remove(int[] eventCodes, MonoBase mono)
    {
        for(int i = 0; i < eventCodes.Length; i++)
        {
            Remove(eventCodes[i], mono);
        }
    }

}

