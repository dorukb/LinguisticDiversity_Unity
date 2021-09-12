using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] Sprite filledBarSprite;
    [SerializeField] Sprite emptyBarSprite;

    [SerializeField] Image barFirst;
    [SerializeField] Image barSecond;

    [SerializeField] ProgressUIElement infoElement;
    [SerializeField] ProgressUIElement formElement;
    [SerializeField] ProgressUIElement recordingElement;

    // based on scene, do some visual changes on the UI elements that signify progression

    private void Start()
    {
        var scene = SceneManager.GetActiveScene();
        if (   scene.name == SceneTransition.MobileFormSceneName 
            || scene.name == SceneTransition.WebFormSceneName)
        {
            SetupFormSceneUI();

        }
        else if(scene.name == SceneTransition.MobileRecordingSceneName
             || scene.name == SceneTransition.WebRecordingSceneName)
        {
            SetupRecordingSceneUI();
        }
    }

    private void SetupRecordingSceneUI()
    {
        infoElement.ToggleTick(true);
        formElement.ToggleTick(true);
        recordingElement.ToggleTick(false);

        barFirst.sprite = filledBarSprite;
        barSecond.sprite = filledBarSprite;
    }

    private void SetupFormSceneUI()
    {
        infoElement.ToggleTick(true);
        formElement.ToggleTick(false);
        recordingElement.ToggleTick(false);

        barFirst.sprite = filledBarSprite;
        barSecond.sprite = emptyBarSprite;
    }
}
