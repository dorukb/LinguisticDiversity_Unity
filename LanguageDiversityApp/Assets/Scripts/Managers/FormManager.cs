using System;
using UnityEngine;
using TMPro;
public class FormManager : MonoBehaviour
{
    public TMP_Dropdown genderDropdown;
    public TMP_Dropdown profDropdown;
    public TMP_InputField ageInput;
    public TMP_InputField nativeLangInput;
    public TMP_InputField contributionLangInput;

    public InputFieldUI nativeLanguageVisual;
    public InputFieldUI contributionLanguageVisual;

    public GameObject EmptyFieldsPopup;

    private FormData localFormData;
    private bool nativeLanguageEmptyFlag = false;
    private bool contributionLanguageEmptyFlag = false;

    private void Awake()
    {
        localFormData = new FormData();
        EmptyFieldsPopup.SetActive(false);
        AddButtonListeners();
        SetPlaceholderValues();
    }
    #region API
    public void OnSubmitButton()
    {
        nativeLanguageEmptyFlag = nativeLangInput.text.Length < 2;
        contributionLanguageEmptyFlag = contributionLangInput.text.Length < 2;

        if (nativeLanguageEmptyFlag || contributionLanguageEmptyFlag)
        {
            //not allow submission and notify user that thare are empty fields that needs to be filled in.
            if (nativeLanguageEmptyFlag) nativeLanguageVisual.ShowFillError();
            if (contributionLanguageEmptyFlag) contributionLanguageVisual.ShowFillError();

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
    #endregion

    private void OnAgeChange(string value)
    {
        int parsedInt = -1;
        int.TryParse(value, out parsedInt);
        localFormData.age = parsedInt;
    }
    private void OnNativeLanguageChange(string text)
    {
        localFormData.nativeLanguage = text.ToUpper();
        if (nativeLanguageEmptyFlag && !string.IsNullOrEmpty(text))
        {
            nativeLanguageEmptyFlag = false;
            nativeLanguageVisual.ClearFillError();
        }
        CheckAndHideProficiencyButton();
    }
    private void OnContributionLanguageChange(string text)
    {
        localFormData.contributionLanguage = text.ToUpper();
        if (contributionLanguageEmptyFlag && !string.IsNullOrEmpty(text))
        {
            contributionLanguageEmptyFlag = false;
            contributionLanguageVisual.ClearFillError();
        }
        CheckAndHideProficiencyButton();
    }
    private void OnProficiencyChange(int value)
    {
        localFormData.proficiencyLevel = value;
    }
    private void OnGenderChange(Int32 value)
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
                localFormData.gender = "OTHER";
                break;
        }
    }
    private void CheckAndHideProficiencyButton()
    {
        if (string.IsNullOrEmpty(localFormData.nativeLanguage) || string.IsNullOrEmpty(localFormData.contributionLanguage))
        { 
            // either one is invalid
            profDropdown.gameObject.SetActive(true);
        }
        else if (localFormData.nativeLanguage.ToUpper() == localFormData.contributionLanguage.ToUpper())
        { 
            // languages match
            profDropdown.gameObject.SetActive(false);
            localFormData.proficiencyLevel = 2; //native
        }
        else
        { 
            // languages dont match
            profDropdown.gameObject.SetActive(true);
        }
    }
    private void SetPlaceholderValues()
    {
        OnAgeChange(ageInput.text);
        OnGenderChange(genderDropdown.value);
        OnNativeLanguageChange(nativeLangInput.text);
        OnContributionLanguageChange(contributionLangInput.text);
        OnProficiencyChange(profDropdown.value);
    }
    private void AddButtonListeners()
    {
        ageInput.onValueChanged.AddListener(OnAgeChange);
        nativeLangInput.onValueChanged.AddListener(OnNativeLanguageChange);
        contributionLangInput.onValueChanged.AddListener(OnContributionLanguageChange);
        genderDropdown.onValueChanged.AddListener(OnGenderChange);
        profDropdown.onValueChanged.AddListener(OnProficiencyChange);
    }
}
