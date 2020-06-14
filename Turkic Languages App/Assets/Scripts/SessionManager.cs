using System;
using System.IO;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    static public string sessionId = string.Empty;
    static public bool sessionOpen = false;
    private void Awake()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("lastSession")))
        { // prev session exists
            sessionId = PlayerPrefs.GetString("lastSession");
            Debug.Log("Session id is:" + sessionId);
            string dirPath = Path.Combine(Application.persistentDataPath, sessionId);
            if (!Directory.Exists(dirPath))
            {
                Debug.Log("session id was found but no save folder?.");
                PlayerPrefs.SetString("lastSession", string.Empty);
                PlayerPrefs.Save();
                sessionId = "";
            }
        }
        else
        {
            Debug.Log("no prev session found");
        }
    }

    public void RestoreSession()
    {
        // todo actually restore the state of the recordings and the form :/
        Debug.LogFormat("Restoring previous session with id: {0}", sessionId);
        DataManager.Instance.sessionId = sessionId;
        sessionOpen = true;
    }
    public void NewSession()
    {

        string oldDir = Path.Combine(Application.persistentDataPath, sessionId);
        if (Directory.Exists(oldDir))
        {
            Directory.Delete(oldDir, true);
        }

        string newId = GenerateSessionId();
        string dirPath = Path.Combine(Application.persistentDataPath, newId);
        Directory.CreateDirectory(dirPath);

        DataManager.Instance.sessionId = newId;
        PlayerPrefs.SetString("lastSession", newId);
        PlayerPrefs.Save();
        sessionOpen = true;
    }

    private string GenerateSessionId()
    {
        return Guid.NewGuid().ToString();
    }
}
