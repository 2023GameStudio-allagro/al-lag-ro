using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMarkerUI : MonoBehaviour
{
    private bool isSetted = false;
    private GameObject[] iconObjects;
    private SpriteRenderer[] iconRenderers;
    private int maxHealth;
    private float iconSpacing = 0.6f;
    [SerializeField] Sprite[] markerIcons;

    public void SetInitialMarker(List<AttackKey> markers)
    {
        if(isSetted) ClearMarker();
        isSetted = true;
        this.maxHealth = markers.Count;

        iconObjects = new GameObject[this.maxHealth];
        iconRenderers = new SpriteRenderer[this.maxHealth];
        
        for (int i = 0; i < this.maxHealth; i++)
        {
            GameObject iconObject = new GameObject("MarkerIcon" + i);
            iconObject.transform.SetParent(transform);
            SpriteRenderer spriteRenderer = iconObject.AddComponent<SpriteRenderer>();
            iconObjects[i] = iconObject;
            iconRenderers[i] = spriteRenderer;
        }

        SetMarker(markers);
    }
    public void SetMarker(List<AttackKey> markers)
    {
        int currentHealth = markers.Count;
        float width = iconSpacing * (currentHealth - 1);

        for (int i = 0; i < this.maxHealth; i++)
        {
            if(i < currentHealth)
            {
                iconObjects[i].SetActive(true);
                iconRenderers[i].sprite = markerIcons[(int)Mathf.Log((int)markers[i], 2)];
                float offsetX = i * iconSpacing - width / 2;
                iconObjects[i].transform.localPosition = new Vector3(offsetX, 0f, 0f);
            }
            else
            {
                iconObjects[i].SetActive(false);
            }
        }
    }
    private void ClearMarker()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
