using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    //public string mobileTestAddress = "http://localhost/turkicLanguages/upload.php";

    public string mobilePostAddress = "http://coltekin.net/audio/mobileUpload.php";

    public string sessionId;
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
    public void RestoreRecordingsFromPreviousSession(string sessionId)
    {
        //string sessionDirectory = Path.Combine(Application.persistentDataPath, sessionId);
        //string[] recordingPaths = Directory.GetFiles(sessionDirectory);
    }
    public void SaveFormData(FormData data, Action endCallback)
    {
#if UNITY_WEBGL
        //directly send form data since the audio data will be send from the web side anyways.
        StartCoroutine(UploadFormData(endCallback, data));
#endif
#if !UNITY_WEBGL
        saveData.formData = data;
        Debug.Log("saving form data.");

        string sessionFolder = Path.Combine(Application.persistentDataPath, sessionId);
        string filePath= Path.Combine(sessionFolder, "form.json");
        File.WriteAllText(filePath, JsonUtility.ToJson(data));
        endCallback?.Invoke();
#endif
    }
    public void AddRecordingData(string id, AudioClip clip)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        string sessionFolder = Path.Combine(Application.persistentDataPath, sessionId);
        string filePath = Path.Combine(sessionFolder, id);
        Debug.Log("saving audio to: " + filePath);
        SavWav.Save(filePath, clip);
    }

    public void SendDataToServer(Action endCallback = null)
    {
        Debug.Log("will send previously saved data to web backend");
        StartCoroutine(UploadAllFiles(endCallback));
    }
    IEnumerator UploadFormData(Action endCallback, FormData data)
    {
        WWWForm form = new WWWForm();
        string sessionDirectory = Path.Combine(Application.persistentDataPath, sessionId);
        form.AddField("id", sessionId);
        form.AddField("form", JsonUtility.ToJson(data));

        UnityWebRequest req = UnityWebRequest.Post(mobilePostAddress, form);
        yield return req.SendWebRequest();

        if (req.isHttpError || req.isNetworkError)
            Debug.Log(req.error);
        else
            Debug.Log("Uploaded successfully");
        endCallback?.Invoke();
    }
    IEnumerator UploadAllFiles(Action endCallback)
    {
        WWWForm form = new WWWForm();
        string sessionDirectory = Path.Combine(Application.persistentDataPath, sessionId);

        // Does not use any save file, directly gets all audio files associated with this session.
        string[] recordingPaths = Directory.GetFiles(sessionDirectory);

        form.AddField("id", sessionId);
        foreach(string filePath in recordingPaths)
        {
            File.OpenRead(filePath);
            byte[] bytes = File.ReadAllBytes(filePath);

            string fileName = Path.GetFileName(filePath);
            form.AddBinaryData("files[]", bytes, fileName);
        }

        UnityWebRequest req = UnityWebRequest.Post("http://localhost/turkicLanguages/uploadMobile.php", form);        
        yield return req.SendWebRequest();

        if (req.isHttpError || req.isNetworkError)
            Debug.Log(req.error);
        else
            Debug.Log("Uploaded " + recordingPaths.Length + " audio files Successfully");

        if (req.isDone)
        {
            Debug.Log("req is done");

        }
        endCallback?.Invoke();
    }
}


public class SaveData
{
    public FormData formData;
    public List<RecordingData> recordingData;
}
public class FormData
{
    public string gender = "";
    public string nativeLanguage = "";
    public string contributionLanguage = "";
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
