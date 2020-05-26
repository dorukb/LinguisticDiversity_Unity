using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public List<Sprite> sprites;

    [Header("UI References")]
    public GameObject recordingVisual;
    public Image imageShown;
    public GameObject keepButton;
    public GameObject discardButton;
    public TextMeshProUGUI statusText;



    private int imageIndex;
    private AudioClip currentRecording;
    private RecordAudio recorder;
    public List<AudioClip> savedRecordings;
    private void Start()
    {
        recorder = FindObjectOfType<RecordAudio>();
        savedRecordings = new List<AudioClip>();
        currentRecording = null;
        recordingVisual.SetActive(false);

        statusText.text = "Hold to start Recording.";
        imageIndex = 0;
        imageShown.sprite = sprites[imageIndex];
    }

    public void StartRecording()
    {
        currentRecording = recorder.Record();
        recordingVisual.SetActive(false);
        statusText.text = "Recording...";
    }
    public void StopRecording()
    {
        statusText.text = "Finished.";

        recorder.StopRecording();
        keepButton.SetActive(true);
        discardButton.SetActive(true);
        recordingVisual.SetActive(true);
    }
    public void AcceptRecording()
    {
        if (currentRecording == null) return;
        currentRecording.name = sprites[imageIndex].name;
        savedRecordings.Add(currentRecording);
        discardButton.SetActive(false);

        //visually show the accepted state.
        //give option for re-recording.

    }
    public void DiscardRecording()
    {
        //currentRecording.GetData();
        currentRecording = null;
        statusText.text = "Discarded. Record Again.";
        recordingVisual.SetActive(false);
    }
    public void PlayCurrentRecording()
    {
        if (currentRecording == null) return;
        recorder.PlayRecording(currentRecording);
    }
    public void NextImage()
    {
        imageIndex++;
        imageShown.sprite = sprites[imageIndex];

        // current recording will be reset here. make sure its stored somewhere before this point.
        currentRecording = null;
    }
}
