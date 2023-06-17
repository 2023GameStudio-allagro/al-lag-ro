using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TitleBeatManager : MonoBehaviour
{
    public UnityEvent beatEvent = new UnityEvent();
    void Start()
    {
        StartCoroutine(SendBeatEvent(120));
    }
    IEnumerator SendBeatEvent(float bpm, float delay=0)
    {
        WaitForSeconds interval = new WaitForSeconds(60f / bpm);
        yield return new WaitForSeconds(delay);
        while(true)
        {
            beatEvent?.Invoke();
            yield return interval;
        }
    }
}
