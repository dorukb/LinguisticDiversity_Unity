using System;
using System.IO;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    static private string _sessionId;
    static public string sessionId
    {
        get
        {
            return _sessionId;
        }
        private set { _sessionId = value; }
    }
    static public string sessionPath 
    { 
        get {
            if (!String.IsNullOrEmpty(sessionId))
            {
                return Path.Combine(Application.persistentDataPath, sessionId);
            }
            else return Application.persistentDataPath;
        } 
        private set {}
    }

    static public bool sessionOpen = false;
    private void Awake()
    {
        sessionId = "";
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("lastSession")))
        { // prev session exists
            sessionId = PlayerPrefs.GetString("lastSession");
            Debug.Log("Previous session id is:" + sessionId);
            string dirPath = Path.Combine(Application.persistentDataPath, sessionId);
            if (!Directory.Exists(dirPath))
            {
                Debug.Log("session id was found but no save folder?.");
                PlayerPrefs.SetString("lastSession", string.Empty);
                PlayerPrefs.Save();
            }
        }
        else
        {
            Debug.Log("no prev session found");
        }
    }
    public void NewSession()
    {
        sessionId = GenerateSessionId();
        string oldDir = sessionPath;
        if (Directory.Exists(oldDir))
        {
            Directory.Delete(oldDir, true);
        }

        string dirPath = sessionPath;
        Directory.CreateDirectory(dirPath);

        PlayerPrefs.SetString("lastSession", sessionId);
        PlayerPrefs.Save();
        sessionOpen = true;
    }

    private string GenerateSessionId()
    {
        return Guid.NewGuid().ToString();
    }
}
