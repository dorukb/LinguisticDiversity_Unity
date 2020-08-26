using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public List<Word> wordImagePairs;

    [Header("UI References")]
    public GameObject recordingVisual;
    public Image imageShown;
    public GameObject discardButton;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI recordingNameText;
    public TextMeshProUGUI recordingCounterText;

    public GameObject recordingPanel;
    public GameObject endPanel;

    private RecordAudio recorder;
    private int wordIndex;
    private string currentRecordingId = "";
    private AudioClip currentRecording;
    private bool isRecording = false;

    public Dictionary<string, AudioClip> savedRecordings;
    private void Start()
    {
        recorder = FindObjectOfType<RecordAudio>();
        savedRecordings = new Dictionary<string, AudioClip>();
        recordingVisual.SetActive(false);

        statusText.text = "Hold to start Recording.";
        wordIndex = 0;
        imageShown.sprite = wordImagePairs[wordIndex].sprite;
        recordingCounterText.text = (wordIndex + 1).ToString() + " / " + wordImagePairs.Count;
    }

    public void StartRecording()
    {
        if (isRecording) return;

        if (savedRecordings.ContainsKey(currentRecordingId))
        {
            savedRecordings.Remove(currentRecordingId);
        }
        currentRecording = recorder.Record(wordImagePairs[wordIndex].keyword);
        // will be null if WebGL, since it wont return but keep the data on the browser side.

        isRecording = true;
        currentRecordingId = wordImagePairs[wordIndex].keyword;

        recordingVisual.SetActive(false);
        statusText.text = "Recording...";
    }
    public void StopRecording()
    {
        if (!isRecording) return;
        isRecording = false;
        statusText.text = "Finished.";

        recorder.StopRecording();

        discardButton.SetActive(true);
        recordingVisual.SetActive(true);

        recordingNameText.text = currentRecordingId;
        if(currentRecording != null) currentRecording.name = currentRecordingId;

        savedRecordings.Add(currentRecordingId, currentRecording);
    }

    public void DiscardRecording()
    {
        savedRecordings.Remove(currentRecordingId);
        currentRecording = null;
        currentRecordingId = "";

        statusText.text = "Discarded. Hold to Record Again.";
        recordingVisual.SetActive(false);
    }
    public void PlayCurrentRecording()
    {
        if (currentRecording == null) return;
        recorder.PlayRecording(currentRecording);
    }
    public void NextImage()
    {
        wordIndex++;
        recordingCounterText.text = (wordIndex + 1).ToString() + " / " + wordImagePairs.Count;

        if (wordIndex >= wordImagePairs.Count)
        {
            FinishRecordingPhase();
            //DataManager.Instance.SendDataToServer();
        }
        else
        {
            imageShown.sprite = wordImagePairs[wordIndex].sprite;

            // current recording will be reset here. make sure its stored somewhere before this point.
            //reset recording visuals and state, except savedRecording, we keep them ofc.
            currentRecording = null;
            currentRecordingId = "";

            recordingVisual.SetActive(false);
            statusText.text = "Hold to start Recording.";
        }
       
    }

    public void FinishRecordingPhase()
    {
        recordingPanel.SetActive(false);
        endPanel.SetActive(true);
        //we are done, send all recorded audio to data manager for saving.
        foreach (var recordingPair in savedRecordings)
        {
            if(recordingPair.Value == null)
            {
                Debug.Log("This was saved on webGL side. skip it.");
                continue;
            }
            string recordingId = recordingPair.Key;
            AudioClip recording = recordingPair.Value;
            DataManager.Instance.AddRecordingData(recordingId, recording);
        }
    }

    // Called by the submit button
    public void SubmitRecordings()
    {
        DataManager.Instance.SendDataToServer(() =>
        {
            // TODO: show success message/screen then redirect to menu scene.
            FindObjectOfType<SceneTransition>().LoadMenuScene();
        }
        // TODO: Also handle failed case and give option to retry.
        );
    }


}
