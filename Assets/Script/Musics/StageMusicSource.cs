using UnityEngine;

[CreateAssetMenu(fileName = "stageMusic", menuName = "SoundSource/StageMusic", order = 1)]
public class StageMusicSource : MusicSource
{
    public AudioClip audioClip;
    [SerializeField] private string notePattern="";
    public override bool HasNote(int beat)
    {
        if(notePattern == null) return beat % 4 == 0;
        if(beat < 0) return beat % 4 == 0;
        if(beat >= notePattern.Length) return beat % 4 == 0;
        return notePattern[beat] == 'o';
    }
}