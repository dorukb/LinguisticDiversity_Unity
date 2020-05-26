using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public SaveData saveData = new SaveData();
    #region SingletonAndDontDestroyBehaviour
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }
    #endregion
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Load() from file prev recordings?.
    }

    public void SaveFormData(FormData data)
    {
        saveData.formData = data;
        saveData.recordingData = new List<RecordingData>();
        Debug.Log("save form data.");
        Debug.Log(data);
        //Save(); write to file
    }
    public void AddRecordingData(RecordingData recordingData)
    {
        saveData.recordingData.Add(recordingData);
        //Save(); write to file
    }

}


public class SaveData
{
    public FormData formData;
    public List<RecordingData> recordingData;
}
public class FormData
{
    public string gender;
    public string nativeLanguage;
    public string contributionLanguage;
    public int proficiencyLevel;
    public int age;
}
public class RecordingData
{
    string title;
    float[] audioData;
    
    // other various audio meta data required for analysis.
}
