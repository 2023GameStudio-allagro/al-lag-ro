using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IResultManager
{
    public void SetScoreData(ScoreData scoreData);
}

public class ScoreResultManager : MonoBehaviour, IResultManager
{
    private TextMeshProUGUI MaxComboText;
    private TextMeshProUGUI ScoreText;
    [SerializeField] private GameObject MaxComboTextUI;
    [SerializeField] private GameObject ScoreTextUI;
    private void Awake()
    {
        MaxComboText = MaxComboTextUI.GetComponent<TextMeshProUGUI>();
        ScoreText = ScoreTextUI.GetComponent<TextMeshProUGUI>();
    }
    public void SetScoreData(ScoreData scoreData)
    {
        ChangeText(MaxComboText, $"Max Combo : {scoreData.combo}");
        ChangeText(ScoreText, $"Total Score : {scoreData.score}");
    }
    private void ChangeText(TextMeshProUGUI container, string text)
    {
        container.SetText(text);
    }
}