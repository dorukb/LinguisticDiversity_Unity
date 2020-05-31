using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
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
        StartCoroutine(UploadMultipleFiles());
    }

    IEnumerator UploadMultipleFiles()
    {
        //string[] path = new string[3];
        //path[0] = "D:/File1.txt";
        //UnityWebRequest[] files = new UnityWebRequest[path.Length];
        WWWForm form = new WWWForm();

        foreach (var recData in saveData.recordingData)
        {
            byte[] audioBytes = ToByteArray(recData.audioData);
            form.AddBinaryData("files[]", audioBytes, recData.title);
        }

        UnityWebRequest req = UnityWebRequest.Post("http://localhost/File%20Upload/Uploader.php", form);
        yield return req.SendWebRequest();

        if (req.isHttpError || req.isNetworkError)
            Debug.Log(req.error);
        else
            Debug.Log("Uploaded " + saveData.recordingData.Count + " audio files Successfully");
    }
    public byte[] ToByteArray(float[] floatArray)
    {
        int len = floatArray.Length * 4;
        byte[] byteArray = new byte[len];
        int pos = 0;
        foreach (float f in floatArray)
        {
            byte[] data = System.BitConverter.GetBytes(f);
            System.Array.Copy(data, 0, byteArray, pos, 4);
            pos += 4;
        }
        return byteArray;
    }

    //public float[] ToFloatArray(byte[] byteArray)
    //{
    //    int len = byteArray.Length / 4;
    //    float[] floatArray = new float[len];
    //    for (int i = 0; i < byteArray.Length; i += 4)
    //    {
    //        floatArray[i / 4] = System.BitConverter.ToSingle(byteArray, i);
    //    }
    //    return floatArray;
    //}

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
