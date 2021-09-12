using UnityEngine;

public class RecordAudio : MonoBehaviour
{
    public int samplerate = 44100;
    public float frequency = 440;

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public AudioClip Record(string recordingName)
    {

#if UNITY_WEBGL && !UNITY_EDITOR
        MicrophoneWeb.StartRecording(SessionManager.sessionId, recordingName);
        Debug.Log("SENT RECORDING REQUEST TO JS SIDE with id: " + SessionManager.sessionId);
        return null;
#endif
#if !UNITY_WEBGL || UNITY_EDITOR
        AudioClip myClip = Microphone.Start("", false, 15, 44100);
        return myClip;
#endif
    }
    public void PlayRecording(AudioClip recording)
    {
        audioSource.clip = recording;
        audioSource.Play();
    }
    public void StopRecording()
    {
        Invoke("StopDelayed", 1f);
    }

    private void StopDelayed()
    {

#if UNITY_WEBGL && !UNITY_EDITOR
        MicrophoneWeb.EndRecording();
#endif
#if !UNITY_WEBGL || UNITY_EDITOR
        Microphone.End("");
#endif
    }
}
