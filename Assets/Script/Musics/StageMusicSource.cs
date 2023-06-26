using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "stageMusic", menuName = "SoundSource/StageMusic", order = 1)]
public class StageMusicSource : MusicSource
{
    public AudioClip audioClip;
    [SerializeField, TextArea(5, 12)] private string notePattern="";
    public EnemyData[] enemyRawData;

    private int _firstNoteTiming = 0;
    public int firstNoteTiming
    {
        get { return _firstNoteTiming; }
    }
    private Dictionary<char, EnemyData> enemyDataReal;
    private Dictionary<int, char> notePatternReal;

    public void Initialize()
    {
        enemyDataReal = new Dictionary<char, EnemyData>();
        notePatternReal = new Dictionary<int, char>();
        foreach(var eachEnemyData in enemyRawData)
        {
            enemyDataReal[eachEnemyData.identifier] = eachEnemyData;
        }
        int notePatternLength = notePattern.Length;
        for(int i=0; i<notePatternLength; i++)
        {
            char note = notePattern[i];
            if(note == '.') continue;
            if(note == 'o') note = GetRandomNote();
            if(!enemyDataReal.ContainsKey(note)) continue;

            int duration = (int)Mathf.Round(4 * Constants.BASE_DURATION_BEAT / enemyDataReal[note].speed);
            notePatternReal[i - duration] = note;
            if(_firstNoteTiming > i - duration) _firstNoteTiming = i - duration;
        }
    }
    public override bool HasNote(int beat)
    {
        Nullable<bool> exceptional = GetExceptionNote(beat);
        if(exceptional != null) return exceptional.Value;
        if(beat >= 4 && notePattern[beat - 4] == 'v') return true; //리팩터링 예정임... 일단은 하드코딩
        return notePattern[beat] != '.';
    }
    public char GetNote(int beat)
    {
        Nullable<bool> exceptional = GetExceptionNote(beat);
        if(exceptional != null) return exceptional.Value ? 'z' : '.';

        char note;
        notePatternReal.TryGetValue(beat, out note);
        return note;
    }
    private Nullable<bool> GetExceptionNote(int beat)
    {
        if(notePattern == null) return beat % 4 == 0;
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