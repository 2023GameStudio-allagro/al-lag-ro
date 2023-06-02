using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossMusicManager : MonoBehaviour, IMusicManager
{
    private struct PhaseTime
    {
        public int phase;
        public float time;
    }

    private UnityEvent sharedNoteEvent;
    private UnityEvent<int> sharedBeatEvent;
    public UnityEvent noteEvent { get {return sharedNoteEvent;} }
    public UnityEvent<int> beatEvent { get {return sharedBeatEvent;} }
    public UnityEvent phaseEndEvent;

    [SerializeField] private BossMusicSource[] sources;
    private BossMusicSource currentSource;
    private AudioSource audioRunner;

    private PhaseTime elapsedPhaseTime;
    private bool isRunning = false;
    private int phase = 0;

    void Start()
    {
        audioRunner = GetComponent<AudioSource>();
        audioRunner.playOnAwake = false;
    }

    void Update()
    {
        if(!isRunning || currentSource == null) return;
        PhaseTime prevElapsedPhaseTime = elapsedPhaseTime;
        elapsedPhaseTime = GetElapsedTime(prevElapsedPhaseTime, Time.deltaTime);

        MakeBeatEvent(prevElapsedPhaseTime, elapsedPhaseTime);
        MakeNoteEvent(prevElapsedPhaseTime, elapsedPhaseTime);
        HandleAudio();
    }

    void HandleAudio()
    {
        if(elapsedPhaseTime.time <= 0f || audioRunner.isPlaying) return;
        phase++;
        phaseEndEvent?.Invoke();
        if(phase >= currentSource.phases)
        {
            isRunning = false;
            return;
        }
        audioRunner.clip = currentSource.audioClip[phase];
        audioRunner.loop = true;
        audioRunner.Play();
    }

    public void LoadBGMData(int stageNo)
    {
        if(stageNo < 1 || stageNo > sources.Length) return;
        currentSource = sources[stageNo - 1];
    }

    public void StartMusic()
    {
        if(currentSource == null) return;

        elapsedPhaseTime = new PhaseTime
        {
            phase = 0,
            time = -3f - currentSource.offset
        };
        isRunning = true;
        phase = 0;
        StartCoroutine(PlayDelayedFirstMusic(3f));
    }

    public void EndMusic()
    {
        if(currentSource == null) return;

        elapsedPhaseTime = new PhaseTime
        {
            phase = 0,
            time = -3f - currentSource.offset
        };
        isRunning = false;
        phase = 0;
        audioRunner.Stop();
    }

    public void CallNextPhase()
    {
        audioRunner.loop = false;
    }

    public void SetSharedEvent(UnityEvent note, UnityEvent<int> beat)
    {
        sharedNoteEvent = note;
        sharedBeatEvent = beat;
    }

    private void MakeBeatEvent(PhaseTime prevTime, PhaseTime curTime)
    {
        float bpm = currentSource.bpm;
        int prevBeatNo = GetBeatNo(prevTime.time, bpm);
        int curBeatNo = GetBeatNo(curTime.time, bpm);
        if(curBeatNo < 0) return;
        if(prevBeatNo < 0 && curBeatNo >= 0) beatEvent?.Invoke(0);
        else if(prevBeatNo/4 != curBeatNo/4) beatEvent?.Invoke(curBeatNo/4);
    }

    private void MakeNoteEvent(PhaseTime prevTime, PhaseTime curTime)
    {
        float bpm = currentSource.bpm;
        float delay = 1f;
        int prevBeatNo = GetBeatNo(prevTime.time + delay, bpm);
        int curBeatNo = GetBeatNo(curTime.time + delay, bpm);
        if(curBeatNo < 0) return;
        if(prevBeatNo != curBeatNo && currentSource.HasNote(curBeatNo, curTime.phase))
        {
            noteEvent?.Invoke();
        }
    }

    private IEnumerator PlayDelayedFirstMusic(float delaySec)
    {
        yield return new WaitForSeconds(delaySec);
        audioRunner.loop = true;
        audioRunner.clip = currentSource.audioClip[0];
        audioRunner.Play();
    }

    private PhaseTime GetElapsedTime(PhaseTime prev, float delta)
    {
        int prevPhase = prev.phase;
        float prevTime = prev.time;

        PhaseTime defaultResult = new PhaseTime
        {
            phase = prevPhase,
            time = prevTime + delta
        };

        if(!audioRunner.loop) return defaultResult;
        if(prevTime + delta < 0) return defaultResult;
        if(currentSource == null) return defaultResult;
        if(currentSource.phases >= prev.phase) return defaultResult;

        float currentPhaseSec = currentSource.audioClip[phase].length;
        if(prevTime + delta < currentPhaseSec) return defaultResult;
        float loopedTime = prevTime + delta - currentPhaseSec;
        if(audioRunner.loop == false)
        {
            return new PhaseTime
            {
                phase = prevPhase + 1,
                time = loopedTime
            };
        }
        return new PhaseTime{phase = prevPhase, time = loopedTime};
    }

    private int GetBeatNo(float time, float bpm)
    {
        float interval = 60 / (bpm * 4);
        return (int)Mathf.Floor(time / interval );
    }
}