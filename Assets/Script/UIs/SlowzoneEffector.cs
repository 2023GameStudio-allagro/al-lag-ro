using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowzoneEffector : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Activate()
    {
        animator.SetBool("activate", true);
    }
    public void Deactivate()
    {
        animator.SetBool("activate", false);
    }
}
