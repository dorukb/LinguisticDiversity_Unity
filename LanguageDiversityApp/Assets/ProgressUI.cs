using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] string formSceneName;
    [SerializeField] string recordingSceneName;

    // based on scene, do some visual changes on the UI elements

    private void Start()
    {
        var scene  = SceneManager.GetActiveScene();
        if(scene.name == formSceneName)
        {
            // info checked
            infoElement.ToggleTick(true);
            formElement.ToggleTick(false);
            recordingElement.ToggleTick(false);

            // first bar filled
            barFirst.sprite = filledBarSprite;
            barSecond.sprite = emptyBarSprite;

        }
        else if(scene.name == recordingSceneName)
        {
            // info checked
            infoElement.ToggleTick(true);
            formElement.ToggleTick(true);
            recordingElement.ToggleTick(false);

            // first bar filled
            barFirst.sprite = filledBarSprite;
            barSecond.sprite = filledBarSprite;
        }
    }
}
