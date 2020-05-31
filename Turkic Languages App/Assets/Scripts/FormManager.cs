using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.TMP_Dropdown;
public class FormManager : MonoBehaviour
{

    public TMP_Dropdown nativeLangDropdown;
    public TMP_Dropdown genderDropdown;
    public TMP_Dropdown contributionLangDropdown;
    public TMP_Dropdown profDropdown;
    public TMP_InputField ageInput;

    private FormData localFormData;

    //prob read from a file since this is LONG.
    private string[] nativeLanguages = new string[]
    {
        "All the others",
        "Turkish",
        "German",
        "Azeri",
    };

    // This will be relatively short, depends on how many languages we support. <25 i'd say. So can be written here or also in file.
    private string[] contributionLanguages = new string[]
    {
        "Turkish",
        "Azeri",
    };
    private void Awake()
    {
        ConfigureNativeLanguageDropdown();
        ConfigureContributionLanguageDropdown();

        //ageInput.keyboardType = TouchScreenKeyboardType.NumberPad;
        ageInput.onValueChanged.AddListener(OnAgeChange);
        contributionLangDropdown.onValueChanged.AddListener(OnContributionLanguageChange);
        nativeLangDropdown.onValueChanged.AddListener(OnNativeLanguageChange);
        genderDropdown.onValueChanged.AddListener(OnGenderChange);
        profDropdown.onValueChanged.AddListener(OnProficiencyChange);

        localFormData = new FormData();
        OnAgeChange(ageInput.text);
        OnGenderChange(genderDropdown.value);
        OnNativeLanguageChange(nativeLangDropdown.value);
        OnContributionLanguageChange(contributionLangDropdown.value);
        OnProficiencyChange(profDropdown.value);
    }

    public void OnAgeChange(string value)
    {
        Debug.Log("Age: " + value);
        int parsedInt = -1;
        int.TryParse(value, out parsedInt);
        localFormData.age = parsedInt;
    }
    public void OnNativeLanguageChange(int value)
    {
        Debug.Log(nativeLangDropdown.captionText.text);
        localFormData.nativeLanguage = nativeLangDropdown.captionText.text;
    }
    public void OnContributionLanguageChange(int value)
    {
        Debug.Log(contributionLangDropdown.captionText.text);
        localFormData.contributionLanguage = contributionLangDropdown.captionText.text;
    }
    public void OnProficiencyChange(int value)
    {
        Debug.Log(profDropdown.captionText.text);
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
                localFormData.gender = "NONE";
                break;
        }
        Debug.Log("gender value: " + value);
    }


    // Do this for all dropdown that have LOTS of options, and so are read from file.
    // For our purposes, native language & contribution language are the only ones.
    // rest are configured inside the editor.
    private void ConfigureNativeLanguageDropdown()
    {

        List<OptionData> nativeLangOptions = new List<OptionData>();
        foreach (var lang in nativeLanguages)
        {
            nativeLangOptions.Add(new OptionData(lang));
        }
        nativeLangDropdown.AddOptions(nativeLangOptions);
    }
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
        DataManager.Instance.SaveFormData(localFormData);
        FindObjectOfType<SceneTransition>().LoadRecordingScene();
    }

}
