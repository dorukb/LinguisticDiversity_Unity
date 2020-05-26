using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.TMP_Dropdown;
public class FormManager : MonoBehaviour
{

    public TMP_Dropdown NativeLangDropdown;
    private FormData localFormData;

    private string[] nativeLanguages = new string[]
    {
        "Turkish",
        "German",
        "Azeri",
    };
    private void Awake()
    {
        localFormData = new FormData();
        localFormData.gender = "MALE";
        localFormData.nativeLanguage = "TURKISH";

        ConfigureNativeLanguageDropdown();
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
        NativeLangDropdown.AddOptions(nativeLangOptions);
    }



    public void OnSubmitButton()
    {
        DataManager.Instance.SaveFormData(localFormData);
        FindObjectOfType<SceneTransition>().LoadRecordingScene();
    }

}
