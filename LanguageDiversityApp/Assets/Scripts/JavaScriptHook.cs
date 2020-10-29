using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavaScriptHook : MonoBehaviour
{
    public void PrintMessage(string msg)
    {
        Debug.Log("Log from js side:" +  msg);
    }

    public void DataReady()
    {
        var appManager = FindObjectOfType<AppManager>();
        appManager.RecordingDataReady();
    }
}
