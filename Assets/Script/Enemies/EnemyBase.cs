using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IAttackable
{
    private Animator animator;
    private Rigidbody2D rigid;
    public int hp{get; protected set;}
    public int moveSpeed{get; protected set;}
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        MoveTowardPlayer();
    }

    protected void MoveTowardPlayer()
    {
        if(player == null || player.Equals(null)) return;
        Vector3 velocity = (player.transform.position - transform.position);
        if(velocity.magnitude < 0.5f) return;
        velocity = velocity.normalized * moveSpeed;
        rigid.velocity = velocity;
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
