using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabUINavigation : MonoBehaviour
{
    // Fill these in the editor, with any selectable UI element that will be cycled thru using "Tab" key input.
    // organize indices as required by the navigation sequence.
    // i.e, elm at index 0 will start selected, when 'Tab' is pressed once, index 1 will be selected and so on.
    // supports wrap around as well.

    public List<Selectable> selectableElements;

    int selectedIndex = 0;
    void Start()
    {
        // Select the first one by default, 
        selectableElements[0].Select();
    }
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            selectedIndex = (selectedIndex + 1) % selectableElements.Count;
            var selectedElm = selectableElements[selectedIndex];

            if (selectedElm == null || selectedElm.gameObject.activeSelf == false)
            {
                // skip this element, as its either deleted or not active currently.
                selectedIndex = (selectedIndex + 1) % selectableElements.Count;
            }
            selectableElements[selectedIndex].Select();
        }
    }
}
