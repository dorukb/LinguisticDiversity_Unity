using UnityEngine.SceneManagement;

public static class SceneTransition
{
    public static string WebRecordingSceneName = "Web_Recording";
    public static string WebFormSceneName = "Web_Form";
    public static string WebMenuSceneName = "Web_Menu";

    public static string MobileRecordingSceneName = "Mobile_Recording";
    public static string MobileFormSceneName = "Mobile_Form";
    public static string MobileMenuSceneName = "Mobile_Menu";
    public static void LoadRecordingScene()
    {
#if UNITY_WEBGL
        SceneManager.LoadScene(WebRecordingSceneName);
#else
        SceneManager.LoadScene(MobileRecordingSceneName);
#endif
    }

    public static void LoadFormScene()
    {
#if UNITY_WEBGL
        SceneManager.LoadScene(WebFormSceneName);
#else
        SceneManager.LoadScene(MobileFormSceneName);
#endif
    }

    public static void LoadMenuScene()
    {
#if UNITY_WEBGL
        SceneManager.LoadScene(WebMenuSceneName);
#else
        SceneManager.LoadScene(MobileMenuSceneName);
#endif 
    }
}
