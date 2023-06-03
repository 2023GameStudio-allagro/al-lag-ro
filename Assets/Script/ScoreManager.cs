using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : Singletone<ScoreManager>, IScoreManager
{
    private int _score;
    private int _combo;
    public UnityEvent<int> OnChangeScore;
    public UnityEvent<int> OnChangeCombo;
    public int score
    {
        get
        {
            return _score;
        }
        private set
        {
            int newScore = (value < 0) ? 0 : value;
            if(_score != newScore) OnChangeScore?.Invoke(newScore);
            _score = newScore;
        }
    }
    public int combo
    {
        get
        {
            return _combo;
        }
        private set
        {
            if(_combo != value) OnChangeCombo?.Invoke(value);
            _combo = value;
        }
    }
    void Start()
    {
        InitializeScore();
    }
    public void HitEnemy(int count)
    {
        combo += count;
        score += count * combo * 10;
    }
    public void HitEnemyPerfect(int count)
    {
        combo += count;
        score += count * combo * 10;
    }
    public void AttackWrongTime()
    {
        combo = 0;
        score -= 200;
    }
    public void Miss()
    {
        combo = 0;
    }
    public void GetDamagedByEnemy()
    {
        combo = 0;
        score -= 1000;
    }
    private void InitializeScore()
    {
        _score = 0;
        _combo = 0;
    }
}
