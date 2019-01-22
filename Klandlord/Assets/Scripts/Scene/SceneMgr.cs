using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

public class SceneMgr : ManagerBase
{
    public static SceneMgr Instance = null;

    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        Add(SceneEvent.LOAD_SCENE, this);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case SceneEvent.LOAD_SCENE:
                LoadSceneMsg sceneIndex = message as LoadSceneMsg;
                LoadScene(sceneIndex);
                break;
            default:
                break;
        }
    }

    public Action OnSceneLoaded = null;

    //load scene
    private void LoadScene(LoadSceneMsg msg)
    {
        if (msg.SceneBuildInex != -1)
            SceneManager.LoadScene(msg.SceneBuildInex);
        if (msg.SceneBuildName != null)
            SceneManager.LoadScene(msg.SceneBuildName);
        if (msg.OnSceneLoad != null)
            OnSceneLoaded = msg.OnSceneLoad;
    }

    //call when scene load finish
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (OnSceneLoaded != null)
        {
            OnSceneLoaded();
        }
        OnSceneLoaded = null;
    }
}

