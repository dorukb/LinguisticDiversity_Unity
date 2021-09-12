using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RecordingUIController;

public class RecordingManager : MonoBehaviour
{
    public List<Word> wordImagePairs;
    public RecordingUIController uiController;

    private RecordAudio recorder;
    private int wordIndex = 0;
    private string currentRecordingId = "";
    private AudioClip currentRecording;
    private bool isRecording = false;

    private Dictionary<string, AudioClip> savedRecordings;

    private void Start()
    {
        recorder = FindObjectOfType<RecordAudio>();
        savedRecordings = new Dictionary<string, AudioClip>();

        //shuffle the words for more uniform data collection
        wordImagePairs.Shuffle();
        wordIndex = 0;

        if (uiController == null) uiController = FindObjectOfType<RecordingUIController>();
        uiController.OnRecordingStateChange(RecordingState.Waiting);
    }

    public void StartRecording()
    {
        if (isRecording) return;

        isRecording = true;
        // Remove previous recording of the same word.
        if (savedRecordings.ContainsKey(currentRecordingId))
        {
            savedRecordings.Remove(currentRecordingId);
        }

        // will be null if on WebGL, since it wont return but keep the data on the browser side.
        currentRecording = recorder.Record(wordImagePairs[wordIndex].keyword);
        currentRecordingId = wordImagePairs[wordIndex].keyword;

        uiController.OnRecordingStateChange(RecordingState.Recording);
    }


    public void StopRecording()
    {
        if (!isRecording) return;

        isRecording = false;
        recorder.StopRecording();

#if !UNITY_WEBGL || UNITY_EDITOR
        // When not WebGL, can directly update UI and allow advancing as no upload/async operation is happening.
        uiController.OnRecordingStateChange(RecordingState.Recorded);
#endif
        if(currentRecording != null)
        {
            currentRecording.name = currentRecordingId;
        }
        savedRecordings.Add(currentRecordingId, currentRecording);
    }

    public void DiscardRecording()
    {
        if (!savedRecordings.Remove(currentRecordingId))
        {
            Debug.LogError("Unable to discard recording with id: " + currentRecordingId);
        }
        currentRecording = null;
        currentRecordingId = "";

        uiController.OnRecordingStateChange(RecordingState.Discarded);
    }
    public void PlayCurrentRecording()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        MicrophoneWeb.PlayCurrentRecording();
#else
        recorder.PlayRecording(currentRecording);
#endif
    }
    public void RecordingDataReady() 
    {
        // Case for WebGL versions. 
        // Since the audio processing on the browser side is async and out of unity's control, we require this callback.
        // now we can advance to the next item safely, without losing data.
        uiController.OnRecordingStateChange(RecordingState.Recorded);
    }
    // Called by next button
    public void NextImage()
    {
        wordIndex++;
        if (wordIndex >= wordImagePairs.Count)
        {
            FinishRecordingPhase();
        }
        else
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // when the advance/button is clicked, if on WebGL, audio files are sent one by one by the js code.
            // Call to plugin code which will in turn call JS submit function.
            MicrophoneWeb.SubmitCurrentRecording();
#endif
            currentRecording = null;
            currentRecordingId = "";
            uiController.OnRecordingStateChange(RecordingState.Waiting);
        }
       
    }
    public void FinishRecordingPhase()
    {
        bool canContinueLater = wordIndex < wordImagePairs.Count;
        uiController.ShowEndPanel(canContinueLater);

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
            uiController.ShowRecordingPanel();
        }
        else
        {
            // if recorded all, maybe create new word image pairs?
            // currently does nothing.
        }
    }
    public Word GetCurrentWord()
    {
        return wordImagePairs[wordIndex];
    }
    public int GetCurrentImageIndex()
    {
        return wordIndex;
    }
    public int GetTotalImageCount()
    {
        return wordImagePairs.Count;
    }
    public int GetRemainingImageCount()
    {
        return wordImagePairs.Count - (wordIndex + 1);
    }

    // Called by Start Again Button
    public void LoadMenuScene()
    {
        SceneTransition.LoadMenuScene();
    }
    private void SubmitRecordings()
    {
        DataManager.Instance.SendDataToServer(() =>
        {
            SceneTransition.LoadMenuScene();
        }
        // TODO: Also handle failed case and give option to retry.
        );
    }
}
