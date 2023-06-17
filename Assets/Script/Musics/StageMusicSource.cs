using UnityEngine;
using System;

[CreateAssetMenu(fileName = "stageMusic", menuName = "SoundSource/StageMusic", order = 1)]
public class StageMusicSource : MusicSource
{
    public AudioClip audioClip;
    [SerializeField, TextArea(5, 12)] private string notePattern="";
    public override bool HasNote(int beat)
    {
        Nullable<bool> exceptional = GetExceptionNote(beat);
        if(exceptional != null) return exceptional.Value;
        return notePattern[beat] != '.';
    }
    public char GetNote(int beat)
    {
        Nullable<bool> exceptional = GetExceptionNote(beat);
        if(exceptional != null) return exceptional.Value ? 'z' : '.';
        switch(notePattern[beat])
        {
            case 'o': return GetRandomNote();
            case 'z':
            case 'Z': return 'z';
            case 'x':
            case 'X': return 'x';
            case 'c':
            case 'C': return 'c';
            case 'v':
            case 'V': return 'v';
            case 's': return 's';
            default: return ' ';
        }
    }
    private Nullable<bool> GetExceptionNote(int beat)
    {
        if(notePattern == null) return beat % 4 == 0;
        if(beat < 0) return beat % 4 == 0;
        if(beat >= notePattern.Length) return beat % 4 == 0;
        return null;
    }
    private char GetRandomNote()
    {
        float random = UnityEngine.Random.Range(0f, 1f);
        if(random < 0.4) return 'z';
        if(random < 0.7) return 'x';
        if(random < 0.9) return 'c';
        if(random < 0.96) return 'v';
        return 's';
    }
}