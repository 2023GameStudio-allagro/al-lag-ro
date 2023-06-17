using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Singletone<SFXManager>, ISFXManager
{
    private AudioSource SFXPlayer;
    [SerializeField] private AudioClip[] keySFX;
    [SerializeField] private AudioClip baseSFX;
    protected override void Awake()
    {
        base.Awake();
        SFXPlayer = GetComponent<AudioSource>();
    }
    public void PlayTrack(AttackKey key)
    {
        for (int i = 0; i < 4; i++)
        {
            if (((int)key & 1 << i) == 1 << i) SFXPlayer.PlayOneShot(keySFX[i]);
        }
    }
    public void PlayTrack(AttackKey key, float volume)
    {
        for (int i = 0; i < 4; i++)
        {
            if (((int)key & 1 << i) == 1 << i) SFXPlayer.PlayOneShot(keySFX[i], volume);
        }
    }
    public void PlayBase()
    {
        SFXPlayer.PlayOneShot(baseSFX, 0.4f);
    }
}



