using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageMusicManager : MonoBehaviour, IMusicManager
{
    private UnityEvent sharedNoteEvent;
    private UnityEvent sharedBeatEvent;
    public UnityEvent noteEvent { get {return sharedNoteEvent;} }
    public UnityEvent beatEvent { get {return sharedBeatEvent;} }

    [SerializeField] private StageMusicSource[] sources;
    private StageMusicSource currentSource;
    private AudioSource audioRunner;

    private float elapsedTime = -3f;
    private bool isRunning = false;

    void Start()
    {
        audioRunner = GetComponent<AudioSource>();
        audioRunner.playOnAwake = false;
        audioRunner.loop = false;
    }

    void Update()
    {
        if(!isRunning || currentSource == null) return;
        float prevElapsedTime = elapsedTime;
        elapsedTime += Time.deltaTime;

        MakeBeatEvent(prevElapsedTime, elapsedTime);
        MakeNoteEvent(prevElapsedTime, elapsedTime);
        if(!audioRunner.isPlaying) isRunning = false;
    }

    public void LoadBGMData(int stageNo)
    {
        if(stageNo < 1 || stageNo > sources.Length) return;
        currentSource = sources[stageNo - 1];
    }

    public void StartMusic()
    {
        elapsedTime = -3f - currentSource.offset;
        isRunning = true;
        StartCoroutine(PlayDelayedMusic(3f));
    }

    public void EndMusic()
    {
        elapsedTime = -3f - currentSource.offset;
        isRunning = false;
        audioRunner.Stop();
    }

    public void SetSharedEvent(UnityEvent note, UnityEvent beat)
    {
        sharedNoteEvent = note;
        sharedBeatEvent = beat;
    }

    private void MakeBeatEvent(float prevTime, float curTime)
    {
        float bpm = currentSource.bpm;
        int prevBeatNo = GetBeatNo(prevTime, bpm);
        int curBeatNo = GetBeatNo(curTime, bpm);
        if(curBeatNo < 0) return;
        if(prevBeatNo < 0 && curBeatNo >= 0) beatEvent?.Invoke();
        else if(prevBeatNo/4 != curBeatNo/4) beatEvent?.Invoke();
    }

    private void MakeNoteEvent(float prevTime, float curTime)
    {
        float bpm = currentSource.bpm;
        float delay = 1f;
        int prevBeatNo = GetBeatNo(prevTime + delay, bpm);
        int curBeatNo = GetBeatNo(curTime + delay, bpm);
        if(curBeatNo < 0) return;
        if(prevBeatNo != curBeatNo && currentSource.HasNote(curBeatNo)) noteEvent?.Invoke();
    }

    private IEnumerator PlayDelayedMusic(float delaySec)
    {
        yield return new WaitForSeconds(delaySec);
        audioRunner.clip = currentSource.audioClip;
        audioRunner.Play();
    }

    private int GetBeatNo(float time, float bpm)
    {
        float interval = 60 / (bpm * 4);
        return (int)Mathf.Floor(time / interval );
    }
}