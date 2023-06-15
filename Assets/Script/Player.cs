using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private TemporalStatus stun = new TemporalStatus();
    private TemporalStatus invincible = new TemporalStatus();
    private bool isDead = true;

    private SpriteRenderer sprite;
    private Animator animator;
    private Rigidbody2D rigid;
    private JudgeLineCreator judgeSystem;
    private IScoreManager scoreManager;

    public float moveSpeed = 5f;
    public UnityEvent<int> onHealthChanged;
    public int health { get; private set; }
    public bool isStunned
    {
        get
        {
            return stun.ToBool();
        }
    }
    public bool canHeal
    {
        get
        {
            return health > 0 && health < Constants.MAX_HEALTH;
        }
    }

    void Awake()
    {
        health = Constants.MAX_HEALTH;
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        judgeSystem = GetComponentInChildren<JudgeLineCreator>();
        scoreManager = ScoreManager.Instance;
        isDead = false;
    }
    void OnStageChanged()
    {
        health = Constants.MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        if (!stun)
        {
            InputCommand command = GetInputCommand();
            if (command != null) Attack(command);
        }

        stun.Update();
        invincible.Update();
        SetAnimation();

        if (ShouldCameraFollowPlayer())
        {
            Camera.main.GetComponent<CameraController>().StartFollowing();
        }
    }

    void FixedUpdate()
    {
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 velocity = new Vector2(0f, vertical);

        if (stun || isDead)
        {
            velocity = Vector2.zero;
        }
        else
        {
            velocity = velocity.normalized * moveSpeed;
            rigid.velocity = velocity;
        }
        Move(velocity);
    }

    public void ChangeHealth(int healthDelta)
    {
        if (healthDelta == 0) return;
        health += healthDelta;
        if (health < 0) health = 0;
        if (health == 0) StartCoroutine(CallGameOver());
        onHealthChanged?.Invoke(health);
    }

    InputCommand GetInputCommand()
    {
        if (Input.GetKeyDown(KeyCode.Z)) return new InputCommand(AttackKey.z);
        if (Input.GetKeyDown(KeyCode.X)) return new InputCommand(AttackKey.x);
        if (Input.GetKeyDown(KeyCode.C)) return new InputCommand(AttackKey.c);
        if (Input.GetKeyDown(KeyCode.V)) return new InputCommand(AttackKey.v);
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
        if (judgement != Judgement.early) judgeSystem.RemoveCurrentNode();
        switch (judgement)
        {
            case Judgement.perfect:
                CastAttack(2, command.keys);
                break;
            case Judgement.good:
                CastAttack(1, command.keys);
                break;
            default:
                stun.Activate(Constants.STUN_DURATION);
                scoreManager?.AttackWrongTime();
                break;
        }
    }
    void CastAttack(int power, AttackKey keys)
    {
        Vector2 size = new Vector2(0.5f, 4f);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(rigid.position, size, 0f, Vector2.right, 3f, 1 << Constants.ENEMY_LAYER);

        int hitCount = 0;
        foreach (RaycastHit2D hit in hits)
        {
            IAttackable attackable = hit.collider?.GetComponent<IAttackable>();
            if (attackable != null && attackable.CanDamage(keys))
            {
                hitCount++;
                attackable.Damage(power);
            }
        }
        if (power > 1) scoreManager?.HitEnemyPerfect(hitCount);
        else scoreManager?.HitEnemy(hitCount);
        animator.SetTrigger("attack");
        if (hitCount > 0) SFXManager.Instance?.PlayTrack(keys, power > 1 ? 1.0f : 0.5f);
        else SFXManager.Instance?.PlayBase();
    }
    public void AttackAll()
    {
        int hitCount = 0;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(Vector2.zero, new Vector2(18f, 10f), 0f, Vector2.zero, 0f, 1 << Constants.ENEMY_LAYER);
        foreach (RaycastHit2D hit in hits)
        {
            IAttackable attackable = hit.collider?.GetComponent<IAttackable>();
            if (attackable != null)
            {
                hitCount++;
                attackable.Damage(1);
            }
        }
        scoreManager?.HitEnemy(hitCount);
        animator.SetTrigger("attack");
        // 일명 빅장. 모든 키음이 동시에 나옴
        if (hitCount > 0) SFXManager.Instance?.PlayTrack((AttackKey)0b1111);
    }
    public void Hit(int damage)
    {
        if (invincible) return;
        ChangeHealth(-damage);
        scoreManager?.GetDamagedByEnemy();
        invincible.Activate(Constants.INVINCIBLE_DURATION);
        StartCoroutine(BlinkInvincible(Constants.INVINCIBLE_DURATION));
        animator.SetTrigger("hit");
    }
    private IEnumerator CallGameOver()
    {
        animator.SetTrigger("death");
        isDead = true;

        yield return new WaitForSeconds(1f);
        GameManager.Instance.GameOver();
    }
    private IEnumerator BlinkInvincible(float duration)
    {
        float time=0f;
        while(time < duration)
        {
            if(time % 0.5f < 0.25f) sprite.color = new Color(1f, 1f, 1f, 0.5f);
            else sprite.color = new Color(1f, 1f, 1f, 1f);
            yield return null;
            time += Time.deltaTime;
        }
        sprite.color = new Color(1f, 1f, 1f, 1f);
    }

    private bool ShouldCameraFollowPlayer()
    {
        float playerScreenPosY = Camera.main.WorldToScreenPoint(transform.position).y;
        float screenHeight = Screen.height;

        // Check if the player is touching the upper or lower edge of the screen
        return playerScreenPosY <= 0 || playerScreenPosY >= screenHeight;
    }

    private void SetAnimation()
    {
        Vector2 movement = rigid.velocity;
        if (!Mathf.Approximately(movement.x, 0.0f)) sprite.flipX = (movement.x < 0f);
        animator.SetFloat("moveSpeed", movement.magnitude);
        if (stun) animator.SetBool("stun", true);
        else animator.SetBool("stun", false);
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public class Player : MonoBehaviour
//{
//    private TemporalStatus stun = new TemporalStatus();
//    private TemporalStatus invincible = new TemporalStatus();
//    private bool isDead = true;

//    private SpriteRenderer sprite;
//    private Animator animator;
//    private Rigidbody2D rigid;
//    private JudgeLineCreator judgeSystem;
//    private IScoreManager scoreManager;

//    public float moveSpeed = 5f;
//    public UnityEvent<int> onHealthChanged;
//    public int health { get; private set; }
//    public bool isStunned
//    {
//        get
//        {
//            return stun.ToBool();
//        }
//    }
//    public bool canHeal
//    {
//        get
//        {
//            return health > 0 && health < Constants.MAX_HEALTH;
//        }
//    }

//    void Awake()
//    {
//        health = Constants.MAX_HEALTH;
//        sprite = GetComponent<SpriteRenderer>();
//        animator = GetComponent<Animator>();
//        rigid = GetComponent<Rigidbody2D>();
//        judgeSystem = GetComponentInChildren<JudgeLineCreator>();
//        scoreManager = ScoreManager.Instance;
//        isDead = false;
//    }
//    void OnStageChanged()
//    {
//        health = Constants.MAX_HEALTH;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (isDead) return;
//        if (!stun)
//        {
//            InputCommand command = GetInputCommand();
//            if (command != null) Attack(command);
//        }

//        stun.Update();
//        invincible.Update();
//        SetAnimation();

//        if (ShouldCameraFollowPlayer())
//        {
//            Camera.main.GetComponent<CameraController>().StartFollowing();
//        }

//        //if (Input.GetKeyDown(KeyCode.Z)) Attack(AttackKey.z);
//        //if (Input.GetKeyDown(KeyCode.X)) Attack(AttackKey.x);
//        //if (Input.GetKeyDown(KeyCode.C)) Attack(AttackKey.c);
//        //if (Input.GetKeyDown(KeyCode.V)) Attack(AttackKey.v);

//    }

//    void FixedUpdate()
//    {

//        float vertical = Input.GetAxisRaw("Vertical");

//        Vector2 velocity = new Vector2(0f, vertical);

//        if (stun || isDead) velocity = Vector2.zero;
//        else
//        {
//            velocity = velocity.normalized * moveSpeed;
//            rigid.velocity = velocity;
//        }
//        Move(velocity);
//    }

//    public void ChangeHealth(int healthDelta)
//    {
//        if (healthDelta == 0) return;
//        this.health += healthDelta;
//        if (this.health < 0) this.health = 0;
//        if (this.health == 0) StartCoroutine(CallGameOver());
//        onHealthChanged?.Invoke(this.health);
//    }

//    InputCommand GetInputCommand()
//    {
//        if (Input.GetKeyDown(KeyCode.Z)) return new InputCommand(AttackKey.z);
//        if (Input.GetKeyDown(KeyCode.X)) return new InputCommand(AttackKey.x);
//        if (Input.GetKeyDown(KeyCode.C)) return new InputCommand(AttackKey.c);
//        if (Input.GetKeyDown(KeyCode.V)) return new InputCommand(AttackKey.v);
//        return null;
//    }

//    void Move(Vector2 velocity)
//    {
//        velocity = velocity.normalized * moveSpeed;
//        rigid.velocity = velocity;
//    }

//    void Attack(InputCommand command)
//    {
//        Judgement judgement = judgeSystem.HitNote();
//        if (judgement != Judgement.early) judgeSystem.RemoveCurrentNode();
//        switch (judgement)
//        {
//            case Judgement.perfect:
//                CastAttack(2, command.keys);
//                break;
//            case Judgement.good:
//                CastAttack(1, command.keys);
//                break;
//            default:
//                stun.Activate(Constants.STUN_DURATION);
//                scoreManager?.AttackWrongTime();
//                break;
//        }
//    }
//    void CastAttack(int power, AttackKey keys)
//    {
//        RaycastHit2D[] hits = Physics2D.CircleCastAll(rigid.position, Constants.ATTACK_RADIUS, Vector2.zero, 0f, 1 << Constants.ENEMY_LAYER);

//        int hitCount = 0;
//        foreach (RaycastHit2D hit in hits)
//        {
//            IAttackable attackable = hit.collider?.GetComponent<IAttackable>();
//            if (attackable != null && attackable.CanDamage(keys))
//            {
//                hitCount++;
//                attackable.Damage(power);
//            }
//        }
//        if (power > 1) scoreManager?.HitEnemyPerfect(hitCount);
//        else scoreManager?.HitEnemy(hitCount);
//        animator.SetTrigger("attack");
//        if (hitCount > 0) SFXManager.Instance?.PlayTrack(keys);
//    }
//    public void Hit(int damage)
//    {
//        if (invincible) return;
//        ChangeHealth(-damage);
//        scoreManager?.GetDamagedByEnemy();
//        invincible.Activate(Constants.INVINCIBLE_DURATION);
//        animator.SetTrigger("hit");
//    }
//    private IEnumerator CallGameOver()
//    {
//        animator.SetTrigger("death");
//        isDead = true;

//        yield return new WaitForSeconds(1f);
//        GameManager.Instance.GameOver();
//    }

//    private bool ShouldCameraFollowPlayer()
//    {
//        float playerScreenPosY = Camera.main.WorldToScreenPoint(transform.position).y;
//        float screenHeight = Screen.height;

//        // Check if the player is touching the upper or lower edge of the screen
//        return playerScreenPosY <= 0 || playerScreenPosY >= screenHeight;
//    }

//    private void SetAnimation()
//    {
//        Vector2 movement = rigid.velocity;
//        if (!Mathf.Approximately(movement.x, 0.0f)) sprite.flipX = (movement.x < 0f);
//        animator.SetFloat("moveSpeed", movement.magnitude);
//        if (stun) animator.SetBool("stun", true);
//        else animator.SetBool("stun", false);
//    }
//}