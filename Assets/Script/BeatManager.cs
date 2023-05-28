using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float interval
    {
        get{return 60f / bpm;}
    }
    private bool isRunning = false;
    public float bpm = 150f;
    public float offset = 0f;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isRunning) return;
        float prevElapsedTime = elapsedTime;
        elapsedTime += Time.deltaTime;
    }

    public void StartBeat()
    {
        elapsedTime = -3f - offset;
        isRunning = true;
    }
    public void ResumeBeat()
    {
        isRunning = true;
    }
    public void EndBeat()
    {
        isRunning = false;
    }

    private int Get16BeatCount(float time, float offset = 0f)
    {
        return (int)Mathf.Floor((time - offset) / (interval*4));
    }
    private bool IsMovedNextBeat(float prevTime, float curTime, float offset = 0f)
    {
        int prevBeat = Get16BeatCount(prevTime, offset);
        int curBeat = Get16BeatCount(curTime, offset);
        return (prevBeat != curBeat);
    }
}
