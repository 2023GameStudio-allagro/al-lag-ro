using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicManager : Singletone, IMusicManager
{
    public UnityEvent _noteEvent = new UnityEvent();
    public UnityEvent _beatEvent = new UnityEvent();
    public UnityEvent noteEvent { get {return _noteEvent;} }
    public UnityEvent beatEvent { get {return _beatEvent;} }

    private IMusicManager currentMusicManager;
    private StageMusicManager stageMusicManager;
    private BossMusicManager bossMusicManager;

    // Start is called before the first frame update
    void Start()
    {
        stageMusicManager = GetComponent<StageMusicManager>();
        bossMusicManager = GetComponent<BossMusicManager>();
        stageMusicManager.SetSharedEvent(noteEvent, beatEvent);
        bossMusicManager.SetSharedEvent(noteEvent, beatEvent);

        LoadBGMData(1);
        StartMusic();
    }

    public void LoadBGMData(int stageNo)
    {
        currentMusicManager?.EndMusic();

        bool isBossLevel = (stageNo-1) % 2 == 1;
        if(isBossLevel) currentMusicManager = bossMusicManager;
        else currentMusicManager = stageMusicManager;
        currentMusicManager?.LoadBGMData((stageNo-1) / 2 + 1);

        Debug.Log(currentMusicManager);
    }

    public void StartMusic()
    {
        currentMusicManager?.StartMusic();
    }
    public void EndMusic()
    {
        currentMusicManager?.EndMusic();
    }
    public void CallNextPhase()
    {
       if (currentMusicManager is BossMusicManager == false) return;
       BossMusicManager manager = (BossMusicManager)currentMusicManager;
       manager?.CallNextPhase();
    }
}
