using System.Collections;
using UnityEngine;

public class WaitTimePauseable : IEnumerator
{
    private float delay;
    private float elapsedTime;

    public WaitTimePauseable(float seconds)
    {
        delay = seconds;
        elapsedTime = 0f;
    }

    public object Current { get { return null; } }

    public bool MoveNext()
    {
        if(ShouldPlay()) elapsedTime += Time.deltaTime;
        return elapsedTime < delay;
    }

    public void Reset()
    {
        elapsedTime = 0f;
    }

    public bool ShouldPlay()
    {
        return true;
    }
}