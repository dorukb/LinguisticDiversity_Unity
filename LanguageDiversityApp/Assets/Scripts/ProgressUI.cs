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
        if (scene.name == SceneTransition.MobileFormSceneName 
            || scene.name == SceneTransition.WebFormSceneName)
        {
            // Form scene setup
            // info checked
            infoElement.ToggleTick(true);
            formElement.ToggleTick(false);
            recordingElement.ToggleTick(false);

            // first bar filled, second empty
            barFirst.sprite = filledBarSprite;
            barSecond.sprite = emptyBarSprite;

        }
        else if(scene.name == SceneTransition.MobileRecordingSceneName
            || scene.name == SceneTransition.WebRecordingSceneName)
        {
            // Recording scene setup
            // info & form checked
            infoElement.ToggleTick(true);
            formElement.ToggleTick(true);
            recordingElement.ToggleTick(false);

            // first & second bar filled
            barFirst.sprite = filledBarSprite;
            barSecond.sprite = filledBarSprite;
        }
    }
}
