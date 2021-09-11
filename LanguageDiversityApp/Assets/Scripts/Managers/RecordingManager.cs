using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordingManager : MonoBehaviour
{
    public List<Word> wordImagePairs;

    [Header("UI References")]
    public GameObject recordingVisual;
    public Image imageShown;
    public GameObject discardButton;
    public GameObject advanceButton;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI recordingNameText;
    public TextMeshProUGUI recordingCounterText;

    public GameObject recordingPanel;
    public GameObject endPanel;
    public GameObject endPanelContinueButton;

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

        statusText.text = "Hold to Record";
        wordIndex = 0;

        //shuffle the words for more uniform data collection
        wordImagePairs.Shuffle();

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

        advanceButton.SetActive(false);
        recordingVisual.SetActive(false);
        statusText.text = "Recording...";
    }
    public void StopRecording()
    {
        if (!isRecording) return;

        isRecording = false;
        statusText.text = "Hold to Record";

        recorder.StopRecording();
        discardButton.SetActive(true);
#if !UNITY_WEBGL || UNITY_EDITOR
        recordingVisual.SetActive(true);
        advanceButton.SetActive(true);
#endif
        recordingNameText.text = currentRecordingId;
        if(currentRecording != null) currentRecording.name = currentRecordingId;

        savedRecordings.Add(currentRecordingId, currentRecording);
    }

    public void DiscardRecording()
    {
        bool succesful = savedRecordings.Remove(currentRecordingId);
        if (!succesful)
        {
            Debug.LogError("Unable to discard recording with id: " + currentRecordingId);
        }
               
        currentRecording = null;
        currentRecordingId = "";

        statusText.text = "Hold to Record Again.";
        recordingVisual.SetActive(false);
    }
    public void PlayCurrentRecording()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        MicrophoneWeb.PlayCurrentRecording();
#endif
        if (currentRecording == null) return;
        recorder.PlayRecording(currentRecording);
    }
    public void RecordingDataReady() 
    {
        // Case for WebGL versions. 
        // Since the audio processing on the browser side is async and out of unity's control, we require this callback.
        // now we can advance to the next item safely, without losing data.
        advanceButton.SetActive(true);
        recordingVisual.SetActive(true);
    }
    public int GetRemainingImageCount()
    {
        return wordImagePairs.Count - (wordIndex + 1);
    }
    public void NextImage()
    {
        wordIndex++;
        recordingCounterText.text = (wordIndex + 1).ToString() + " / " + wordImagePairs.Count;

        if (wordIndex >= wordImagePairs.Count)
        {
            FinishRecordingPhase();
        }
        else
        {
            imageShown.sprite = wordImagePairs[wordIndex].sprite;
#if UNITY_WEBGL && !UNITY_EDITOR
            MicrophoneWeb.SaveCurrentRecording();
#endif
            // current recording will be reset here. make sure its stored somewhere before this point.
            //reset recording visuals and state, except savedRecording, we keep them ofc.
            currentRecording = null;
            currentRecordingId = "";

            recordingVisual.SetActive(false);
            statusText.text = "Hold to Record";
        }
       
    }

    public void FinishRecordingPhase()
    {
        recordingPanel.SetActive(false);
        endPanel.SetActive(true);
        bool canContinueLater = wordIndex < wordImagePairs.Count;
        endPanelContinueButton.SetActive(canContinueLater);

            //we are done, send all recorded audio to data manager for saving.
#if !UNITY_WEBGL || UNITY_EDITOR
        foreach (var recordingPair in savedRecordings)
        {
            if(recordingPair.Value != null)
            {
                string recordingId = recordingPair.Key;
                AudioClip recording = recordingPair.Value;
                DataManager.Instance.AddRecordingData(recordingId, recording);
            }
        }
        SubmitRecordings();
#endif
    }
    public void ContinueRecordingFromLast()
    {
        // Continue recording from the skipped position
        if(wordIndex < wordImagePairs.Count)
        {
            endPanel.SetActive(false);
            recordingPanel.SetActive(true);
        }
        else
        {
            // if recorded all, maybe create new word image pairs?
            // currently does nothing.
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
