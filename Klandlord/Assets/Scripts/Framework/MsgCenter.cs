using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// class12 
/// 消息处理中心
///     只负责消息的转发
///     
///     ui -> msgCenter-> character
/// </summary>
public class MsgCenter : MonoBase
{
    public static MsgCenter Instance = null;

    private void Awake()
    {
        Instance = this;

        gameObject.AddComponent<NetManager>();
        gameObject.AddComponent<UIManager>();
        gameObject.AddComponent<SceneMgr>();
        gameObject.AddComponent<CharacterManager>();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 该方法用于转发所有系统里面发送的消息
    ///   比如说 第一个参数 识别到是角色模块 但是角色模块有很多功能 比如 移动 攻击 死亡 逃跑...
    ///         就需要第二个参数 来识别 具体是做哪一个动作
    /// </summary>
    /// <param name="areaCode">通过areaCode识别不同的模块</param>
    /// <param name="eventCode">区分做什么事情</param>
    /// <param name="message"></param>
    public void Dispatch(int areaCode, int eventCode, object message)
    {
        switch (areaCode)
        {
            case AreaCode.UI:
                UIManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.NET:
                NetManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.SCENE:
                SceneMgr.Instance.Execute(eventCode, message);
                break;
            case AreaCode.CHARACTER:
                CharacterManager.Instance.Execute(eventCode, message);
                break;
            default:
                break;
        }
    }
}