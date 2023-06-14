using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Singletone<SFXManager>, ISFXManager
{
    private AudioSource[] tracks;
    private Coroutine soundEffectCoroutine;
    void Start()
    {
        tracks = new AudioSource[4];
        for (int i = 0; i < 4; i++)
        {
            tracks[i] = transform.GetChild(i)?.GetComponent<AudioSource>();
        }
    }
    public void PlayTrack(AttackKey key)
    {
        for (int i = 0; i < 4; i++)
        {
            if (((int)key & 1 << i) == 1 << i) tracks[i].Play();
            soundEffectCoroutine = StartCoroutine(StopSoundEffect(tracks[i]));
        }


    }

    IEnumerator StopSoundEffect(AudioSource audioSource)
    {
        yield return new WaitForSeconds(2f); // Adjust the duration as needed
        audioSource.Stop();
        soundEffectCoroutine = null;
    }

}



