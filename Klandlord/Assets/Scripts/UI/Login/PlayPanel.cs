using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
//class07   
public class PlayPanel : UIBase
{
    private Button btnStart;
    private Button btnRegist;

    private void Start()
    {
        btnStart = transform.Find("BtnStart").GetComponent<Button>();
        btnRegist = transform.Find("BtnRegist").GetComponent<Button>();

        btnStart.onClick.AddListener(StartClick);
        btnRegist.onClick.AddListener(RegistClick);
    }

    public override void OnDestroy()
    {
        btnStart.onClick.RemoveAllListeners();
        btnRegist.onClick.RemoveAllListeners();
    }

    void StartClick()
    {
        Dispatch(AreaCode.UI, UIEvent.START_PANEL_ACTIVE, true);
    }

    void RegistClick()
    {
        Dispatch(AreaCode.UI, UIEvent.REGIST_PANEL_ACTIVE, true);
    }


}

