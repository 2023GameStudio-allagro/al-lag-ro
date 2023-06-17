using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageMusicManager : MonoBehaviour, IMusicManager
{
    public float bpm { get { return currentSource?.bpm ?? 120; } }

    private UnityEvent<char> sharedNoteEvent;
    private UnityEvent<int> sharedBeatEvent;
    public UnityEvent<char> noteEvent { get {return sharedNoteEvent;} }
    public UnityEvent<int> beatEvent { get {return sharedBeatEvent;} }

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
        if(elapsedTime > 0f && !audioRunner.isPlaying)
        {
            isRunning = false;
            GameManager.Instance.GameClear();
        }
    }

    public void LoadBGMData(int stageNo)
    {
        if(stageNo < 1 || stageNo > sources.Length) return;
        currentSource = sources[stageNo - 1];
    }

    public void StartMusic()
    {
        elapsedTime = -GetInitialElapsedTime(bpm, currentSource.offset);
        isRunning = true;
        StartCoroutine(PlayDelayedMusic(GetInitialMusicDelay(bpm, currentSource.offset)));
    }

    public void EndMusic()
    {
        elapsedTime = -GetInitialElapsedTime(bpm, currentSource.offset);
        isRunning = false;
        audioRunner.Stop();
    }

    public void SetSharedEvent(UnityEvent<char> note, UnityEvent<int> beat)
    {
        sharedNoteEvent = note;
        sharedBeatEvent = beat;
    }

    private void MakeBeatEvent(float prevTime, float curTime)
    {
        float bpm = currentSource.bpm;
        int prevBeatNo = GetBeatNo(prevTime, bpm);
        int curBeatNo = GetBeatNo(curTime, bpm);
        if(Input.GetMouseButtonDown(0)) Debug.Log(curBeatNo);
        if(Input.GetMouseButtonDown(1)) Debug.Log("This is mainBeat!");

        if(curBeatNo < 0) return;
        if(prevBeatNo < 0 && curBeatNo >= 0) beatEvent?.Invoke(0);
        else if(prevBeatNo/4 != curBeatNo/4)
        {
            beatEvent?.Invoke(curBeatNo/4);
        }
    }

    private void MakeNoteEvent(float prevTime, float curTime)
    {
        float bpm = currentSource.bpm;
        float delay = Utils.GetBaseDuration(bpm);
        
        if(IsMusicStampEnd(curTime + delay)) return;
        int prevBeatNo = GetBeatNo(prevTime + delay, bpm);
        int curBeatNo = GetBeatNo(curTime + delay, bpm);
        if(curBeatNo < 0) return;

        if(prevBeatNo != curBeatNo && currentSource.HasNote(curBeatNo))
        {
            char key = currentSource.GetNote(curBeatNo);
            noteEvent?.Invoke(key);
        }
    }

    private IEnumerator PlayDelayedMusic(float delaySec)
    {
        yield return new WaitForSeconds(delaySec);
        audioRunner.clip = currentSource.audioClip;
        audioRunner.Play();
    }

    private bool IsMusicStampEnd(float time)
    {
        if(currentSource == null) return true;
        return (time + currentSource.offset) > currentSource.audioClip.length;
    }

    private int GetBeatNo(float time, float bpm)
    {
        float interval = 60 / (bpm * 4);
        return (int)Mathf.Floor(time / interval );
    }

    private float GetInitialElapsedTime(float bpm, float offset)
    {
        float beatDelay = Utils.GetBaseDuration(bpm);
        if(offset > beatDelay) return offset;
        return beatDelay;
    }
    private float GetInitialMusicDelay(float bpm, float offset)
    {
        float beatDelay = Utils.GetBaseDuration(bpm);
        if(offset > beatDelay) return 0f;
        return beatDelay - offset;
    }
}