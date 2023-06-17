using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowzoneActivator : MonoBehaviour
{
    private AudioSource audioSource;
    private IEnumerator currentActivation = null;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnDisable()
    {
        Time.timeScale = 1f;
    }
    public void Activate()
    {
        if(currentActivation != null) StopCoroutine(currentActivation);
        float duration = Utils.GetBaseDuration(MusicManager.Instance.bpm);
        currentActivation = ActivateSlowZoneCoroutine(duration);
        StartCoroutine(currentActivation);
    }
    private IEnumerator ActivateSlowZoneCoroutine(float duration)
    {
        Time.timeScale = 0.5f;
        audioSource.pitch = 0.5f;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1f;
        audioSource.pitch = 1f;
        currentActivation = null;
    }
}