using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.TMP_Dropdown;
public class FormManager : MonoBehaviour
{
    public TMP_Dropdown genderDropdown;
    public TMP_Dropdown profDropdown;
    public TMP_InputField ageInput;
    public TMP_InputField nativeLangInput;
    public TMP_InputField contributionLangInput;


    public InputFieldVisual nativeLanguageVisual;
    public InputFieldVisual contributionLanguageVisual;

    public GameObject EmptyFieldsPopup;
    private FormData localFormData;

    private bool nativeLangEmpty = false;
    private bool contrLangEmpty = false;
    private void Awake()
    {
        localFormData = new FormData();
        EmptyFieldsPopup.SetActive(false);

        //ageInput.keyboardType = TouchScreenKeyboardType.NumberPad;
        ageInput.onValueChanged.AddListener(OnAgeChange);
        nativeLangInput.onValueChanged.AddListener(OnNativeLanguageChange);
        contributionLangInput.onValueChanged.AddListener(OnContributionLanguageChange);
        genderDropdown.onValueChanged.AddListener(OnGenderChange);
        profDropdown.onValueChanged.AddListener(OnProficiencyChange);

        OnAgeChange(ageInput.text);
        OnGenderChange(genderDropdown.value);
        OnNativeLanguageChange(nativeLangInput.text);
        OnContributionLanguageChange(contributionLangInput.text);
        OnProficiencyChange(profDropdown.value);
    }

    public void OnAgeChange(string value)
    {
        int parsedInt = -1;
        int.TryParse(value, out parsedInt);
        localFormData.age = parsedInt;
    }
    public void OnNativeLanguageChange(string text)
    {
        localFormData.nativeLanguage = text.ToUpper();
        if (nativeLangEmpty && !string.IsNullOrEmpty(text))
        {
            nativeLangEmpty = false;
            nativeLanguageVisual.ClearFillError();
        }
        CheckAndHideProficiencyButton();
    }
    public void OnContributionLanguageChange(string text)
    {
        localFormData.contributionLanguage = text.ToUpper();
        if (contrLangEmpty && !string.IsNullOrEmpty(text))
        {
            contrLangEmpty = false;
            contributionLanguageVisual.ClearFillError();
        }
        CheckAndHideProficiencyButton();
    }

    private void CheckAndHideProficiencyButton()
    {
        if (string.IsNullOrEmpty(localFormData.nativeLanguage) || string.IsNullOrEmpty(localFormData.contributionLanguage))
        {
            profDropdown.gameObject.SetActive(true);
        }
        else if (localFormData.nativeLanguage.ToUpper() == localFormData.contributionLanguage.ToUpper())
        {
            profDropdown.gameObject.SetActive(false);
            localFormData.proficiencyLevel = 2; //native
        }
        else
        {
            profDropdown.gameObject.SetActive(true);
        }
    }

    public void OnProficiencyChange(int value)
    {
        localFormData.proficiencyLevel = value;
    }
    public void OnGenderChange(Int32 value)
    {
        switch (value)
        {
            case 0:
                localFormData.gender = "MALE";
                break;
            case 1:
                localFormData.gender = "FEMALE";
                break;
            default:
                localFormData.gender = "UNKNOWN";
                break;
        }
    }

    public void OnSubmitButton()
    {
        nativeLangEmpty = nativeLangInput.text.Length < 2;
        contrLangEmpty = contributionLangInput.text.Length < 2;

        if (nativeLangEmpty || contrLangEmpty) 
        {
            //not allow submission and notify user that thare are empty fields that need to be filled in.
            if (nativeLangEmpty) nativeLanguageVisual.ShowFillError();
            if (contrLangEmpty) contributionLanguageVisual.ShowFillError();

            EmptyFieldsPopup.SetActive(true);
        }
        else
        {
            DataManager.Instance.SaveFormData(localFormData, () =>
            {
                SceneTransition.LoadRecordingScene();
            });
        }
    }


}
