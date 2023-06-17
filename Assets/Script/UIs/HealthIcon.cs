using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIcon : MonoBehaviour
{
    private bool isActive = true;
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
        if(!isActive || beatNo % 4 != 0) return;
        animator.SetTrigger("Glitter");
    }
    public void Activate(bool isActive)
    {
        this.isActive = isActive;
        animator.SetBool("Active", isActive);
    }
}
