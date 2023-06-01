using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private TemporalStatus stun = new TemporalStatus();
    private TemporalStatus invincible = new TemporalStatus();

    private Rigidbody2D rigid;
    private JudgeLineCreator judgeSystem;

    public float moveSpeed = 5f;
    public UnityEvent<int> onHealthChanged;
    public int health {get; private set;}
    public bool isStunned
    {
        get
        {
            return stun.ToBool();
        }
    }

    void Awake()
    {
        health = Constants.MAX_HEALTH;
        rigid = GetComponent<Rigidbody2D>();
        judgeSystem = GetComponentInChildren<JudgeLineCreator>();
    }

    void Start()
    {
    }

    void OnStageChanged()
    {
        health = Constants.MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        if(!stun)
        {
            InputCommand command = GetInputCommand();
            if(command != null) Attack(command);
        }
        stun.Update();
        invincible.Update();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 velocity;
        if(stun) velocity = Vector2.zero;
        else velocity = new Vector2(horizontal, vertical);

        Move(velocity);
    }

    public void ChangeHealth(int healthDelta)
    {
        if(healthDelta == 0) return;
        this.health += healthDelta;
        if(this.health < 0)
        {
            this.health = 0;
        }
        onHealthChanged?.Invoke(this.health);
    }

    InputCommand GetInputCommand()
    {
        if(Input.GetKeyDown(KeyCode.Z)) return new InputCommand(AttackKey.z);
        if(Input.GetKeyDown(KeyCode.X)) return new InputCommand(AttackKey.x);
        if(Input.GetKeyDown(KeyCode.C)) return new InputCommand(AttackKey.c);
        if(Input.GetKeyDown(KeyCode.V)) return new InputCommand(AttackKey.v);
        return null;
    }

    void Move(Vector2 velocity)
    {
        velocity = velocity.normalized * moveSpeed;
        rigid.velocity = velocity;
    }

    void Attack(InputCommand command)
    {
        Judgement judgement = judgeSystem.HitNote();
        if(judgement != Judgement.early) judgeSystem.RemoveCurrentNode();
        switch(judgement)
        {
            case Judgement.perfect:
                CastAttack(2, command.keys);
                Debug.Log("perfect attacked!");
                break;
            case Judgement.good:
                CastAttack(1, command.keys);
                Debug.Log("good attacked!");
                break;
            default:
                stun.Activate(Constants.STUN_DURATION);
                Debug.Log("missed!");
                break;
        }
    }
    void CastAttack(int power, AttackKey keys)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(rigid.position, Constants.ATTACK_RADIUS, Vector2.zero, 0f, 1 << Constants.ENEMY_LAYER);
        foreach(RaycastHit2D hit in hits)
        {
            IAttackable attackable = hit.collider?.GetComponent<IAttackable>();
            if(attackable != null && attackable.CanDamage(keys)) attackable.Damage(power);
        }
    }
    public void Hit(int damage)
    {
        if(invincible) return;
        ChangeHealth(-damage);
        invincible.Activate(Constants.INVINCIBLE_DURATION);
    }
}
