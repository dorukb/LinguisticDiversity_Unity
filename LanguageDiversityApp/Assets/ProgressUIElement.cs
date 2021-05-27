using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUIElement : MonoBehaviour
{
    public Image tickImage;

    public void Awake()
    {
        tickImage.gameObject.SetActive(false);
    }
    public void ToggleTick(bool state)
    {
        tickImage.gameObject.SetActive(state);
    }
}
