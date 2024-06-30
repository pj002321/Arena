using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIWindowEvent : MonoBehaviour
{
    [SerializeField]
    [Header("StartEvent")]
    public string sceneName;

    [Header("SettingEvent")]
    public GameObject settingUI;
    private void Start()
    {
        settingUI.SetActive(false);
    }
    public void OnClickStart()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnClickQuit()
    {
        Debug.Break();
    }

    public void OnClickSetting()
    {
        settingUI.SetActive(true);
    }
    public void OnClickExitSetting()
    {
        settingUI.SetActive(false);
    }
}
