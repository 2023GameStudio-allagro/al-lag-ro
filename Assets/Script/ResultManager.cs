using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResultManager
{
    public void SetScoreData(ScoreData scoreData);
}

public class ScoreResultManager : MonoBehaviour, IResultManager
{
    public void SetScoreData(ScoreData scoreData)
    {
        // 대충 스코어 데이터 넣는다는 내용
    }
}