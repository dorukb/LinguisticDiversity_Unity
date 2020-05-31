using System.Collections;
using System.Collections.Generic;
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
        AudioClip recordedClip = Microphone.Start("", true, 5, 44100);
        //Debug.Log("recording...");
        return recordedClip;
    }
    public void StopRecording()
    {
        //Debug.Log("stopped.");
        Microphone.End("");
    }
    public void PlayRecording(AudioClip recording)
    {
        audioSource.clip = recording;
        audioSource.Play();
    }
}
