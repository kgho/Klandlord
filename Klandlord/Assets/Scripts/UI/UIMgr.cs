using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//class06    UIRoot,所有界面的父节点，挂载该脚本管理所有的界面
public class UIMgr : MonoBehaviour
{

    [SerializeField] RectTransform layer;


    private static UIMgr mInstance;
    /// <summary>
    /// Get resources load instance
    /// </summary>
    public static UIMgr Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = GameObject.Find("UIRoot");
                if (go == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("UIPrefabs/UIRoot") as GameObject;
                    go = Instantiate(prefab);
                    Transform rootTf = go.transform;
                    rootTf.SetParent(null);
                }
                mInstance = go.GetComponent<UIMgr>();
                mInstance.OnInit();
                DontDestroyOnLoad(go);
            }
            return mInstance;
        }
    }

    private void OnInit()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int curSceneID = scene.buildIndex;
        int count = layer.childCount;
        for (int i = 0; i < count; i++)
        {
            var child = layer.GetChild(i);
            UIBase uiBase = child.GetComponent<UIBase>();
            if (uiBase.sceneID != curSceneID)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void OpenWindow(string winName, int sceneID)
    {
        string uiPath = string.Format("UIPrefabs/{0}", winName);
        GameObject prefab = Resources.Load<GameObject>(uiPath);
        if (prefab == null)
        {
            Debug.LogError("Error:UI prefab not found:" + uiPath);
            return;
        }
        GameObject uiGo = Instantiate(prefab);
        RectTransform rectTransform = uiGo.transform as RectTransform;
        rectTransform.SetParent(layer);
        rectTransform.anchoredPosition = Vector2.zero;

        //UIBase
    }
}
