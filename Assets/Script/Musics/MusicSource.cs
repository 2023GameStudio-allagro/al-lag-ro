using UnityEngine;

public abstract class MusicSource : ScriptableObject
{
    public float bpm;
    public float offset;
    public abstract bool HasNote(int beat);
}