using UnityEngine;

public class RecordAudio : MonoBehaviour
{
    //AudioClip recordedClip;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public AudioClip Record()
    {
        AudioClip recordedClip = Microphone.Start("", false, 15, 44100);
        return recordedClip;
    }
    public void StopRecording()
    {
        Invoke("StopDelayed", 1f);
    }

    private void StopDelayed()
    {
        Microphone.End("");
    }
    public void PlayRecording(AudioClip recording)
    {
        audioSource.clip = recording;
        audioSource.Play();
    }
}
