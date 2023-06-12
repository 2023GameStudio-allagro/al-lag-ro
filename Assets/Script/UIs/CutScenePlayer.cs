using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CutScenePlayer : MonoBehaviour, IPointerClickHandler
{
    private int _cutsceneNo;
    private Image image;
    [SerializeField] private Sprite[] cutscenes;
    [SerializeField] private GameObject touchToNextIndicator;
    public UnityEvent onEndCutscene;
    public int cutsceneNo
    {
        get {return _cutsceneNo;}
    }
    void Awake()
    {
        image = GetComponent<Image>();
        _cutsceneNo = 0;
    }
    void Start()
    {
        touchToNextIndicator.SetActive(false);
        StartCoroutine(ShowIndicator());
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        touchToNextIndicator.SetActive(false);
        NextCutscene();
        StartCoroutine(ShowIndicator());
    }
    private void NextCutscene()
    {
        _cutsceneNo++;
        if(_cutsceneNo >= cutscenes.Length)
        {
            onEndCutscene?.Invoke();
            return;
        }
        image.sprite = cutscenes[_cutsceneNo];
    }
    private IEnumerator ShowIndicator()
    {
        yield return new WaitForSeconds(3f);
        touchToNextIndicator.SetActive(true);
    }
}
