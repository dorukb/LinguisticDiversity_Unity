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

    public string mobilePostAddress = "https://coltekin.net/audio/uploadMobile.php";
    public string webPostAdress = "https://coltekin.net/audio/upload.php";

    private Coroutine uploadCR;

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
    #endregion
    public void SaveFormData(FormData data, Action endCallback)
    {
#if UNITY_WEBGL
        // Directly send form data since the audio data will be send from the web side.
        if(uploadCR == null)
        {
            uploadCR = StartCoroutine(UploadFormDataWEBGL(endCallback, data));
        }
#endif
#if !UNITY_WEBGL
        // Write to session folder, will be sent later together with all the audio files.
        string filePath = Path.Combine(SessionManager.sessionPath, "form.json");
        File.WriteAllText(filePath, JsonUtility.ToJson(data));
        endCallback?.Invoke();
#endif
    }
    public void AddRecordingData(string id, AudioClip clip)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        string filePath = Path.Combine(SessionManager.sessionPath, id);
        Debug.Log("saving audio to: " + filePath);
        SavWav.Save(filePath, clip);
    }

    public void SendDataToServer(Action endCallback = null)
    {
        Debug.Log("will send previously saved data to web backend");
        StartCoroutine(UploadAllFiles(endCallback));
    }
    IEnumerator UploadFormDataWEBGL(Action endCallback, FormData data)
    {
        WWWForm form = new WWWForm();
        string sessionDirectory = SessionManager.sessionPath;
        form.AddField("id", SessionManager.sessionId);
        form.AddField("form", JsonUtility.ToJson(data));

        UnityWebRequest req = UnityWebRequest.Post(webPostAdress, form);
        yield return req.SendWebRequest();

        if (req.isHttpError || req.isNetworkError)
            Debug.Log(req.error);
        else
            Debug.Log("Uploaded successfully");

        uploadCR = null;
        endCallback?.Invoke();
    }
    IEnumerator UploadAllFiles(Action endCallback)
    {
        WWWForm form = AddAllRecordingsToForm();
        UnityWebRequest req = UnityWebRequest.Post(mobilePostAddress, form);
        Debug.Log("send the web request now. ts: " + Time.realtimeSinceStartup);
        yield return req.SendWebRequest();

        if (req.isHttpError || req.isNetworkError)
            Debug.Log(req.error);
        else
            Debug.Log("Uploaded audio files Successfully. TS: " + Time.realtimeSinceStartup);

        if (req.isDone)
        {
            Debug.Log("req is done. TS: " + Time.realtimeSinceStartup);

        }
        endCallback?.Invoke();
    }

    private static WWWForm AddAllRecordingsToForm()
    {
        // Get all files saved to disk this session.
        // Includes all non-discarded recordings and the form.json file.
        WWWForm form = new WWWForm();
        string[] recordingPaths = Directory.GetFiles(SessionManager.sessionPath);
        form.AddField("id", SessionManager.sessionId);

        foreach (string filePath in recordingPaths)
        {
            File.OpenRead(filePath);
            byte[] bytes = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            form.AddBinaryData("files[]", bytes, fileName);

            Debug.LogFormat("added file: {0} to form.", fileName);
        }
        return form;
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
        Debug.LogFormat("Gender: {0}, Native lang: {1}, Contribution Lang: {2}, Prof.Level: {3}, Age: {4}", 
            gender, nativeLanguage, contributionLanguage, proficiencyLevel, age);
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
    // add in other audio meta data required for analysis.
}
