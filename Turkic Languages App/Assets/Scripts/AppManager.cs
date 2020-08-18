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
    public GameObject discardButton;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI recordingNameText;
    public TextMeshProUGUI recordingCounterText;

    public GameObject recordingPanel;
    public GameObject endPanel;

    private RecordAudio recorder;
    private int imageIndex;
    private string currentRecordingId = "";
    private AudioClip currentRecording;
    public Dictionary<string, AudioClip> savedRecordings;
    private void Start()
    {
        recorder = FindObjectOfType<RecordAudio>();
        savedRecordings = new Dictionary<string, AudioClip>();
        recordingVisual.SetActive(false);

        statusText.text = "Hold to start Recording.";
        imageIndex = 0;
        imageShown.sprite = sprites[imageIndex];
        recordingCounterText.text = (imageIndex + 1).ToString() + " / " + sprites.Count;
    }

    public void StartRecording()
    {
        if (savedRecordings.ContainsKey(currentRecordingId))
        {
            savedRecordings.Remove(currentRecordingId);
        }
        currentRecording = recorder.Record();
        currentRecordingId = sprites[imageIndex].name;

        recordingVisual.SetActive(false);
        statusText.text = "Recording...";
    }
    public void StopRecording()
    {
        if (currentRecording == null) return;

        statusText.text = "Finished.";
        recorder.StopRecording();

        discardButton.SetActive(true);
        recordingVisual.SetActive(true);

        recordingNameText.text = currentRecordingId;
        currentRecording.name = currentRecordingId;
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
        imageIndex++;
        recordingCounterText.text = (imageIndex + 1).ToString() + " / " + sprites.Count;

        if (imageIndex >= sprites.Count)
        {
            FinishRecordingPhase();
            //DataManager.Instance.SendDataToServer();
        }
        else
        {
            imageShown.sprite = sprites[imageIndex];

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
