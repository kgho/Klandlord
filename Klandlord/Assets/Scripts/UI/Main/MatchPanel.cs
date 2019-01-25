using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.MATCH_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.MATCH_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private Text textDes;
    private Button btnCancel;
    private Button btnEnter;

    // Use this for initialization
    void Start()
    {
        textDes = transform.Find("TextDes").GetComponent<Text>();
        btnCancel = transform.Find("ButtonCancel").GetComponent<Button>();
        btnEnter = transform.Find("ButtonEnter").GetComponent<Button>();

        btnCancel.onClick.AddListener(CancelClick);
        btnEnter.onClick.AddListener(EnterCliek);

        setPanelActive(false);
    }

    private void CancelClick()
    {
        
    }

    private void EnterCliek()
    {
        Dispatch(AreaCode.SCENE, SceneEvent.LOAD_SCENE, new LoadSceneMsg(2, null));
    }
}
