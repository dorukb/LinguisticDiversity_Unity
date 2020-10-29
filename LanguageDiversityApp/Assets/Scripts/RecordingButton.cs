using UnityEngine;
using UnityEngine.EventSystems;

public class RecordingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    RecordingManager manager;
    void Awake()
    {
        manager = FindObjectOfType<RecordingManager>();
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
