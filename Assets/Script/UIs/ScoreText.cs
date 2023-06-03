using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI textUI;

    void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    public void OnChangeScore(int score)
    {
        textUI.SetText(score.ToString());
    }
}
