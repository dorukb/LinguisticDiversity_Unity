using UnityEngine;

public class CheckboxButton : MonoBehaviour
{
    public GameObject selectedImage;
    public bool consentGiven = false;
    public void ToggleCross()
    {
        if (consentGiven)
        {
            consentGiven = false;
            selectedImage.SetActive(false);

            Debug.Log("Consent NOT given.");
        }
        else
        {
            consentGiven = true;
            selectedImage.SetActive(true);
            Debug.Log("Consent given.");
        }
    }
}
