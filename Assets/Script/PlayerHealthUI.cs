using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealthUI : MonoBehaviour
{
    private GameObject[] healthIcons;
    [SerializeField] private GameObject healthIconPrefab;

    // Start is called before the first frame update
    void Start()
    {
        const float OFFSET = 15f;
        healthIcons = new GameObject[Constants.MAX_HEALTH];
        for(int i=0; i<Constants.MAX_HEALTH; i++)
        {
            GameObject healthIconObj = Instantiate(healthIconPrefab, transform);
            RectTransform rectTransform = healthIconObj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(6f + OFFSET*i, 0f);
            healthIcons[i] = healthIconObj;
        }
    }

    public void OnHealthChanged(int health)
    {
        for(int i=0; i<Constants.MAX_HEALTH; i++)
        {
            healthIcons[i].SetActive(i < health);
        }
    }
}
