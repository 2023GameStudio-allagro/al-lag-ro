using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboText : MonoBehaviour
{
    private TextMeshProUGUI textUI;

    void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    public void OnChangeCombo(int combo)
    {
        if(combo <= 0) textUI.SetText("");
        else textUI.SetText(combo.ToString() + " Combo");
    }
}
