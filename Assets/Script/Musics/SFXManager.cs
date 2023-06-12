using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Singletone<SFXManager>, ISFXManager
{
    private AudioSource[] tracks;
    void Start()
    {
        tracks = new AudioSource[4];
        for(int i=0; i<4; i++)
        {
            tracks[i] = transform.GetChild(i)?.GetComponent<AudioSource>();
        }
    }
    public void PlayTrack(AttackKey key)
    {
        for(int i=0; i<4; i++)
        {
            if( ((int)key & 1 << i) == 1 << i ) tracks[i].Play();
        }
    }
}
