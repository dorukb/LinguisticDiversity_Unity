using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecordingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // time(secs) to wait before stopping recording after release.
    // if pointer Down happens during this time, no cancelletion occurs.
    public float toleranceDuration = 0.5f;


    RecordingManager manager;

    bool pointerDown = false;
    Coroutine stoppingCR;

    void Awake()
    {
        manager = FindObjectOfType<RecordingManager>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        manager.StartRecording();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;

        if(stoppingCR != null)
        {
            StopCoroutine(stoppingCR);
        }
        stoppingCR = StartCoroutine(MaybeStopRecording());
    }
    private IEnumerator MaybeStopRecording()
    {
        yield return new WaitForSeconds(toleranceDuration);

        if (!pointerDown)
        {
            manager.StopRecording();
        }
        else
        {
            // pointer down occured during our wait, do not stop recording.
        }
    }
}
