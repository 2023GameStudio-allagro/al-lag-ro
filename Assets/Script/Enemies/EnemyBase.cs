using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IAttackable
{
    private Animator animator;
    public int hp{get; protected set;}
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void Damage(int damage)
    {
        Debug.Log($"{damage}를 입었다! 으악!");
        animator?.SetTrigger("hit");
    }

    protected virtual void OnDead()
    {
    }
}
