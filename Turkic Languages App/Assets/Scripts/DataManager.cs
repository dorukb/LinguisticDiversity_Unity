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
        if (Instance == null)
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
        saveData.recordingData = new List<RecordingData>();
    }

    public void SaveFormData(FormData data)
    {
        saveData.formData = data;
        //saveData.recordingData = new List<RecordingData>();
        Debug.Log("save form data.");
        Debug.Log(data);
        //Save(); write to file
    }
    public void AddRecordingData(string id, AudioClip clip)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);
        saveData.recordingData.Add(new RecordingData(id, samples));
        //Save(); write to file
    }

    public void Save()
    {
        //write to file.
        Debug.Log("Saved to file, also will send to web backend");
        saveData.formData.Print();
        foreach (var recData in saveData.recordingData)
        {
            Debug.LogFormat("Title: {0}, sampleSize:{1}", recData.title, recData.audioData.Length);
        }
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
    public void Print()
    {
        Debug.LogFormat("Gender: {0}, Native lang: {1}, Contribution Lang: {2}, Prof.Level: {3}, Age: {4}", gender, nativeLanguage, contributionLanguage, proficiencyLevel, age);
    }
}
public class RecordingData
{
    public string title;
    public float[] audioData;
    public RecordingData(string title, float[] audioData)
    {
        this.title = title;
        this.audioData = audioData;
    }
    // other various audio meta data required for analysis.
}
