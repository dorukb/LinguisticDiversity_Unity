using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.TMP_Dropdown;
public class FormManager : MonoBehaviour
{

    //public TMP_Dropdown nativeLangDropdown;
    public TMP_Dropdown genderDropdown;
    public TMP_Dropdown contributionLangDropdown;
    public TMP_Dropdown profDropdown;
    public TMP_InputField ageInput;
    public TMP_InputField nativeLangInput;

    private FormData localFormData;

    //prob read from a file since this is LONG.
    // We let the user type in their native language.
    //private string[] nativeLanguages = new string[]
    //{
    //    "Turkish",
    //    "German",
    //    "Azeri",
    //};

    // This will be relatively short, depends on how many languages we support. <25 i'd say. So can be written here or also in file.
    private string[] contributionLanguages = new string[]
    {
        "Turkish",
        "Azeri",
    };
    private void Awake()
    {
        //ConfigureNativeLanguageDropdown();
        ConfigureContributionLanguageDropdown();

        //ageInput.keyboardType = TouchScreenKeyboardType.NumberPad;
        ageInput.onValueChanged.AddListener(OnAgeChange);
        contributionLangDropdown.onValueChanged.AddListener(OnContributionLanguageChange);

        //nativeLangDropdown.onValueChanged.AddListener(OnNativeLanguageChange);
        nativeLangInput.onValueChanged.AddListener(OnNativeLanguageChange);

        genderDropdown.onValueChanged.AddListener(OnGenderChange);
        profDropdown.onValueChanged.AddListener(OnProficiencyChange);

        localFormData = new FormData();
        localFormData.Print();
        OnAgeChange(ageInput.text);
        OnGenderChange(genderDropdown.value);
        //OnNativeLanguageChange(nativeLangDropdown.value);
        OnNativeLanguageChange(nativeLangInput.text);
        OnContributionLanguageChange(contributionLangDropdown.value);
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
        CheckAndHideProficiencyButton();
    }
    //public void OnNativeLanguageChange(int value)
    //{
    //    localFormData.nativeLanguage = nativeLangDropdown.captionText.text;
    //    if(localFormData.nativeLanguage == localFormData.contributionLanguage)
    //    {
    //        profDropdown.gameObject.SetActive(false);
    //        localFormData.proficiencyLevel = 2; //native
    //    }
    //    else
    //    {
    //        profDropdown.gameObject.SetActive(true);
    //    }
    //}
    public void OnContributionLanguageChange(int value)
    {
        localFormData.contributionLanguage = contributionLangDropdown.captionText.text;
        CheckAndHideProficiencyButton();
    }

    private void CheckAndHideProficiencyButton()
    {
        if (localFormData.nativeLanguage == null || localFormData.contributionLanguage == null) Debug.Log("null error.");
        if (localFormData.nativeLanguage.ToUpper() == null) Debug.Log("to upper returns null");
        if (localFormData.nativeLanguage.ToUpper() == localFormData.contributionLanguage.ToUpper())
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


    // Do this for all dropdown that have LOTS of options, and so are read from file.
    // For our purposes, native language & contribution language are the only ones.
    // rest are configured inside the editor.
    //private void ConfigureNativeLanguageDropdown()
    //{

    //    List<OptionData> nativeLangOptions = new List<OptionData>();
    //    foreach (var lang in nativeLanguages)
    //    {
    //        nativeLangOptions.Add(new OptionData(lang));
    //    }
    //    nativeLangDropdown.AddOptions(nativeLangOptions);
    //}
    private void ConfigureContributionLanguageDropdown()
    {

        List<OptionData> options = new List<OptionData>();
        foreach (var lang in contributionLanguages)
        {
            options.Add(new OptionData(lang));
        }
        contributionLangDropdown.AddOptions(options);
    }


    public void OnSubmitButton()
    {
        DataManager.Instance.SaveFormData(localFormData, () =>
        { 
            FindObjectOfType<SceneTransition>().LoadRecordingScene(); 
        });
    }

}
