﻿using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject mobileUI;
    public GameObject webUI;
    // for mobile UI
    [Header("Mobile UI Fields")]
    public Button restoreSessionButton;
    public Button newSessionButton;
    public GameObject sessionUi;
    public GameObject menuUi;

    // webGL ui fields    
    [Header("WebGL UI Fields")]
    public Button restoreSessionButtonWeb;
    public Button newSessionButtonWeb;
    public GameObject sessionUiWeb;
    public GameObject menuUiWeb;

    private SessionManager sessionManager;
    private bool isWeb = false;
    public bool forceWebUI = false;
    private void Awake()
    {
#if UNITY_WEBGL
        isWeb = true;
#endif
        if (Application.platform == RuntimePlatform.WebGLPlayer || forceWebUI)
        {
            Debug.Log("running on WEBGL");
            isWeb = true;
        }
    }
    void Start()
    {
        mobileUI.gameObject.SetActive(!isWeb);
        webUI.gameObject.SetActive(isWeb);

        if (isWeb)
        {
            //swap the UIs
            Debug.Log("running on webGL");
            restoreSessionButton = restoreSessionButtonWeb;
            newSessionButton = newSessionButtonWeb;
            sessionUi = sessionUiWeb;
            menuUi = menuUiWeb;
        }
        restoreSessionButton.onClick.AddListener(RestoreSessionButtonCallback);
        newSessionButton.onClick.AddListener(NewSessionButtonCallback);
        sessionManager = FindObjectOfType<SessionManager>();


        if (isWeb || SessionManager.sessionOpen)
        {
            menuUi.SetActive(false);
            sessionUi.SetActive(true);
        }
        bool hasPrevSession = !string.IsNullOrEmpty(SessionManager.sessionId);
        // restore session functionality is disabled/not implemented on webGL version.
        restoreSessionButton.gameObject.SetActive(hasPrevSession && !isWeb);
        
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

        //sessionUi.SetActive(false);
        //menuUi.SetActive(true);

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
        // TODO: Add form data as well!
        DataManager.Instance.SendDataToServer();
    }
}