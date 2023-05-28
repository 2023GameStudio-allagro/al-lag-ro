using UnityEngine;

[CreateAssetMenu(fileName = "bossMusic", menuName = "SoundSource/BossMusic", order = 1)]
public class BossMusicSource : MusicSource
{
    public AudioClip[] audioClip;
    public int phases{ get{return audioClip.Length;} }
    [SerializeField] private string[] notePattern;
    public override bool HasNote(int beat)
    {
        return beat % 4 == 0;
    }
    public bool HasNote(int beat, int phase)
    {
        if(phase < 0 || phase >= this.phases) return beat % 4 == 0;

        string currentNotePattern = notePattern[phase];
        if(currentNotePattern == null) return beat % 4 == 0;
        if(beat < 0) return beat % 4 == 0;
        if(beat >= currentNotePattern.Length) return beat % 4 == 0;
        return currentNotePattern[beat] == 'o';
    }
}