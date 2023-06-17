using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeTextMaker : MonoBehaviour
{
    [SerializeField] private Sprite perfectText;
    [SerializeField] private Sprite goodText;
    [SerializeField] private Sprite badText;
    [SerializeField] private GameObject judgeTextPrefab;
    [SerializeField] private Transform parentObject;
    
    public void MakeJudgeText(Judgement judgement)
    {
        GameObject obj;
        if(parentObject == null) obj = Instantiate(judgeTextPrefab);
        else obj = Instantiate(judgeTextPrefab, parentObject);
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sprite = GetSprite(judgement);
    }
    private Sprite GetSprite(Judgement judgement)
    {
        switch(judgement)
        {
            case Judgement.perfect: return perfectText;
            case Judgement.good: return goodText;
            default: return badText;
        }
    }
}
