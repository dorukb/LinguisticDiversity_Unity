using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputFieldVisual : MonoBehaviour
{

    public string defaultLabel = "Native language";
    public string fillErrorLabel = "Native language (required)";

    public TextMeshProUGUI label;
    public Image fieldBackground;
    public Material fillErrorMat;
    public Material defaultMat;
    public GameObject fillErrorIcon;

    public void ShowFillError()
    {
        label.text = fillErrorLabel;
        fieldBackground.material = fillErrorMat;
        fillErrorIcon.SetActive(true);
    }
    public void ClearFillError() 
    {
        label.text = defaultLabel;
        fieldBackground.material = defaultMat;
        fillErrorIcon.SetActive(false);
    }
