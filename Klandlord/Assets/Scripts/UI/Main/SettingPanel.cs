using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : UIBase
{

    private Button BtnClose;
    private Toggle TogSound;
    private Slider SldVolume;
    private Button BtnQuit;

    private void Awake()
    {
        Bind(UIEvent.SETTING_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SETTING_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start()
    {
        BtnClose = transform.Find("ButtonClose").GetComponent<Button>();
        TogSound = transform.Find("ToggleSound").GetComponent<Toggle>();
        SldVolume = transform.Find("SliderVolume").GetComponent<Slider>();
        BtnQuit = transform.Find("ButtonQuit").GetComponent<Button>();

        BtnClose.onClick.AddListener(BtnCloseClick);
        BtnQuit.onClick.AddListener(BtnQuitClick);
        TogSound.onValueChanged.AddListener(TogAudio_onValueChanged);
        SldVolume.onValueChanged.AddListener(SldVolume_onValueChanged);

        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        BtnClose.onClick.RemoveAllListeners();
        BtnQuit.onClick.RemoveAllListeners();
        TogSound.onValueChanged.RemoveAllListeners();
        SldVolume.onValueChanged.RemoveAllListeners();
    }

    void BtnCloseClick()
    {
        setPanelActive(false);
    }

    void BtnQuitClick()
    {
        Application.Quit();
    }

    void TogAudio_onValueChanged(bool value)
    {

    }

    void SldVolume_onValueChanged(float value)
    {

    }

}
