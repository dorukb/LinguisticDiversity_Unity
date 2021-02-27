﻿using UnityEngine;

public class RecordAudio : MonoBehaviour
{
    //AudioClip recordedClip;
    AudioSource audioSource;

    public int samplerate = 44100;
    public float frequency = 440;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public AudioClip Record(string recordingName)
    {

#if UNITY_WEBGL && !UNITY_EDITOR
        MicrophoneWeb.StartRecording(DataManager.Instance.sessionId, recordingName);
        Debug.Log("SENT RECORDING REQUEST TO JS SIDE with id: " + DataManager.Instance.sessionId);
        return null;
#endif
#if !UNITY_WEBGL || UNITY_EDITOR
        AudioClip myClip = Microphone.Start("", false, 15, 44100);
        return myClip;
#endif
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
    public void PlayRecording(AudioClip recording)
    {
        audioSource.clip = recording;
        audioSource.Play();
    }
}