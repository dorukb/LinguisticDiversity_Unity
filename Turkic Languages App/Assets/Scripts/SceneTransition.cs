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
        SceneManager.LoadScene(RecordingSceneName);
    }
    public void LoadFormScene()
    {
        SceneManager.LoadScene(FormSceneName);
    }
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(MenuSceneName);
    }
}
