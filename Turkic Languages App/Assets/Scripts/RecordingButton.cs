using UnityEngine;
using UnityEngine.EventSystems;

public class RecordingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    AppManager manager;
    void Awake()
    {
        manager = FindObjectOfType<AppManager>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        manager.StartRecording();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        manager.StopRecording();
    }

}
