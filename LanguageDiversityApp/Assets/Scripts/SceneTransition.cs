using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public const string RecordingSceneName = "Recording";
    public const string FormSceneName = "Form";
    public const string MenuSceneName = "Menu";

    public void LoadRecordingScene()
    {
#if UNITY_WEBGL
        SceneManager.LoadScene("RecordingWeb");
#endif
#if !UNITY_WEBGL
        SceneManager.LoadScene(RecordingSceneName);
#endif
    }

    public void LoadFormScene()
    {
#if UNITY_WEBGL
        SceneManager.LoadScene("FormWeb");
#endif
#if !UNITY_WEBGL
        SceneManager.LoadScene(FormSceneName);
#endif
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(MenuSceneName);
    }
}
