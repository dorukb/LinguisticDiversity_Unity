using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputFieldVisual : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Image fieldBackground;
    public Material fillErrorMat;
    public Material defaultMat;
    public GameObject fillErrorIcon;

    private string defaultLabel;

    private void Start()
    {
        defaultLabel = label.text;
    }
    public void ShowFillError()
    {
        label.text = defaultLabel + " (required)";
        fieldBackground.material = fillErrorMat;
        fillErrorIcon.SetActive(true);
    }
    public void ClearFillError() 
    {
        label.text = defaultLabel;
        fieldBackground.material = defaultMat;
        fillErrorIcon.SetActive(false);
    }
}
