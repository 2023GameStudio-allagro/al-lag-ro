using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIcon : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        MusicManager.Instance?.beatEvent.AddListener(Glitter);
    }
    void OnDisable()
    {
        MusicManager.Instance?.beatEvent.RemoveListener(Glitter);
    }

    public void Glitter(int beatNo)
    {
        if(beatNo % 4 != 0) return;
        animator.SetTrigger("Glitter");
    }
}
