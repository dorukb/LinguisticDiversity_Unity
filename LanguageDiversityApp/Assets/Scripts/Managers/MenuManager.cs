using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button startButton;
    private CheckboxButton checkbox;

    void Start()
    {
        startButton.onClick.AddListener(StartButtonCallback);
        checkbox = FindObjectOfType<CheckboxButton>();
        if (checkbox == null) 
            Debug.LogError("Checkbox button script reference is null");
    }
    public void StartButtonCallback()
    {
        if (checkbox.consentGiven)
        {
             FindObjectOfType<SceneTransition>().LoadFormScene();
        }
        else
        {
            Debug.Log("please first give consent.");
        }
    }
}
