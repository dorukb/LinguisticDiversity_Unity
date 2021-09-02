using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public const string WebRecordingSceneName = "Web_Recording";
    public const string WebFormSceneName = "Web_Form";
    public const string WebMenuSceneName = "Web_Menu";

    public const string MobileRecordingSceneName = "Mobile_Recording";
    public const string MobileFormSceneName = "Mobile_Form";
    public const string MobileMenuSceneName = "Mobile_Menu";
    public void LoadRecordingScene()
    {
#if UNITY_WEBGL
        SceneManager.LoadScene(WebRecordingSceneName);
#else
        SceneManager.LoadScene(MobileRecordingSceneName);
#endif
    }

    public void LoadFormScene()
    {
#if UNITY_WEBGL
        SceneManager.LoadScene(WebFormSceneName);
#else
        SceneManager.LoadScene(MobileFormSceneName);
#endif
    }

    public void LoadMenuScene()
    {
#if UNITY_WEBGL
        SceneManager.LoadScene(WebMenuSceneName);
#else
        SceneManager.LoadScene(MobileMenuSceneName);
#endif 
    }
}
