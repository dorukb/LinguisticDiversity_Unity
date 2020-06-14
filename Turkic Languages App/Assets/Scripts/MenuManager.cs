using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button restoreSessionButton;
    public Button newSessionButton;

    public GameObject sessionUi;
    public GameObject menuUi;

    private SessionManager sessionManager;
    void Start()
    {
        restoreSessionButton.onClick.AddListener(RestoreSessionButtonCallback);
        newSessionButton.onClick.AddListener(NewSessionButtonCallback);

        sessionManager = FindObjectOfType<SessionManager>();

        if (SessionManager.sessionOpen)
        {
            sessionUi.SetActive(false);
            menuUi.SetActive(true);
        }
        else
        {
            menuUi.SetActive(false);
            sessionUi.SetActive(true);

            bool hasPrevSession = !string.IsNullOrEmpty(SessionManager.sessionId);
            restoreSessionButton.gameObject.SetActive(hasPrevSession);
        }
    }

    public void RestoreSessionButtonCallback()
    {
        sessionManager.RestoreSession();

        sessionUi.SetActive(false);
        menuUi.SetActive(true);
    }
    public void NewSessionButtonCallback()
    {
        sessionManager.NewSession();

        sessionUi.SetActive(false);
        menuUi.SetActive(true);
    }

    public void SendPreviouslySavedData()
    {
        // TODO: Add form data as well!
        DataManager.Instance.SendDataToServer();
    }
}
