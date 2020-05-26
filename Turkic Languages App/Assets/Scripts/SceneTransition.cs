using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public const string RecordingSceneName = "Recording";
    public const string FormSceneName = "Start";
    public void LoadRecordingScene()
    {
        SceneManager.LoadScene(RecordingSceneName);
    }
    public void LoadFormScene()
    {
        SceneManager.LoadScene(FormSceneName);
    }
}
