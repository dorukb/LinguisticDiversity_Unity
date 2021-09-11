using System;
using System.IO;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    static private string _sessionId = "";
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

    private void Awake()
    {
        if (string.IsNullOrEmpty(_sessionId))
        {
            NewSession();
        }
    }
    private void NewSession()
    {
        sessionId = GenerateSessionId();
        RemoveOldSaveDirectoryIfExists();
        Directory.CreateDirectory(sessionPath);
    }
    private void RemoveOldSaveDirectoryIfExists()
    {
        string oldDir = sessionPath;
        if (Directory.Exists(oldDir))
        {
            Directory.Delete(oldDir, true);
        }
    }
    private string GenerateSessionId()
    {
        return Guid.NewGuid().ToString();
    }
}
