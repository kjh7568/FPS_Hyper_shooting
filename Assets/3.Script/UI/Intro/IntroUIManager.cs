using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroUIManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject settingPanel;

    public void OnClickStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickSettings()
    {
        buttonPanel.SetActive(false);
        settingPanel.SetActive(true);
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        // 에디터일 때 플레이 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 게임 종료
        Application.Quit();
#endif
    }
}