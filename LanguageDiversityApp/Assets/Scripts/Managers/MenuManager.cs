using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // for mobile UI
    [Header("Mobile UI Fields")]
    public Button newSessionButton;
    public GameObject sessionUi;

    private SessionManager sessionManager;

    void Start()
    {
        newSessionButton.onClick.AddListener(NewSessionButtonCallback);
        sessionManager = FindObjectOfType<SessionManager>();

        if (SessionManager.sessionOpen)
        {
            sessionUi.SetActive(true);
        }
        bool hasPrevSession = !string.IsNullOrEmpty(SessionManager.sessionId);
    }
    public void NewSessionButtonCallback()
    {
        sessionManager.NewSession();
        // Directly transition to Form scene
        if (FindObjectOfType<CheckboxButton>().consentGiven)
        {
            FindObjectOfType<SceneTransition>().LoadFormScene();
        }
        else
        {
            Debug.Log("please first give consent.");
        }
    }

    public void SendPreviouslySavedData()
    {
        DataManager.Instance.SendDataToServer();
    }
}
