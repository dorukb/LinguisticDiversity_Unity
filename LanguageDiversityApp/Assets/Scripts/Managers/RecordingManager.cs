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
    public Button advanceButton;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI recordingNameText;
    public TextMeshProUGUI recordingCounterText;

    public GameObject recordingPanel;
    public GameObject endPanel;
    public GameObject endPanelContinueButton;

    public RecordingUIController uiController;

    private RecordAudio recorder;
    private int wordIndex = 0;
    private string currentRecordingId = "";
    private AudioClip currentRecording;
    private bool isRecording = false;

    private Dictionary<string, AudioClip> savedRecordings;
    public enum RecordingState
    {
        Waiting,
        Recording,
        Recorded,
        Discarded
    }
    private void Awake()
    {
        advanceButton.onClick.AddListener(NextImage);
    }
    private void Start()
    {
        recorder = FindObjectOfType<RecordAudio>();
        savedRecordings = new Dictionary<string, AudioClip>();

        //shuffle the words for more uniform data collection
        wordImagePairs.Shuffle();
        wordIndex = 0;
        UpdateRecordingUI(RecordingState.Waiting);
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

        UpdateRecordingUI(RecordingState.Recording);
    }


    public void StopRecording()
    {
        if (!isRecording) return;

        isRecording = false;
        recorder.StopRecording();

#if !UNITY_WEBGL || UNITY_EDITOR
        // When not WebGL, can directly update UI and allow advancing as no upload/async operation is happening.
        UpdateRecordingUI(RecordingState.Recorded);
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

        UpdateRecordingUI(RecordingState.Discarded);
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
        UpdateRecordingUI(RecordingState.Recorded);
    }
    public int GetRemainingImageCount()
    {
        return wordImagePairs.Count - (wordIndex + 1);
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
            UpdateRecordingUI(RecordingState.Waiting);
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
    private void UpdateRecordingUI(RecordingState visualState)
    {
        switch (visualState)
        {
            case RecordingState.Waiting:

                discardButton.SetActive(false);
                advanceButton.gameObject.SetActive(true);
                recordingVisual.SetActive(false);
                statusText.text = "Hold to Record";
                recordingNameText.text = "";

                imageShown.sprite = wordImagePairs[wordIndex].sprite;
                recordingCounterText.text = (wordIndex + 1).ToString() + " / " + wordImagePairs.Count;
                break;

            case RecordingState.Recording:

                discardButton.SetActive(false);
                advanceButton.gameObject.SetActive(false);
                recordingVisual.SetActive(false);
                statusText.text = "Recording...";
                recordingNameText.text = "";
                break;

            case RecordingState.Recorded:

                discardButton.SetActive(true);
                advanceButton.gameObject.SetActive(true);
                recordingVisual.SetActive(true);
                statusText.text = "Hold to Record";
                recordingNameText.text = currentRecordingId;
                break;

            case RecordingState.Discarded:

                discardButton.SetActive(false);
                advanceButton.gameObject.SetActive(true);
                recordingVisual.SetActive(false);
                statusText.text = "Hold to Record Again.";
                recordingNameText.text = "";
                break;
            default:
                break;
        }
    }
}
