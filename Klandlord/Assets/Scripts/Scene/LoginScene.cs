﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//class05    startup
public class LoginScene : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        UIMgr.Instance.OpenWindow("UILogin/PlayPanel", SceneID.Login);
        UIMgr.Instance.OpenWindow("UILogin/RegistPanel", SceneID.Login);
        UIMgr.Instance.OpenWindow("PromptPanel", SceneID.Login);
        UIMgr.Instance.OpenWindow("UILogin/LoginPanel", SceneID.Login);
    }

}
