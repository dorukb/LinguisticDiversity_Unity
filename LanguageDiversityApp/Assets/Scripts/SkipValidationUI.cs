using TMPro;
using UnityEngine;

public class SkipValidationUI : MonoBehaviour
{
    public TextMeshProUGUI skipValidationText;
    public string replacementTextMarker = "$$";

    private RecordingManager recordingManager;

    private int startingIndex = -1;
    private int endingIndex = -1;

    string firstPiece;
    string secondPiece;
    void Awake()
    {
        recordingManager = FindObjectOfType<RecordingManager>();
        if(recordingManager == null)
        {
            Debug.LogError("SkipValidationUI script couldnt get reference to RecordingManager.");
        }
        startingIndex = skipValidationText.text.IndexOf(replacementTextMarker[0]);
        endingIndex = skipValidationText.text.LastIndexOf(replacementTextMarker[1]);

        firstPiece = skipValidationText.text.Substring(0, startingIndex);
        secondPiece = skipValidationText.text.Substring(endingIndex + 1);

    }
    private void OnEnable()
    {
        if(startingIndex != -1 && endingIndex != -1 && recordingManager != null)
        {
            string remainingImageCount = recordingManager.GetRemainingImageCount().ToString();
            skipValidationText.text = firstPiece + remainingImageCount + secondPiece;
        }
    }
}
