using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordingUIController : MonoBehaviour
{
    [Header("Recording Panel child References")]
    public GameObject recordingInfo;
    public GameObject discardButton;
    public Button advanceButton;
    public Image imageShown;

    public TextMeshProUGUI statusText;
    public TextMeshProUGUI recordingNameText;
    public TextMeshProUGUI recordingCounterText;

    [Header("Parent References")]
    public GameObject recordingPanel;
    public GameObject endPanel;
    public GameObject endPanelContinueButton;

    private RecordingManager recordingManager;
    public enum RecordingState
    {
        Waiting,
        Recording,
        Recorded,
        Discarded
    }
    private void Awake()
    {
        recordingManager = FindObjectOfType<RecordingManager>();
    }
    public void ShowRecordingPanel()
    {
        recordingPanel.SetActive(true);
        endPanel.SetActive(false);
    }
    public void ShowEndPanel(bool canContinueLater)
    {
        endPanel.SetActive(true);
        endPanelContinueButton.SetActive(canContinueLater);
        recordingPanel.SetActive(false);
    }
    public void OnRecordingStateChange(RecordingState visualState, string currentRecordingId = "")
    {
        switch (visualState)
        {
            case RecordingState.Waiting:

                discardButton.SetActive(false);
                advanceButton.gameObject.SetActive(true);
                recordingInfo.SetActive(false);
                statusText.text = "Hold to Record";
                recordingNameText.text = "";

                UpdateImageAndCounter();

                break;

            case RecordingState.Recording:

                discardButton.SetActive(false);
                advanceButton.gameObject.SetActive(false);
                recordingInfo.SetActive(false);
                statusText.text = "Recording...";
                recordingNameText.text = "";
                break;

            case RecordingState.Recorded:

                discardButton.SetActive(true);
                advanceButton.gameObject.SetActive(true);
                recordingInfo.SetActive(true);
                statusText.text = "Hold to Record";
                recordingNameText.text = recordingManager.GetCurrentWord().keyword;
                break;

            case RecordingState.Discarded:

                discardButton.SetActive(false);
                advanceButton.gameObject.SetActive(true);
                recordingInfo.SetActive(false);
                statusText.text = "Hold to Record";
                recordingNameText.text = "";
                break;
            default:
                break;
        }
    }

    private void UpdateImageAndCounter()
    {
        var currWord = recordingManager.GetCurrentWord();
        int currentCount = recordingManager.GetCurrentImageIndex() + 1;
        int totalCount = recordingManager.GetTotalImageCount();

        imageShown.sprite = currWord.sprite;
        recordingCounterText.text = currentCount.ToString() + " / " + totalCount;
    }
}
